using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OrangeJuiceBank.Domain.Entities;

namespace OrangeJuiceBank.Domain.Interfaces
{
    public interface IContaRepository : IRepository<Conta>
    {
        Task<IEnumerable<Conta>> GetByUsuarioIdAsync(Guid usuarioId);
        Task<Conta?> GetByNumeroContaAsync(string numeroConta);
    }
}