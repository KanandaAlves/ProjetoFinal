using Microsoft.EntityFrameworkCore;
using Venda.Models;

namespace Venda.Data
{
    public class VendaContext : DbContext
    {
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Produto> Produtos { get; set; }
        public DbSet<Pedido> Pedidos { get; set; }
        public DbSet<ItemPedido> ItemPedidos { get; set; }
        public DbSet<Movimentacao> Movimentacoes { get; set; }

        public VendaContext(DbContextOptions<VendaContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ItemPedido>()
                .HasKey(pp => new { pp.PedidoId, pp.ProdutoId });

            modelBuilder.Entity<ItemPedido>()
                .HasOne(pp => pp.Pedido)
                .WithMany(p => p.ItensPedidos)
                .HasForeignKey(pp => pp.PedidoId);

            modelBuilder.Entity<ItemPedido>()
                .HasOne(pp => pp.Produto)
                .WithMany(p => p.ItensPedidos)
                .HasForeignKey(pp => pp.ProdutoId);

            modelBuilder.Entity<Movimentacao>()
                .HasOne(m => m.Produto)
                .WithMany(p => p.Movimentacoes)
                .HasForeignKey(m => m.ProdutoId);

            modelBuilder.Entity<Cliente>()
                .HasIndex(c => c.CPF)
                .IsUnique();

        }
    }
}
