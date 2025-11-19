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
    public class UsuarioRepository : Repository<Usuario>, IUsuarioRepository
    {
        public UsuarioRepository(AppDbContext context) : base(context) { }
        
        public async Task<Usuario?> GetByCPFAsync(string cpf)
        {
            return await _dbSet.FirstOrDefaultAsync(c => c.CPF == cpf);
        }

        public async Task<Usuario?> GetByEmailAsync(string email)
        {
            return await _dbSet.FirstOrDefaultAsync(e => e.Email == email);
        }
    }
}