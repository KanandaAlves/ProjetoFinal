using System.ComponentModel;

namespace Venda.Models
{
    public class Movimentacao
    {
        public int MovimentacaoId { get; set; }
        [DisplayName("Nome do Produto")]
        public int ProdutoId { get; set; }
        public Produto? Produto { get; set; }
        public int Quantidade { get; set; }
        public DateTime DataMovimentacao { get; set; }
        public TipoMovimentacao Tipo { get; set; }

        public enum TipoMovimentacao
        {
            Entrada = 0,
            Saida = 1
        }
    }
}
