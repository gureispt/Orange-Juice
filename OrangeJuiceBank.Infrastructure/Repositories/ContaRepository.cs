using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OrangeJuiceBank.Domain.Entities;
using OrangeJuiceBank.Domain.Interfaces;
using OrangeJuiceBank.Infrastructure.Data;

namespace OrangeJuiceBank.Infrastructure.Repositories
{
    public class ContaRepository : Repository<Conta>, IContaRepository
    {
        public ContaRepository(AppDbContext context) : base(context){}

        public async Task<Conta?> GetByNumeroContaAsync(string numeroConta)
        {
            return await _dbSet.FirstOrDefaultAsync(num => num.NumeroConta == numeroConta);
        }

        public async Task<IEnumerable<Conta>> GetByUsuarioIdAsync(Guid usuarioId)
        {
            return await _dbSet.Where(user => user.UsuarioId == usuarioId).ToListAsync();
        }
    }
}