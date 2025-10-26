using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OrangeJuiceBank.Domain.Enums;

namespace OrangeJuiceBank.Domain.Entities
{
    public class Transacao
    {
        public Guid Id { get; set; }
        public Guid? ContaOrigemId { get; set; }
        public Guid? ContaDestinoId { get; set; }
        public Guid? AtivoId { get; set; }
        public decimal Valor { get; set; }
        public int? Quantidade { get; set; }
        public TipoTransacao TipoTransacao { get; set; }
        public decimal TaxaCobrada { get; set; }
        public required string Descricao { get; set; }
        public DateTime DataHora { get; set; }
        public StatusTransacao Status { get; set; }

        //RELAÇÃO COM AS OUTRAS CLASSES: 
        public Conta? ContaOrigem { get; set; }
        public Conta? ContaDestino { get; set; }
        public Ativo? Ativo { get; set; }
    }
}