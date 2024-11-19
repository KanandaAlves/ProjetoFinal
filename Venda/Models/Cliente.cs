using Microsoft.EntityFrameworkCore;

namespace Venda.Models
{

    [Index(nameof(CPF), IsUnique = true)]
    public class Cliente
    {
        public int ClienteId { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string CPF { get; set; }
        public string Telefone { get; set; }
        public ICollection<Pedido>? Pedidos { get; set; }
    }
}
