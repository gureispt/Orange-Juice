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
    public class CarteiraRepository : Repository<Carteira>, ICarteiraRepository
    {
        public CarteiraRepository(AppDbContext context) : base(context){}

        public async Task<Carteira?> GetByUsuarioAndAtivoAsync(Guid usuarioId, Guid ativoId)
        {
            return await _dbSet.FirstOrDefaultAsync(u => u.UsuarioId == usuarioId && u.AtivoId == ativoId);
        }

        public async Task<IEnumerable<Carteira>> GetByUsuarioIdAsync(Guid usuarioId)
        {
            return await _dbSet.Where(u => u.UsuarioId == usuarioId).ToListAsync();
        }
    }
}