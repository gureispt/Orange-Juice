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
    public class TransacaoRepository : Repository<Transacao>, ITransacaoRepository
    {
        public TransacaoRepository(AppDbContext context) : base(context) { }
        
        public async Task<IEnumerable<Transacao>> GetByContaIdAsync(Guid contaId)
        {
            return await _dbSet
            .Where(t => t.ContaOrigemId == contaId || t.ContaDestinoId == contaId)
            .OrderByDescending(t => t.DataHora)
            .ToListAsync();
        }

        public async Task<IEnumerable<Transacao>> GetByUsuarioIdAsync(Guid usuarioId)
        {
            return await _dbSet
            .Include(u => u.ContaOrigem)
            .Include(u => u.ContaDestino)
            .Where(u => u.ContaOrigem!.UsuarioId == usuarioId || u.ContaDestino!.UsuarioId == usuarioId)
            .OrderByDescending(u => u.DataHora)
            .ToListAsync();
        }

        public async Task<IEnumerable<Transacao>> GetByTipoAsync(TipoTransacao tipo)
        {
            return await _dbSet.Where(ti => ti.TipoTransacao == tipo).ToListAsync();
        }
    }
}