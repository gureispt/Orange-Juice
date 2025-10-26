using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OrangeJuiceBank.Domain.Entities;

namespace OrangeJuiceBank.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Transacao> Transacoes { get; set; }
        public DbSet<Conta> Contas { get; set; }
        public DbSet<Carteira> Carteiras { get; set; }
        public DbSet<Ativo> Ativos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configura relacionamento de Transacao com Conta (origem e destino)
            modelBuilder.Entity<Transacao>()
            .HasOne(t => t.ContaOrigem)
            .WithMany(c => c.TransacaoOrigem)
            .HasForeignKey(t => t.ContaOrigemId)
            .OnDelete(DeleteBehavior.Restrict); //Evita que deletar uma Conta delete todas as transações em cascata
            
            modelBuilder.Entity<Transacao>()
            .HasOne(t => t.ContaDestino)
            .WithMany(c => c.TransacaoDestino)
            .HasForeignKey(t => t.ContaDestinoId)
            .OnDelete(DeleteBehavior.Restrict); 
        }
    }
}