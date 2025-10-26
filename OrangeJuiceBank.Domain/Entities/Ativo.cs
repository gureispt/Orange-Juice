using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OrangeJuiceBank.Domain.Enums;

namespace OrangeJuiceBank.Domain.Entities
{
    public class Ativo
    {
        public Guid Id { get; set; }
        public required string Codigo { get; set; }
        public required string Nome { get; set; }
        public decimal PrecoAtual { get; set; }
        public TipoAtivo TipoAtivo { get; set; }
        public decimal? TaxaRentabilidadeAnual { get; set; }
        public DateTime DataAtualizacao { get; set; }

        //RELAÇÃO COM AS OUTRAS CLASSES
        public ICollection<Carteira> Carteiras { get; set; } = [];
        public ICollection<Transacao> Transacoes { get; set; } = [];
    }
}