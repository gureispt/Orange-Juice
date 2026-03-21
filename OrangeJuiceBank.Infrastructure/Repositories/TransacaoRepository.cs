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
    public class TransacaoRepository : ITransacaoRepository
    {
         private readonly AppDbContext _context;

        public TransacaoRepository(AppDbContext context)
        {
            _context = context;
        }
        
        public async Task<IEnumerable<Transacao>> GetByContaIdAsync(Guid contaId)
        {
            return await _context.Transacoes
            .Where(t => t.ContaOrigemId == contaId || t.ContaDestinoId == contaId)
            .OrderByDescending(t => t.DataHora)
            .ToListAsync();
        }

        public async Task<IEnumerable<Transacao>> GetByUsuarioIdAsync(Guid usuarioId)
        {
            return await _context.Transacoes
            .Include(u => u.ContaOrigem)
            .Include(u => u.ContaDestino)
            .Where(u => u.ContaOrigem!.UsuarioId == usuarioId || u.ContaDestino!.UsuarioId == usuarioId)
            .OrderByDescending(u => u.DataHora)
            .ToListAsync();
        }

        public async Task<IEnumerable<Transacao>> GetByTipoAsync(TipoTransacao tipo)
        {
            return await _context.Transacoes.Where(ti => ti.TipoTransacao == tipo).ToListAsync();
        }

        public async Task<IEnumerable<Transacao>> GetByPeriodoAsync(Guid usuarioId, DateTime dataInicio, DateTime dataFim)
        {
            return await _context.Transacoes
            .Include(t => t.ContaOrigem)
            .Include(t => t.ContaDestino)
            .Include(t => t.Ativo)
            .Where(t => ((t.ContaOrigem != null && t.ContaOrigem.UsuarioId == usuarioId) ||
                    (t.ContaDestino != null && t.ContaDestino.UsuarioId == usuarioId)) &&
                    t.DataHora >= dataInicio && t.DataHora <= dataFim)
            .OrderByDescending(t => t.DataHora)
            .ToListAsync();
        }

        public async Task<Transacao> AddAsync (Transacao transacao)
        {
            await _context.Transacoes.AddAsync(transacao);
            await _context.SaveChangesAsync();
            return transacao;
        }

        public Task<Transacao?> GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Transacao>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(Transacao entity)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}