using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OrangeJuiceBank.Domain.Entities;

namespace OrangeJuiceBank.Domain.Interfaces
{
    public interface ICarteiraRepository : IRepository<Carteira>
    {
        Task<IEnumerable<Carteira>> GetByUsuarioIdAsync(Guid usuarioId);
        Task<Carteira?> GetByUsuarioAndAtivoAsync(Guid usuarioId, Guid ativoId);
        Task<Carteira> AddAsync(Carteira carteira);
        Task<Carteira> UpdateAsync(Carteira carteira);
        Task DeleteAsync(Guid id);
    }
}