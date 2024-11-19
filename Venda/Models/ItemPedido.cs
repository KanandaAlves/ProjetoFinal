using System.ComponentModel;

namespace Venda.Models
{
    public class ItemPedido
    {
        [DisplayName("Cliente do Pedido")]
        public int PedidoId { get; set; }
        public Pedido? Pedido { get; set; }
        [DisplayName("Produto Escolhido")]
        public int ProdutoId { get; set; }
        public Produto? Produto { get; set; }

        public int Quantidade { get; set; }
        [DisplayName("Preço Unitário")]
        public decimal PrecoUnitario { get; set; }
        public decimal TotalPedidos { get; set; }
    }
}
