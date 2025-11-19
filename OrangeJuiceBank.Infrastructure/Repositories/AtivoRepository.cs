using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OrangeJuiceBank.Domain.Entities;
using OrangeJuiceBank.Domain.Enums;
using OrangeJuiceBank.Domain.Interfaces;
using OrangeJuiceBank.Infrastructure.Data;

namespace OrangeJuiceBank.Infrastructure.Repositories
{
    public class AtivoRepository : Repository<Ativo>, IAtivoRepository
    {
        public AtivoRepository(AppDbContext context) : base(context){}

        public async Task<Ativo?> GetByCodigoAsync(string codigo)
        {
            return await _dbSet.FirstOrDefaultAsync(c => c.Codigo == codigo);
        }

        public async Task<IEnumerable<Ativo>> GetByTipoAsync(TipoAtivo tipo)
        {
            return await _dbSet.Where(t => t.TipoAtivo == tipo).ToListAsync();
        }
    }
}