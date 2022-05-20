using Microsoft.VisualStudio.TestTools.UnitTesting;
using Planetario.Models;
using Planetario.Controllers;
using System.Web.Mvc;
using Moq;
using System.Collections.Generic;
using System.Web.Script.Serialization;
using Planetario.Interfaces;

namespace PruebasPlanetarios.Controllers
{
    [TestClass]
    public class VentasControllerTest
    {
        private Mock<CookiesInterfaz> CrearMockDeCookies()
        {
            var mockCookies = new Mock<CookiesInterfaz>();
            mockCookies.Setup(servicio => servicio.SesionIniciada()).Returns(true);
            mockCookies.Setup(servicio => servicio.CorreoUsuario()).Returns("ejemplo@gmail.com");
            return mockCookies;
        }
        private Mock<VentasInterfaz> CrearMockDeVentas()
        {
            string correoUsuario = "ejemplo@gmail.com";
            var mockVentas = new Mock<VentasInterfaz>();
            mockVentas.Setup(servicio => servicio.ObtenerTodosLosProductosDelCarrito(correoUsuario)).Returns(new List<ComprableModel>());
            mockVentas.Setup(servicio => servicio.ObtenerTodasLasEntradasDelCarrito(correoUsuario)).Returns(new List<ComprableModel>());
            mockVentas.Setup(servicio => servicio.ObtenerPrecioTotalDeProductosDelCarrito(correoUsuario)).Returns(5);
            mockVentas.Setup(servicio => servicio.ObtenerPrecioTotalDeEntradasDelCarrito(correoUsuario)).Returns(5);
            mockVentas.Setup(servicio => servicio.ObtenerCantidadDeProductosDelCarrito(correoUsuario)).Returns(1);
            mockVentas.Setup(servicio => servicio.ObtenerCantidadDeEntradasDelCarrito(correoUsuario)).Returns(1);
            return mockVentas;
        } 

        [TestMethod]
        public void ListaProductosNoEsNula()
        {
            VentasController ventasController = new VentasController();

            ViewResult vistaResultado = ventasController.ListaProductos() as ViewResult;

            Assert.IsNotNull(vistaResultado);
        }

        [TestMethod]
        public void ListaProductosFiltradosNoEsNula()
        {
            var mockProductos = new Mock<ProductosInterfaz>();
            mockProductos.Setup(servicio => servicio.ObtenerProductosFiltrados(0, 1000000, "", "", "")).Returns(new List<ProductoModel>());
            VentasController ventasController = new VentasController(mockProductos.Object);           

            JsonResult resultado = ventasController.ListaProductosFiltrados(0, 1000000, "", "", "");

            Assert.IsNotNull(resultado);
        }

        [TestMethod]
        public void VerCarritoDelUsuarioNoEsNulo()
        {
            Mock<VentasInterfaz> mockVentas = CrearMockDeVentas();
            Mock<CookiesInterfaz> mockCookies = CrearMockDeCookies();
            VentasController ventasController = new VentasController(mockVentas.Object,mockCookies.Object);

            ViewResult vistaResultado = ventasController.Carrito() as ViewResult;

            Assert.IsNotNull(vistaResultado);
        }

        [TestMethod]
        public void VerCarritoDelUsuarioRetornaTotalCorrecto()
        {
            Mock<VentasInterfaz> mockVentas = CrearMockDeVentas();
            Mock<CookiesInterfaz> mockCookies = CrearMockDeCookies();
            VentasController ventasController = new VentasController(mockVentas.Object,mockCookies.Object);
            double precioEsperado = 10;

            ViewResult vistaResultado = ventasController.Carrito() as ViewResult;
            double precioRecivido = vistaResultado.ViewBag.Precio;

            Assert.AreEqual(precioEsperado, precioRecivido);
        }

        [TestMethod]
        public void VerCarritoDelUsuarioRetornaListaProductosNoNula()
        {
            Mock<VentasInterfaz> mockVentas = CrearMockDeVentas();
            Mock<CookiesInterfaz> mockCookies = CrearMockDeCookies();
            VentasController ventasController = new VentasController(mockVentas.Object,mockCookies.Object);

            ViewResult vistaResultado = ventasController.Carrito() as ViewResult;
            var listaProductos = vistaResultado.ViewBag.ListaProductos;

            Assert.IsNotNull(listaProductos);
        }

