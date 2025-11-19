using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OrangeJuiceBank.Domain.Entities;
using OrangeJuiceBank.Domain.Enums;

namespace OrangeJuiceBank.Domain.Interfaces
{
    public interface IAtivoRepository : IRepository<Ativo>
    {
        Task<Ativo?> GetByCodigoAsync(string codigo);
        Task<IEnumerable<Ativo>> GetByTipoAsync(TipoAtivo tipo);
    }
}