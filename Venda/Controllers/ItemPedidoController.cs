using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Venda.Data;
using Venda.Models;

namespace Venda.Controllers
{

    public class ItemPedidoController : Controller
    {
        private readonly VendaContext _context;

        public ItemPedidoController(VendaContext context)
        {
            _context = context;
        }


        // GET: ItemPedido
        public async Task<IActionResult> Index()
        {
            var vendaContext = _context.ItemPedidos.Include(i => i.Pedido).Include(i => i.Produto);
            return View(await vendaContext.ToListAsync());
        }

        // GET: ItemPedido/Details/5
        public async Task<IActionResult> Details(int? pedidoId, int? produtoId)
        {
            if (pedidoId == null || produtoId == null)

            {
                return NotFound();
            }

            var itemPedido = await _context.ItemPedidos
                .Include(i => i.Pedido)
                .Include(i => i.Produto)
                .FirstOrDefaultAsync(m => m.PedidoId == pedidoId && m.ProdutoId == produtoId);

            if (itemPedido == null)
            {
                return NotFound();
            }

            return View(itemPedido);
        }

        // GET: ItemPedido/Create
        public IActionResult Create()
        {

            ViewData["PedidoId"] = new SelectList(_context.Pedidos.Include(l => l.Cliente), "PedidoId", "Cliente.Nome");
            ViewData["ProdutoId"] = new SelectList(_context.Produtos, "ProdutoId", "Nome");
            return View();
        }

        // POST: ItemPedido/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PedidoId,ProdutoId,Quantidade,PrecoUnitario,TotalPedidos")] ItemPedido itemPedido)
        {
            if (ModelState.IsValid)
            {

                var existingItemPedido = await _context.ItemPedidos
                    .FirstOrDefaultAsync(ip => ip.PedidoId == itemPedido.PedidoId && ip.ProdutoId == itemPedido.ProdutoId);

                if (existingItemPedido != null)
                {

                    ModelState.AddModelError("", "Este produto já está cadastrado neste pedido.");
                    ViewData["PedidoId"] = new SelectList(_context.Pedidos.Include(l => l.Cliente), "PedidoId", "Cliente.Nome", itemPedido.PedidoId);
                    ViewData["ProdutoId"] = new SelectList(_context.Produtos, "ProdutoId", "Nome", itemPedido.ProdutoId);
                    return View(itemPedido);
                }
                var produto = await _context.Produtos.FindAsync(itemPedido.ProdutoId);
                if (produto == null)
                {
                    ModelState.AddModelError("", "Produto não encontrado.");
                }
                else if (itemPedido.Quantidade > produto.Estoque)
                {
                    ModelState.AddModelError("", "A quantidade do produto não pode ser maior que o estoque disponível.");
                }
                else
                {
                    itemPedido.TotalPedidos = itemPedido.Quantidade * itemPedido.PrecoUnitario;
                    _context.ItemPedidos.Add(itemPedido);

                    produto.Estoque -= itemPedido.Quantidade;
                    _context.Update(produto);

                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                ViewData["PedidoId"] = new SelectList(_context.Pedidos.Include(l => l.Cliente), "PedidoId", "Cliente.Nome", itemPedido.PedidoId);
                ViewData["ProdutoId"] = new SelectList(_context.Produtos, "ProdutoId", "Nome", itemPedido.ProdutoId);
                return View(itemPedido);
            }

            ViewData["PedidoId"] = new SelectList(_context.Pedidos.Include(l => l.Cliente), "PedidoId", "Cliente.Nome", itemPedido.PedidoId);
            ViewData["ProdutoId"] = new SelectList(_context.Produtos, "ProdutoId", "Nome", itemPedido.ProdutoId);
            return View(itemPedido);
        }


        // GET: ItemPedido/Edit/5
        public async Task<IActionResult> Edit(int? pedidoId, int? produtoId)
        {
            if (pedidoId == null || produtoId == null)
            {
                return NotFound();
            }

            var itemPedido = await _context.ItemPedidos.FindAsync(pedidoId, produtoId);
            if (itemPedido == null)
            {
                return NotFound();
            }
            ViewData["PedidoId"] = new SelectList(_context.Pedidos, "PedidoId", "PedidoId", itemPedido.PedidoId);
            ViewData["ProdutoId"] = new SelectList(_context.Produtos, "ProdutoId", "ProdutoId", itemPedido.ProdutoId);
            return View(itemPedido);
        }

        // POST: ItemPedido/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int pedidoId, int produtoId, [Bind("PedidoId,ProdutoId,Quantidade,PrecoUnitario,TotalPedidos")] ItemPedido itemPedido)
        {
            if (pedidoId != itemPedido.PedidoId || produtoId != itemPedido.ProdutoId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(itemPedido);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ItemPedidoExists(itemPedido.PedidoId, itemPedido.ProdutoId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["PedidoId"] = new SelectList(_context.Pedidos, "PedidoId", "PedidoId", itemPedido.PedidoId);
            ViewData["ProdutoId"] = new SelectList(_context.Produtos, "ProdutoId", "ProdutoId", itemPedido.ProdutoId);
            return View(itemPedido);
        }

        private bool ItemPedidoExists(int pedidoId, int produtoId)
        {
            return _context.ItemPedidos.Any(e => e.PedidoId == pedidoId && e.ProdutoId == produtoId);
        }

        // GET: ItemPedido/Delete/5
        public async Task<IActionResult> Delete(int? pedidoId, int? produtoId)
        {
            if (pedidoId == null || produtoId == null)
            {
                return NotFound();
            }

            var itemPedido = await _context.ItemPedidos
                .Include(i => i.Pedido)
                .Include(i => i.Produto)
                .FirstOrDefaultAsync(m => m.PedidoId == pedidoId && m.ProdutoId == produtoId);

            if (itemPedido == null)
            {
                return NotFound();
            }

            return View(itemPedido);
        }

        // POST: ItemPedido/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int pedidoId, int produtoId)
        {
            var itemPedido = await _context.ItemPedidos
                .FirstOrDefaultAsync(m => m.PedidoId == pedidoId && m.ProdutoId == produtoId);

            if (itemPedido != null)
            {
                _context.ItemPedidos.Remove(itemPedido);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
       
    }
}
