using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Venda.Controllers;
using Venda.Data;
using Venda.Models;

namespace VendaTest
{


    [TestClass]
    public class MovimentacaoControllerTests
    {
        private MovimentacaoController _controller;
        private VendaContext _context;

        [TestInitialize]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<VendaContext>()
                .UseInMemoryDatabase(databaseName: "MovimentacaoTestDb")
                .Options;

            _context = new VendaContext(options);
            SeedDatabase();
            _controller = new MovimentacaoController(_context);
        }

        [TestCleanup]
        public void Cleanup()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        private void SeedDatabase()
        {
            _context.Movimentacoes.Add(new Movimentacao { MovimentacaoId = 1, ProdutoId = 1, Quantidade = 10, DataMovimentacao = System.DateTime.Now, Tipo = 0 });
            _context.SaveChanges();
        }

        [TestMethod]
        public async Task Index_ReturnsViewResult_WithListOfMovimentacoes()
        {
            var result = await _controller.Index();
            var viewResult = result as ViewResult;
            Assert.IsNotNull(viewResult);
            Assert.IsInstanceOfType(viewResult.Model, typeof(List<Movimentacao>));
        }

        [TestMethod]
        public async Task Details_ReturnsViewResult_WhenIdIsValid()
        {
            var result = await _controller.Details(1);
            var viewResult = result as ViewResult;
            Assert.IsNotNull(viewResult);
            Assert.IsInstanceOfType(viewResult.Model, typeof(Movimentacao));
        }

        [TestMethod]
        public async Task Details_ReturnsNotFound_WhenIdIsNull()
        {
            var result = await _controller.Details(null);
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public async Task Details_ReturnsNotFound_WhenMovimentacaoDoesNotExist()
        {
            var result = await _controller.Details(999);
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public void Create_ReturnsViewResult()
        {
            var result = _controller.Create();
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }
    }
}