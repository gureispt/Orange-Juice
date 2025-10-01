using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OrangeJuiceBank.Domain.Enums;

namespace OrangeJuiceBank.Domain.Entities
{
    public class Conta
    {
        public Guid Id { get; set; }
        public required string NumeroConta { get; set; }
        public decimal Saldo { get; set; }
        public TipoConta TipoConta { get; set; }
        public Guid UsuarioId { get; set; }
        public DateTime DataCriacao { get; set; }

        //RELAÇÃO COM AS OUTRAS CLASSES
        public Usuario Usuario { get; set; } = null!;// 1 -> 1
        public ICollection<Transacao> TransacaoOrigem { get; set; } =  []; // 1 -> N
        public ICollection<Transacao> TransacaoDestino { get; set; } = [];
    }
}