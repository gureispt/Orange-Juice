using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrangeJuiceBank.Domain.Entities
{
    public class Usuario
    {
        public Guid Id { get; set; }
        public required string Nome { get; set; }
        public required string Email { get; set; }
        public required string SenhaHash { get; set; }
        public required string CPF { get; set; }
        public DateTime DataCriacao { get; set; }
    }
}