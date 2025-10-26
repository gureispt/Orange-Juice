using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrangeJuiceBank.Domain.Entities
{
    public class Carteira
    {
        public Guid Id { get; set; }
        public Guid UsuarioId { get; set; }
        public Guid AtivoId { get; set; }
        public int Quantidade { get; set; }
        public decimal PrecoMedioCompra { get; set; }
        public DateTime DataPrimeiraCompra { get; set; }
        public DateTime DataUltimaAtualizacao { get; set; }

        //RELAÇÃO COM AS OUTRAS CLASSES:
        public Usuario Usuario { get; set; } = null!;
        public Ativo Ativo { get; set; } = null!;
    }
}