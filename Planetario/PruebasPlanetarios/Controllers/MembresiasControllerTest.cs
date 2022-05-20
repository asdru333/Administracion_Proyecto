using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Planetario.Controllers;
using Planetario.Interfaces;
using System.Web.Mvc;

namespace PruebasPlanetarios.Controllers
{
    [TestClass]
    public class MembresiasControllerTest
    {
        private Mock<CookiesInterfaz> CrearMockDeCookies()
        {
            var mockCookies = new Mock<CookiesInterfaz>();
            mockCookies.Setup(servicio => servicio.SesionIniciada()).Returns(true);
            mockCookies.Setup(servicio => servicio.CorreoUsuario()).Returns("bryan.umaa@hotmail.com");
            return mockCookies;
        }

        private Mock<MembresiasInterfaz> CrearMockDeMembresia()
        {
            string correoUsuario = "bryan.umaa@hotmail.com";
            var mockMembresias = new Mock<MembresiasInterfaz>();
            mockMembresias.Setup(servicio => servicio.ObtenerMembresia(correoUsuario)).Returns("Solar");
            return mockMembresias;
        }

        [TestMethod]
        public void ComprarNoDevuelveVistaNula()
        {
            Mock<CookiesInterfaz> mockCookies = CrearMockDeCookies();
            Mock<MembresiasInterfaz> mockMembresias = CrearMockDeMembresia();     
            MembresiasController membresiasController = new MembresiasController(mockMembresias.Object, mockCookies.Object);
            ViewResult vistaResultado = membresiasController.Comprar() as ViewResult;
            Assert.IsNotNull(vistaResultado);
        }

        [TestMethod]
        public void PagoNoDevuelveVistaNula()
        {
            Mock<CookiesInterfaz> mockCookies = CrearMockDeCookies();
            var mockMembresias = new Mock<MembresiasInterfaz>();
            MembresiasController membresiasController = new MembresiasController(mockMembresias.Object, mockCookies.Object);
            ViewResult vistaResultado = membresiasController.Pago("Lunar") as ViewResult;
            Assert.IsNotNull(vistaResultado);
        }

        [TestMethod]
        public void SatisfaccionNoDevuelveVistaNula()
        {
            Mock<CookiesInterfaz> mockCookies = CrearMockDeCookies();
            var mockMembresias = new Mock<MembresiasInterfaz>();
            MembresiasController membresiasController = new MembresiasController(mockMembresias.Object, mockCookies.Object);
            ViewResult vistaResultado = membresiasController.Satisfactorio("Lunar") as ViewResult;
            Assert.IsNotNull(vistaResultado);
        }

        [TestMethod]
        public void PrecioCorrectoComprarLunar()
        {
            Mock<CookiesInterfaz> mockCookies = CrearMockDeCookies();
            var mockMembresias = new Mock<MembresiasInterfaz>();
            MembresiasController membresiasController = new MembresiasController(mockMembresias.Object, mockCookies.Object);
            ViewResult vistaResultado = membresiasController.Pago("Lunar") as ViewResult;
            Assert.AreEqual(vistaResultado.ViewBag.Precio, 5000);
        }

        [TestMethod]
        public void PrecioCorrectoComprarSolar()
        {
            Mock<CookiesInterfaz> mockCookies = CrearMockDeCookies();
            var mockMembresias = new Mock<MembresiasInterfaz>();
            MembresiasController membresiasController = new MembresiasController(mockMembresias.Object, mockCookies.Object);
            ViewResult vistaResultado = membresiasController.Pago("Solar") as ViewResult;
            Assert.AreEqual(vistaResultado.ViewBag.Precio , 10000);
        }
    }
}
