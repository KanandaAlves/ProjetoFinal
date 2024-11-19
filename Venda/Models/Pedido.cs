using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Venda.Models
{
    public class Pedido
    {
        public int PedidoId { get; set; }

        [CustomDateValidation(ErrorMessage = "A data do pedido não pode ser depois da data atual.")]
        public DateTime DataPedido { get; set; }
        public decimal Total { get; set; }

        [DisplayName("Nome do Cliente")]
        public int ClienteId { get; set; }
        public Cliente? Cliente { get; set; }
        public ICollection<ItemPedido>? ItensPedidos { get; set; }
    }

    public class CustomDateValidationAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value is DateTime date)
            {
                return date <= DateTime.Today;
            }
            return false;
        }
    }
}