        [TestMethod]
        public void VerCarritoDelUsuarioRetornaListaEntradasNoNula()
        {
            Mock<VentasInterfaz> mockVentas = CrearMockDeVentas();
            Mock<CookiesInterfaz> mockCookies = CrearMockDeCookies(); 
            VentasController ventasController = new VentasController(mockVentas.Object,mockCookies.Object);

            ViewResult vistaResultado = ventasController.Carrito() as ViewResult;
            var listaEntradas = vistaResultado.ViewBag.ListaEntradas;

            Assert.IsNotNull(listaEntradas);
        }

        [TestMethod]
        public void VerCarritoDelUsuarioRetornaTotalNoNulo()
        {
            Mock<VentasInterfaz> mockVentas = CrearMockDeVentas();
            Mock<CookiesInterfaz> mockCookies = CrearMockDeCookies();
            VentasController ventasController = new VentasController(mockVentas.Object,mockCookies.Object);

            ViewResult vistaResultado = ventasController.Carrito() as ViewResult;
            var total = vistaResultado.ViewBag.PrecioTotal;

            Assert.IsNotNull(total);
        }

        [TestMethod]
        public void VerCarritoListaDeProductosEsTipoLista()
        {
            Mock<VentasInterfaz> mockVentas = CrearMockDeVentas();
            Mock<CookiesInterfaz> mockCookies = CrearMockDeCookies();
            VentasController ventasController = new VentasController(mockVentas.Object,mockCookies.Object);

            ViewResult vistaResultado = ventasController.Carrito() as ViewResult;
            var listaProductos = vistaResultado.ViewBag.ListaProductos;

            Assert.IsInstanceOfType(listaProductos, typeof(List<ComprableModel>));
        }

        [TestMethod]
        public void VerCarritoListaDeEntradasEsTipoLista()
        {
            Mock<VentasInterfaz> mockVentas = CrearMockDeVentas();
            Mock<CookiesInterfaz> mockCookies = CrearMockDeCookies();
            VentasController ventasController = new VentasController(mockVentas.Object,mockCookies.Object);

            ViewResult vistaResultado = ventasController.Carrito() as ViewResult;
            var listaEntradas = vistaResultado.ViewBag.ListaEntradas;

            Assert.IsInstanceOfType(listaEntradas, typeof(List<ComprableModel>));
        }

        [TestMethod]
        public void VerCarritoPrecioTotalsEsTipoDouble()
        {
            Mock<VentasInterfaz> mockVentas = CrearMockDeVentas();
            Mock<CookiesInterfaz> mockCookies = CrearMockDeCookies();
            VentasController ventasController = new VentasController(mockVentas.Object,mockCookies.Object) { };

            ViewResult vistaResultado = ventasController.Carrito() as ViewResult;
            var total = vistaResultado.ViewBag.PrecioTotal;

            Assert.IsInstanceOfType(total, typeof(double));
        }

        [TestMethod]
        public void EliminarElementoDelCarritoDelUsuarioNoDevuelveNulo()
        {
            string correoUsuario = "danielmonge25@hotmail.com";
            var mockCookies = new Mock<CookiesInterfaz>();
            mockCookies.Setup(servicio => servicio.SesionIniciada()).Returns(true);
            mockCookies.Setup(servicio => servicio.CorreoUsuario()).Returns(correoUsuario);
            var mockVentas = new Mock<VentasInterfaz>();
            int idComprable = 5;
            mockVentas.Setup(servicio => servicio.EliminarDelCarrito(correoUsuario,idComprable)).Returns(false);
            VentasController ventasController = new VentasController(mockVentas.Object,mockCookies.Object);

            JsonResult resultado = ventasController.EliminarElementoDelCarritoDelUsuario(idComprable);

            Assert.IsNotNull(resultado);
        }

        [TestMethod]
        public void EliminarElementoDelCarritoDelUsuarioDevuelveFalsoCuandoFalla()
        {
            string correoUsuario = "danielmonge25@hotmail.com";
            var mockCookies = new Mock<CookiesInterfaz>();
            mockCookies.Setup(servicio => servicio.SesionIniciada()).Returns(true);
            mockCookies.Setup(servicio => servicio.CorreoUsuario()).Returns(correoUsuario);
            var mockVentas = new Mock<VentasInterfaz>();
            int idComprable = 5;
            mockVentas.Setup(servicio => servicio.EliminarDelCarrito(correoUsuario, idComprable)).Returns(false);
            VentasController ventasController = new VentasController(mockVentas.Object,mockCookies.Object);

            JsonResult resultado = ventasController.EliminarElementoDelCarritoDelUsuario(idComprable);
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string dataRecibida = serializer.Serialize(resultado.Data);


            Assert.AreEqual(dataRecibida, "{\"Exito\":false}");
        }


