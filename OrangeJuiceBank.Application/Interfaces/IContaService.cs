using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OrangeJuiceBank.Domain.Entities;

namespace OrangeJuiceBank.Application.Interfaces
{
    public interface IContaService
    {
        Task<Conta?> ObterPorIdAsync(Guid id);
        Task<IEnumerable<Conta>> ObterPorUsuarioIdAsync(Guid usuarioId);
        Task DepositarAsync(Guid contaId, decimal valor);
        Task SacarAsync(Guid contaId, decimal valor);
        Task TransferirInternaAsync(Guid contaOrigemId, Guid contaDestinoId, decimal valor);
        Task TransferirExternaAsync(Guid contaOrigemId, Guid contaDestinoId, decimal valor);
    }
}