namespace relatorioInvestimento
{
    public class NotaNegociacao
    {
        public DateTime DataNegociacao { get; set; }
        public string Conta { get; set; }
        public string Ativo { get; set; }
        public decimal Preco { get; set; }
        public int QuantidadeCompra { get; set; }
        public int QuantidadeVenda { get; set; }
        public decimal FinanceiroVenda { get; set; }
        public decimal FinanceiroCompra { get; set; }
    }
}