        [TestMethod]
        public void EliminarElementoDelCarritoDelUsuarioDevuelveVerdaderoCuandoCumple()
        {
            string correoUsuario = "danielmonge25@hotmail.com";
            var mockCookies = new Mock<CookiesInterfaz>();
            mockCookies.Setup(servicio => servicio.SesionIniciada()).Returns(true);
            mockCookies.Setup(servicio => servicio.CorreoUsuario()).Returns(correoUsuario);
            var mockVentas = new Mock<VentasInterfaz>();
            int idComprable = 5;
            mockVentas.Setup(servicio => servicio.EliminarDelCarrito(correoUsuario, idComprable)).Returns(true);
            VentasController ventasController = new VentasController(mockVentas.Object,mockCookies.Object);

            JsonResult resultado = ventasController.EliminarElementoDelCarritoDelUsuario(idComprable);
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string dataRecibida = serializer.Serialize(resultado.Data);


            Assert.AreEqual(dataRecibida, "{\"Exito\":true}");
        }

        [TestMethod]
        public void DisminiuirLaCantidadDelElementoDelCarritoDelUsuarioDevuelveFalsoCuandoFalla()
        {
            var mockVentas = new Mock<VentasInterfaz>();
            string correoUsuario = "diazfonseca.diego@gmail.com";
            int idComprable = 5;
            mockVentas.Setup(servicio => servicio.DisminiuirLaCantidadDelElementoDelCarrito(correoUsuario, idComprable)).Returns(false);
            VentasController ventasController = new VentasController(mockVentas.Object);

            JsonResult resultado = ventasController.DisminiuirLaCantidadDelElementoDelCarritoDelUsuario(correoUsuario, idComprable);
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string dataRecibida = serializer.Serialize(resultado.Data);


            Assert.AreEqual(dataRecibida, "{\"Exito\":false}");
        }

        [TestMethod]
        public void DisminiuirLaCantidadDelElementoDelCarritoDelUsuarioDevuelveVerdaderoCuandoCumple()
        {
            var mockVentas = new Mock<VentasInterfaz>();
            string correoUsuario = "diazfonseca.diego@gmail.com";
            int idComprable = 5;
            mockVentas.Setup(servicio => servicio.DisminiuirLaCantidadDelElementoDelCarrito(correoUsuario, idComprable)).Returns(true);
            VentasController ventasController = new VentasController(mockVentas.Object);

            JsonResult resultado = ventasController.DisminiuirLaCantidadDelElementoDelCarritoDelUsuario(correoUsuario, idComprable);
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string dataRecibida = serializer.Serialize(resultado.Data);

            Assert.AreEqual(dataRecibida, "{\"Exito\":true}");
        }

        [TestMethod]
        public void AumentarLaCantidadDelElementoDelCarritoDelUsuarioDevuelveFalsoCuandoFalla()
        {
            var mockVentas = new Mock<VentasInterfaz>();
            string correoUsuario = "diazfonseca.diego@gmail.com";
            int idComprable = 5;
            mockVentas.Setup(servicio => servicio.AumentarLaCantidadDelElementoDelCarrito(correoUsuario, idComprable)).Returns(false);
            VentasController ventasController = new VentasController(mockVentas.Object);

            JsonResult resultado = ventasController.AumentarLaCantidadDelElementoDelCarritoDelUsuario(correoUsuario, idComprable);
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string dataRecibida = serializer.Serialize(resultado.Data);


            Assert.AreEqual(dataRecibida, "{\"Exito\":false}");
        }

        [TestMethod]
        public void AumentarLaCantidadDelElementoDelCarritoDelUsuarioDevuelveVerdaderoCuandoCumple()
        {
            var mockVentas = new Mock<VentasInterfaz>();
            string correoUsuario = "diazfonseca.diego@gmail.com";
            int idComprable = 5;
            mockVentas.Setup(servicio => servicio.AumentarLaCantidadDelElementoDelCarrito(correoUsuario, idComprable)).Returns(true);
            VentasController ventasController = new VentasController(mockVentas.Object);

            JsonResult resultado = ventasController.AumentarLaCantidadDelElementoDelCarritoDelUsuario(correoUsuario, idComprable);
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string dataRecibida = serializer.Serialize(resultado.Data);

            Assert.AreEqual(dataRecibida, "{\"Exito\":true}");
        }

        [TestMethod]
        public void AgregarAlCarritoDevuelveFalsoCuandoFalla()
        {
            var mockVentas = new Mock<VentasInterfaz>();
            int cantidad = 5;
            int idComprable = 5;
            mockVentas.Setup(servicio => servicio.AgregarAlCarrito(idComprable, cantidad)).Returns(false);
            VentasController ventasController = new VentasController(mockVentas.Object);

            JsonResult resultado = ventasController.AgregarAlCarrito(idComprable, cantidad);
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string dataRecibida = serializer.Serialize(resultado.Data);


            Assert.AreEqual(dataRecibida, "{\"Exito\":false}");
        }

