using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OrangeJuiceBank.Domain.Entities;
using OrangeJuiceBank.Domain.Enums;

namespace OrangeJuiceBank.Domain.Interfaces
{
    public interface ITransacaoRepository : IRepository<Transacao>
    {
        Task<IEnumerable<Transacao>> GetByContaIdAsync(Guid contaId);
        Task<IEnumerable<Transacao>> GetByUsuarioIdAsync(Guid usuarioId);
        Task<IEnumerable<Transacao>> GetByTipoAsync(TipoTransacao tipo);
        Task<IEnumerable<Transacao>> GetByPeriodoAsync(Guid usuarioId, DateTime dataInicio, DateTime dataFim);
        Task<Transacao> AddAsync(Transacao transacao);
    }
}