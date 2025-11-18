using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OrangeJuiceBank.Domain.Entities;
using OrangeJuiceBank.Domain.Enums;

namespace OrangeJuiceBank.Infrastructure.Data
{
    public class DbInitializer
    {
        public static void Initialize(AppDbContext context)
        {
            context.Database.EnsureCreated();

            if (context.Usuarios.Any())
            {
                return;
            }

            // Criar usuários
            var usuarios = new List<Usuario>
            {
                new() {
                    Id = Guid.NewGuid(),
                    Nome = "Gustavo Reis",
                    Email = "greis@orangejuice.com",
                    CPF = "12345678901",
                    SenhaHash = "hash_senha_123", // Em produção seria hash real
                    DataCriacao = DateTime.UtcNow
                },
                new() {
                    Id = Guid.NewGuid(),
                    Nome = "Maria Santos",
                    Email = "maria@orangejuice.com",
                    CPF = "98765432100",
                    SenhaHash = "hash_senha_456",
                    DataCriacao = DateTime.UtcNow
                },
                new() {
                    Id = Guid.NewGuid(),
                    Nome = "Pedro Costa",
                    Email = "pedro@orangejuice.com",
                    CPF = "45678912300",
                    SenhaHash = "hash_senha_789",
                    DataCriacao = DateTime.UtcNow
                }
            };
            context.Usuarios.AddRange(usuarios);
            context.SaveChanges();

            var contas = new List<Conta>();

            foreach (var usuario in usuarios)
            {
                //CONTA CORRENTE
                contas.Add(new Conta
                {
                    Id = Guid.NewGuid(),
                    NumeroConta = $"CI-{usuario.CPF.Substring(0, 6)}",
                    Saldo = 10000m,
                    TipoConta = TipoConta.Corrente,
                    UsuarioId = usuario.Id,
                    DataCriacao = DateTime.UtcNow

                });

                // CONTA INVESTIMENTO
                contas.Add(new Conta
                {
                    Id = Guid.NewGuid(),
                    NumeroConta = $"CI-{usuario.CPF.Substring(0, 6)}",
                    Saldo = 5000m,
                    TipoConta = TipoConta.Investimento,
                    UsuarioId = usuario.Id,
                    DataCriacao = DateTime.UtcNow
                });
            }

            context.Contas.AddRange(contas);
            context.SaveChanges();

            var ativos = new List<Ativo>
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    Codigo = "ORNG3",
                    Nome = "Orange Juice S.A",
                    PrecoAtual = 25.50m,
                    TipoAtivo = TipoAtivo.Acao,
                    TaxaRentabilidadeAnual = null,
                    DataAtualizacao = DateTime.UtcNow
                },
                new()
                {
                Id = Guid.NewGuid(),
                Codigo = "FRUT4",
                Nome = "Frutaria Tech",
                PrecoAtual = 38.20m,
                TipoAtivo = TipoAtivo.Acao,
                TaxaRentabilidadeAnual = null,
                DataAtualizacao = DateTime.UtcNow
                },
                new()
                {
                    Id = Guid.NewGuid(),
                    Codigo = "BANK5",
                    Nome = "Banco Digital Plus",
                    PrecoAtual = 15.75m,
                    TipoAtivo = TipoAtivo.Acao,
                    TaxaRentabilidadeAnual = null,
                    DataAtualizacao = DateTime.UtcNow
                },
                
                // CDBs
                new()
                {
                    Id = Guid.NewGuid(),
                    Codigo = "CDB001",
                    Nome = "CDB Banco Orange 120% CDI",
                    PrecoAtual = 1000m,
                    TipoAtivo = TipoAtivo.CDB,
                    TaxaRentabilidadeAnual = 13.65m, // 120% do CDI (~13.65% ao ano)
                    DataAtualizacao = DateTime.UtcNow
                },
                new()
                {
                    Id = Guid.NewGuid(),
                    Codigo = "CDB002",
                    Nome = "CDB Prefixado 14% a.a.",
                    PrecoAtual = 1000m,
                    TipoAtivo = TipoAtivo.CDB,
                    TaxaRentabilidadeAnual = 14m,
                    DataAtualizacao = DateTime.UtcNow
                },
                
                // Tesouro Direto
                new() {
                    Id = Guid.NewGuid(),
                    Codigo = "SELIC2027",
                    Nome = "Tesouro Selic 2027",
                    PrecoAtual = 10500m,
                    TipoAtivo = TipoAtivo.TesouroDireto,
                    TaxaRentabilidadeAnual = 11.75m,
                    DataAtualizacao = DateTime.UtcNow
                },
                new()
                {
                    Id = Guid.NewGuid(),
                    Codigo = "IPCA2029",
                    Nome = "Tesouro IPCA+ 2029",
                    PrecoAtual = 3200m,
                    TipoAtivo = TipoAtivo.TesouroDireto,
                    TaxaRentabilidadeAnual = 6.5m, // IPCA + 6.5%
                    DataAtualizacao = DateTime.UtcNow
                }
            };

            context.Ativos.AddRange(ativos);
            context.SaveChanges();
        }
    }
}