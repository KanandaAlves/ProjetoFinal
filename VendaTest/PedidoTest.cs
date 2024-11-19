using Microsoft.VisualStudio.TestTools.UnitTesting;
using Venda.Controllers;
using Venda.Data;
using Venda.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;


namespace VendaTest.Tests
{
    [TestClass]
    public class PedidoControllerTests
    {
        private PedidoController _controller;
        private VendaContext _context;

        [TestInitialize]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<VendaContext>()
                .UseInMemoryDatabase(databaseName: "PecasTestDb")
                .Options;

            _context = new VendaContext(options);
            SeedDatabase();
            _controller = new PedidoController(_context);
        }

        [TestCleanup]
        public void Cleanup()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        private void SeedDatabase()
        {
            _context.Clientes.Add(new Cliente { ClienteId = 1, Nome = "Test Client", CPF = "12345678900", Telefone = "123456789", Email = "test@example.com" });
            _context.Pedidos.Add(new Pedido { PedidoId = 1, DataPedido = System.DateTime.Now, ClienteId = 1 });
            _context.SaveChanges();
        }

        [TestMethod]
        public async Task Index_ReturnsViewResult_WithListOfPedidos()
        {
            var result = await _controller.Index();
            var viewResult = result as ViewResult;
            Assert.IsNotNull(viewResult);
            Assert.IsInstanceOfType(viewResult.Model, typeof(System.Collections.Generic.List<Pedido>));
        }

        [TestMethod]
        public async Task Details_ReturnsViewResult_WhenIdIsValid()
        {
            var result = await _controller.Details(1);
            var viewResult = result as ViewResult;
            Assert.IsNotNull(viewResult);
            Assert.IsInstanceOfType(viewResult.Model, typeof(Pedido));
        }

        [TestMethod]
        public async Task Details_ReturnsNotFound_WhenIdIsNull()
        {
            var result = await _controller.Details(null);
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public async Task Details_ReturnsNotFound_WhenPedidoDoesNotExist()
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
