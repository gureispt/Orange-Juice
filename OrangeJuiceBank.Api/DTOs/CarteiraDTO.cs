namespace OrangeJuiceBank.Api.DTOs
{
    public class CarteiraDTO
    {
        public Guid Id { get; set; }
        public Guid AtivoId { get; set; }
        public int Quantidade { get; set; }
        public decimal PrecoMedioCompra { get; set; }
        public DateTime DataPrimeiraCompra { get; set; }
        public DateTime DataUltimaAtualizacao { get; set; }

        //ATIVO
        public string CodigoAtivo { get; set; }
        public string NomeAtivo { get; set; }
        public decimal PrecoAtualAtivo { get; set; }
        public int TipoAtivo { get; set; }

        //CÁLCULOS 
        public decimal ValorInvestido => PrecoMedioCompra * Quantidade;
        public decimal ValorAtual => PrecoAtualAtivo * Quantidade;
        public decimal LucroPrejuizo => ValorAtual - ValorInvestido;
        public decimal PercentualRetorno => ValorInvestido > 0 
            ? ((ValorAtual - ValorInvestido) / ValorInvestido) * 100 
            : 0;
    }
}