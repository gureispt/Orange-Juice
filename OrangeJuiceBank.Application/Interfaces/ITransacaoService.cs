using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OrangeJuiceBank.Domain.Entities;

namespace OrangeJuiceBank.Application.Interfaces
{
    public interface ITransacaoService
    {
        Task<IEnumerable<Transacao>> ObterPorContaAsync(Guid contaId);
        Task<IEnumerable<Transacao>> ObterPorUsuarioAsync(Guid usuarioId);
        Task<IEnumerable<Transacao>> ObterPorPeriodoAsync(Guid usuarioId, DateTime dataInicio, DateTime dataFim);
    }
}