        [TestMethod]
        public void AgregarAlCarritoDevuelveVerdaderoCuandoCumple()
        {
            var mockVentas = new Mock<VentasInterfaz>();
            int cantidad = 5;
            int idComprable = 5;
            mockVentas.Setup(servicio => servicio.AgregarAlCarrito(idComprable, cantidad)).Returns(true);
            VentasController ventasController = new VentasController(mockVentas.Object);

            JsonResult resultado = ventasController.AgregarAlCarrito(idComprable, cantidad);
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string dataRecibida = serializer.Serialize(resultado.Data);

            Assert.AreEqual(dataRecibida, "{\"Exito\":true}");
        }

        [TestMethod]
        public void AgregarProductoNoDevuelveVistaNula()
        {
            var mockVentas = new Mock<VentasInterfaz>();
            VentasController ventasController = new VentasController(mockVentas.Object);

            ViewResult vistaResultado = ventasController.AgregarProducto() as ViewResult;

            Assert.IsNotNull(vistaResultado);
        }

        [TestMethod]
        public void AgregarProductoTieneMensajeCorrectoAlInsertar()
        {
            var mockProductos = new Mock<ProductosInterfaz>();
            ProductoModel producto = new ProductoModel {
                Id = 1,
                Nombre = "Diego",
                Precio = 123456,
                CantidadDisponible = 12,
                CantidadRebastecer = 12,
                Tamano = "Grande",
                Categoria = "Categoria",
                Descripcion = "Descripción",
                FechaIngreso = "12/12/12",
                FechaUltimaVenta = "12/12/12"
            };
            mockProductos.Setup(servicio => servicio.InsertarProducto(producto)).Returns(true);
            VentasController ventasController = new VentasController(mockProductos.Object);

            ViewResult vistaResultado = ventasController.AgregarProducto(producto) as ViewResult;
            string mensaje = vistaResultado.ViewBag.Mensaje;

            Assert.AreEqual(mensaje, "El producto " + producto.Nombre + " fue agregado con éxito");
        }

        [TestMethod]
        public void AgregarProductoTieneMensajeCorrectoCuandoFallaInsertar()
        {
            var mockProductos = new Mock<ProductosInterfaz>();
            ProductoModel producto = new ProductoModel
            {
                Id = 1,
                Nombre = "Diego",
                Precio = 123456,
                CantidadDisponible = 12,
                CantidadRebastecer = 12,
                Tamano = "Grande",
                Categoria = "Categoria",
                Descripcion = "Descripción",
                FechaIngreso = "12/12/12",
                FechaUltimaVenta = "12/12/12"
            };
            mockProductos.Setup(servicio => servicio.InsertarProducto(producto)).Returns(false);
            VentasController ventasController = new VentasController(mockProductos.Object);

            ViewResult vistaResultado = ventasController.AgregarProducto(producto) as ViewResult;
            string mensaje = vistaResultado.ViewBag.Mensaje;

            Assert.AreEqual(mensaje, "Hubo un error en el servidor");
        }

        [TestMethod]
        public void ObtenerPorcentajeDescuentoNoEsNulo()
        {
            string codigo = "123";
            int porcentaje = 40;
            Mock<DescuentosInterfaz> mockDescuentos = new Mock<DescuentosInterfaz>();
            mockDescuentos.Setup(servicio => servicio.ObtenerPorcentajeDescuento(codigo)).Returns(porcentaje);
            VentasController ventasController = new VentasController();

            JsonResult resultado = ventasController.ObtenerPorcentajeDescuento(codigo);

            Assert.IsNotNull(resultado);
        }

        [TestMethod]
        public void ObtenerPorcentajeDescuentoEsPorcentajeCorrecto()
        {
            string codigo = "123";
            int porcentaje = 40;
            Mock<DescuentosInterfaz> mockDescuentos = new Mock<DescuentosInterfaz>();
            mockDescuentos.Setup(servicio => servicio.ObtenerPorcentajeDescuento(codigo)).Returns(porcentaje);
            VentasController ventasController = new VentasController(mockDescuentos.Object);

            JsonResult resultado = ventasController.ObtenerPorcentajeDescuento(codigo);
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string dataRecibida = serializer.Serialize(resultado.Data);

            Assert.AreEqual(dataRecibida,porcentaje.ToString());
        }
    }
}
