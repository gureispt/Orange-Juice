using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OrangeJuiceBank.Domain.Entities;
using OrangeJuiceBank.Domain.Enums;

namespace OrangeJuiceBank.Application.Interfaces
{
    public interface IAtivoService
    {
        Task<Ativo?> ObterPorIdAsync(Guid id);
        Task<IEnumerable<Ativo>> ObterTodosAsync();
        Task<IEnumerable<Ativo>> ObterPorTipoAsync(TipoAtivo tipo);
        Task ComprarAtivoAsync(Guid usuarioId, Guid ativoId, int quantidade);
        Task VenderAtivoAsync(Guid usuarioId, Guid ativoId, int quantidade);
    }
}