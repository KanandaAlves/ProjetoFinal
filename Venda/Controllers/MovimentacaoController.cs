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
    public class MovimentacaoController : Controller
    {
        private readonly VendaContext _context;

        public MovimentacaoController(VendaContext context)
        {
            _context = context;
        }

        // GET: Movimentacao
        public async Task<IActionResult> Index()
        {
            var vendaContext = _context.Movimentacoes.Include(m => m.Produto);
            return View(await vendaContext.ToListAsync());
        }

        // GET: Movimentacao/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || id <= 0)
            {
                return NotFound();
            }

            var movimentacao = await _context.Movimentacoes
                .Include(m => m.Produto)
                .FirstOrDefaultAsync(m => m.MovimentacaoId == id);
            if (movimentacao == null)
            {
                return NotFound();
            }

            return View(movimentacao);
        }

        // GET: Movimentacao/Create
        public IActionResult Create()
        {
            ViewBag.TiposMovimentacao = new SelectList(Enum.GetValues(typeof(Movimentacao.TipoMovimentacao)));
            ViewData["ProdutoId"] = new SelectList(_context.Produtos, "ProdutoId", "Nome");

            return View();
        }

        // POST: Movimentacao/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MovimentacaoId,ProdutoId,Quantidade,DataMovimentacao,Tipo")] Movimentacao movimentacao)
        {
            if (ModelState.IsValid)
            {
                
                var produto = _context.Produtos.FirstOrDefault(p => p.ProdutoId == movimentacao.ProdutoId);

                if (produto == null)
                {
                    
                    ModelState.AddModelError(string.Empty, "Produto não encontrado.");
                    return View(movimentacao);
                }

                
                if (movimentacao.Tipo == Movimentacao.TipoMovimentacao.Entrada)
                {
                    produto.Estoque += movimentacao.Quantidade; 
                }
                else if (movimentacao.Tipo == Movimentacao.TipoMovimentacao.Saida)
                {
                    if (produto.Estoque >= movimentacao.Quantidade)
                    {
                        produto.Estoque -= movimentacao.Quantidade; 
                    }
                    else
                    {
                        
                        ModelState.AddModelError(string.Empty, "Estoque insuficiente.");
                        return View(movimentacao);
                    }
                }

               
                _context.Movimentacoes.Add(movimentacao);
                await _context.SaveChangesAsync(); 
                return RedirectToAction(nameof(Index));
            }

           
            return View(movimentacao);
        }

        // GET: Movimentacao/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movimentacao = await _context.Movimentacoes.FindAsync(id);
            if (movimentacao == null)
            {
                return NotFound();
            }
            ViewData["ProdutoId"] = new SelectList(_context.Produtos, "ProdutoId", "ProdutoId", movimentacao.ProdutoId);
            return View(movimentacao);
        }

        // POST: Movimentacao/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MovimentacaoId,ProdutoId,Quantidade,DataMovimentacao,Tipo")] Movimentacao movimentacao)
        {
            if (id != movimentacao.MovimentacaoId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(movimentacao);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MovimentacaoExists(movimentacao.MovimentacaoId))
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
            ViewData["ProdutoId"] = new SelectList(_context.Produtos, "ProdutoId", "ProdutoId", movimentacao.ProdutoId);
            return View(movimentacao);
        }

        // GET: Movimentacao/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movimentacao = await _context.Movimentacoes
                .Include(m => m.Produto)
                .FirstOrDefaultAsync(m => m.MovimentacaoId == id);
            if (movimentacao == null)
            {
                return NotFound();
            }

            return View(movimentacao);
        }

        // POST: Movimentacao/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var movimentacao = await _context.Movimentacoes.FindAsync(id);
            if (movimentacao != null)
            {
                _context.Movimentacoes.Remove(movimentacao);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MovimentacaoExists(int id)
        {
            return _context.Movimentacoes.Any(e => e.MovimentacaoId == id);
        }
    }
}
