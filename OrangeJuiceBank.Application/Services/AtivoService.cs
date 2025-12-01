using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OrangeJuiceBank.Application.Interfaces;
using OrangeJuiceBank.Domain.Entities;
using OrangeJuiceBank.Domain.Enums;
using OrangeJuiceBank.Domain.Interfaces;

namespace OrangeJuiceBank.Application.Services
{
    public class AtivoService : IAtivoService
    {
        private readonly IAtivoRepository _ativoRepository;
        private readonly ICarteiraRepository _carteiraRepository;
        private readonly IContaRepository _contaRepository;
        private readonly ITransacaoRepository _transacaoRepository;

        public AtivoService(IAtivoRepository ativoRepository, ICarteiraRepository carteiraRepository,
        IContaRepository contaRepository, ITransacaoRepository transacaoRepository)
        {
            _ativoRepository = ativoRepository;
            _carteiraRepository = carteiraRepository;
            _contaRepository = contaRepository;
            _transacaoRepository = transacaoRepository;
        }

        public async Task<Ativo?> ObterPorIdAsync(Guid id)
        {
            return await _ativoRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Ativo>> ObterPorTipoAsync(TipoAtivo tipo)
        {
            return await _ativoRepository.GetByTipoAsync(tipo);
        }

        public async Task<IEnumerable<Ativo>> ObterTodosAsync()
        {
            return await _ativoRepository.GetAllAsync();
        }

        public async Task ComprarAtivoAsync(Guid usuarioId, Guid ativoId, int quantidade)
        {
            if (quantidade <= 0)
                throw new Exception("Qauntidade deve ser maior que zero");

            var ativo = await _ativoRepository.GetByIdAsync(ativoId);
            if (ativo == null)
                throw new Exception("Ativo não encontrado");

            //buscar conta investimendo do usuário

            var contas = await _contaRepository.GetByUsuarioIdAsync(usuarioId);
            var contaInvestimento = contas.FirstOrDefault(c => c.TipoConta == TipoConta.Investimento);

            if (contaInvestimento == null)
                throw new Exception("Conta Investimento não encontrada");

            decimal valorTotal = ativo.PrecoAtual * quantidade;
            decimal taxaCorretagem = 0;

            //taxa apenas para ações (1%)
            if (ativo.TipoAtivo == TipoAtivo.Acao)
            {
                taxaCorretagem = valorTotal * 0.01m;
            }

            decimal valorComTaxa = valorTotal + taxaCorretagem;

            if (contaInvestimento.Saldo < valorComTaxa)
                throw new Exception($"Saldo insuficiente. Necessário: R$ {valorComTaxa:F2}");

            //atualizar saldo da conta
            contaInvestimento.Saldo -= valorComTaxa;
            await _contaRepository.UpdateAsync(contaInvestimento);

            var posicao = await _carteiraRepository.GetByUsuarioAndAtivoAsync(usuarioId, ativoId);

            if (posicao == null)
            {
                //criar nova posição
                posicao = new Carteira
                {
                    Id = Guid.NewGuid(),
                    UsuarioId = usuarioId,
                    AtivoId = ativoId,
                    Quantidade = quantidade,
                    PrecoMedioCompra = ativo.PrecoAtual,
                    DataPrimeiraCompra = DateTime.UtcNow,
                    DataUltimaAtualizacao = DateTime.UtcNow
                };
                await _carteiraRepository.AddAsync(posicao);
            }
            else
            {
                //atualiza a posção existente
                decimal valorAnterior = posicao.PrecoMedioCompra * posicao.Quantidade;
                decimal valorNovo = ativo.PrecoAtual * quantidade;
                int quantidadeTotal = posicao.Quantidade + quantidade;

                posicao.PrecoMedioCompra = (valorAnterior + valorNovo) / quantidadeTotal;
                posicao.Quantidade = quantidadeTotal;
                posicao.DataUltimaAtualizacao = DateTime.UtcNow;

                await _carteiraRepository.UpdateAsync(posicao);
            }

            var transacao = new Transacao
            {
                Id = Guid.NewGuid(),
                ContaOrigemId = contaInvestimento.Id,
                AtivoId = ativoId,
                Valor = valorTotal,
                Quantidade = quantidade,
                TipoTransacao = TipoTransacao.CompraAtivo,
                TaxaCobrada = taxaCorretagem,
                Descricao = $"Compra de {quantidade} {ativo.Codigo} @ R$ {ativo.PrecoAtual:F2}",
                DataHora = DateTime.UtcNow,
                Status = StatusTransacao.Concluida
            };

            await _transacaoRepository.AddAsync(transacao);
        }

        public async Task VenderAtivoAsync(Guid usuarioId, Guid ativoId, int quantidade)
        {
            if (quantidade <= 0)
                throw new Exception("Quantidade deve ser maior que zero");

            var ativo = await _ativoRepository.GetByIdAsync(ativoId);
            if (ativo == null)
                throw new Exception("Ativo não encontrado");

            var posicao = await _carteiraRepository.GetByUsuarioAndAtivoAsync(usuarioId, ativoId);
            if (posicao == null || posicao.Quantidade < quantidade)
                throw new Exception("Quantidade insuficiente para venda");

            // buscar conta investimento
            var contas = await _contaRepository.GetByUsuarioIdAsync(usuarioId);
            var contaInvestimento = contas.FirstOrDefault(c => c.TipoConta == TipoConta.Investimento);

            if (contaInvestimento == null)
                throw new Exception("Conta de Investimento não encontrada");

            decimal valorVenda = ativo.PrecoAtual * quantidade;
            decimal valorCompra = posicao.PrecoMedioCompra * quantidade;
            decimal lucro = valorVenda - valorCompra;
            decimal impostoRetido = 0;


            if (lucro > 0)
            {
                if (ativo.TipoAtivo == TipoAtivo.Acao)
                {
                    impostoRetido = lucro * 0.15m; //15% para ações
                }
                else
                {
                    impostoRetido = lucro * 0.22m; //22% para renda fixa
                }
            }

            decimal valorLiquido = valorVenda - impostoRetido;

            //atualizar saldo da conta

            contaInvestimento.Saldo += valorLiquido;
            await _contaRepository.UpdateAsync(contaInvestimento);

            //atualizar posição na carteira
            posicao.Quantidade -= quantidade;
            posicao.DataUltimaAtualizacao = DateTime.UtcNow;

            if (posicao.Quantidade == 0)
            {
                await _carteiraRepository.DeleteAsync(posicao.Id);
            }
            else
            {
                await _carteiraRepository.UpdateAsync(posicao);
            }

            var transacao = new Transacao
            {
                Id = Guid.NewGuid(),
                ContaDestinoId = contaInvestimento.Id,
                AtivoId = ativoId,
                Valor = valorVenda,
                Quantidade = quantidade,
                TipoTransacao = TipoTransacao.VendaAtivo,
                TaxaCobrada = impostoRetido,
                Descricao = $"Venda de {quantidade} {ativo.Codigo} @ R$ {ativo.PrecoAtual:F2} (IR: R$ {impostoRetido:F2})",
                DataHora = DateTime.UtcNow,
                Status = StatusTransacao.Concluida
            };

            await _transacaoRepository.AddAsync(transacao);
        }
    }
}