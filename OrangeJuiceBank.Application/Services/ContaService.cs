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
    public class ContaService : IContaService
    {
        private readonly IContaRepository _contaRepository;
        private readonly ITransacaoRepository _transacaoRepository;

        public ContaService(IContaRepository contaRepository, ITransacaoRepository transacaoRepository)
        {
            _contaRepository = contaRepository;
            _transacaoRepository = transacaoRepository;
        }

        public async Task DepositarAsync(Guid contaId, decimal valor)
        {
            if (valor <= 0)
                throw new Exception("Valor deve ser maior que zero");

            var conta = await _contaRepository.GetByIdAsync(contaId);
            if (conta == null)
                throw new Exception("Conta não encontrada");

            if (conta.TipoConta != TipoConta.Corrente)
                throw new Exception("Depósitos só podem ser feitos na Conta Corrente");

            conta.Saldo += valor;
            await _contaRepository.UpdateAsync(conta);


            //REGISTRAR TRANSAÇÃO
            var transacao = new Transacao
            {
                Id = Guid.NewGuid(),
                ContaDestinoId = contaId,
                Valor = valor,
                TipoTransacao = TipoTransacao.Deposito,
                TaxaCobrada = 0,
                Descricao = "Deposito em conta",
                DataHora = DateTime.UtcNow,
                Status = StatusTransacao.Concluida
            };

            await _transacaoRepository.AddAsync(transacao);
        }

        public async Task<Conta?> ObterPorIdAsync(Guid id)
        {
            return await _contaRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Conta>> ObterPorUsuarioIdAsync(Guid usuarioId)
        {
            return await _contaRepository.GetByUsuarioIdAsync(usuarioId);
        }

        public async Task SacarAsync(Guid contaId, decimal valor)
        {
            if (valor <= 0)
                throw new Exception("Valor deve ser maior que zero");

            var conta = await _contaRepository.GetByIdAsync(contaId);
            if (conta == null)
                throw new Exception("Conta não encontrada");

            if (conta.TipoConta != TipoConta.Corrente)
                throw new Exception("Saques só podem ser feitos na Conta Corrente");

            if (conta.Saldo < valor)
                throw new Exception("Saldo insuficiente");

            conta.Saldo -= valor;
            await _contaRepository.UpdateAsync(conta);

            //REGISTRAR TRANSAÇÃO
            var transacao = new Transacao
            {
                Id = Guid.NewGuid(),
                ContaDestinoId = contaId,
                Valor = valor,
                TipoTransacao = TipoTransacao.Saque,
                TaxaCobrada = 0,
                Descricao = "Saque em conta",
                DataHora = DateTime.UtcNow,
                Status = StatusTransacao.Concluida
            };

            await _transacaoRepository.AddAsync(transacao);
        }

        public async Task TransferirExternaAsync(Guid contaOrigemId, Guid contaDestinoId, decimal valor)
        {
            if (valor <= 0)
                throw new Exception("Valor deve ser maior que zero");

            var contaOrigem = await _contaRepository.GetByIdAsync(contaOrigemId);
            var contaDestino = await _contaRepository.GetByIdAsync(contaDestinoId);

            if (contaOrigem == null || contaDestino == null)
                throw new Exception("Conta não encontrada");

            if (contaOrigem.TipoConta != TipoConta.Corrente || contaDestino.TipoConta != TipoConta.Corrente)
                throw new Exception("Transferências externas só podem ser entre Contas Correntes");

            decimal taxa = valor * 0.005m; //0,5%
            decimal valorTotal = valor + taxa;

            if (contaOrigem.Saldo < valorTotal)
                throw new Exception("Saldo insuficiente (inclui taxa de 0,5%)");

            contaOrigem.Saldo -= valorTotal;
            contaDestino.Saldo += valor;

            await _contaRepository.UpdateAsync(contaOrigem);
            await _contaRepository.UpdateAsync(contaDestino);

            var transacao = new Transacao
            {
                Id = Guid.NewGuid(),
                ContaOrigemId = contaOrigemId,
                ContaDestinoId = contaDestinoId,
                Valor = valor,
                TipoTransacao = TipoTransacao.TransferenciaExterna,
                TaxaCobrada = taxa,
                Descricao = $"Transferência externa (taxa: R$ {taxa:F2})",
                DataHora = DateTime.UtcNow,
                Status = StatusTransacao.Concluida
            };

            await _transacaoRepository.AddAsync(transacao);
        }

        public async Task TransferirInternaAsync(Guid contaOrigemId, Guid contaDestinoId, decimal valor)
        {
            if (valor <= 0)
                throw new Exception("Valor deve ser maior que zero");

            var contaOrigem = await _contaRepository.GetByIdAsync(contaOrigemId);
            var contaDestino = await _contaRepository.GetByIdAsync(contaDestinoId);

            if (contaOrigem == null || contaDestino == null)
                throw new Exception("Conta não encontrada");

            if (contaOrigem.UsuarioId != contaDestino.UsuarioId)
                throw new Exception("Contas devem ser do mesmo usuário");

            if (contaOrigem.Saldo < valor)
                throw new Exception("Saldo insuficiente");

            contaOrigem.Saldo -= valor;
            contaDestino.Saldo += valor;

            await _contaRepository.UpdateAsync(contaOrigem);
            await _contaRepository.UpdateAsync(contaDestino);

            // Registrar transação
            var transacao = new Transacao
            {
                Id = Guid.NewGuid(),
                ContaOrigemId = contaOrigemId,
                ContaDestinoId = contaDestinoId,
                Valor = valor,
                TipoTransacao = TipoTransacao.TransferenciaInterna,
                TaxaCobrada = 0,
                Descricao = "Transferência interna entre contas",
                DataHora = DateTime.UtcNow,
                Status = StatusTransacao.Concluida
            };

            await _transacaoRepository.AddAsync(transacao);
        }
    }
}