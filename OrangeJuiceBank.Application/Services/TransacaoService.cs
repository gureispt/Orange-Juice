using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OrangeJuiceBank.Application.Interfaces;
using OrangeJuiceBank.Domain.Entities;
using OrangeJuiceBank.Domain.Interfaces;

namespace OrangeJuiceBank.Application.Services
{
    public class TransacaoService : ITransacaoService
    {
        private readonly ITransacaoRepository _transacaoRepository;

        public TransacaoService(ITransacaoRepository transacaoRepository)
        {
            _transacaoRepository = transacaoRepository;
        }

        public async Task<IEnumerable<Transacao>> ObterPorContaAsync(Guid contaId)
        {
            return await _transacaoRepository.GetByContaIdAsync(contaId);
        }

        public async Task<IEnumerable<Transacao>> ObterPorUsuarioAsync(Guid usuarioId)
        {
            return await _transacaoRepository.GetByUsuarioIdAsync(usuarioId);
        }

        public async Task<IEnumerable<Transacao>> ObterPorPeriodoAsync(Guid usuarioId, DateTime dataInicio, DateTime dataFim)
        {
            return await _transacaoRepository.GetByPeriodoAsync(usuarioId, dataInicio, dataFim);
        }
    }
}