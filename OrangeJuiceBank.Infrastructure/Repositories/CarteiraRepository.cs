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
    public class CarteiraRepository :  ICarteiraRepository
    {
        private readonly AppDbContext _context;  
        
        public CarteiraRepository(AppDbContext context) {
            _context = context;
        }

        public async Task<Carteira?> GetByUsuarioAndAtivoAsync(Guid usuarioId, Guid ativoId)
        {
            return await _context.Carteiras
                .Include(c => c.Ativo)
                .FirstOrDefaultAsync(c => c.UsuarioId == usuarioId && c.AtivoId == ativoId);
        }

        public async Task<IEnumerable<Carteira>> GetByUsuarioIdAsync(Guid usuarioId)
        {
            return await _context.Carteiras
            .Include(u => u.Ativo)
            .Where(u => u.UsuarioId == usuarioId).ToListAsync();
        }

        public async Task<Carteira> AddAsync(Carteira carteira)
        {
            await _context.Carteiras.AddAsync(carteira);
            await _context.SaveChangesAsync();
            return carteira;
        }

        public async Task<Carteira> UpdateAsync(Carteira carteira)
        {
            _context.Carteiras.Update(carteira);
            await _context.SaveChangesAsync();
            return carteira;
        }

        public async Task DeleteAsync(Guid id)
        {
            var carteira = await _context.Carteiras.FindAsync(id);
            if (carteira != null)
            {
                _context.Carteiras.Remove(carteira);
                await _context.SaveChangesAsync();
            }
        }

        public Task<Carteira?> GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Carteira>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        Task IRepository<Carteira>.UpdateAsync(Carteira entity)
        {
            return UpdateAsync(entity);
        }
    }
}