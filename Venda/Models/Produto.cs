namespace Venda.Models
{
    public class Produto
    {
        public int ProdutoId { get; set; }
        public string Nome { get; set; }
        public string Marca { get; set; }
        public decimal Preco { get; set; }
        public int Estoque { get; set; } 

        public ICollection<ItemPedido>? ItensPedidos{ get; set; }
        public ICollection<Movimentacao>? Movimentacoes { get; set; }
    }
}
