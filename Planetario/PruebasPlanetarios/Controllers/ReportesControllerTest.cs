using Microsoft.VisualStudio.TestTools.UnitTesting;
using Planetario.Controllers;
using Planetario.Interfaces;
using System.Web.Mvc;
using Moq;
using System.Collections.Generic;

namespace PruebasPlanetarios.Controllers
{
    [TestClass]
    public class ReportesControllerTest
    {
        private Mock<DatosInterfaz> CrearMockDeDatos()
        {
            var mockDatos = new Mock<DatosInterfaz>();

            mockDatos.Setup(servicio => servicio.SelectListGeneros()).Returns(new List<SelectListItem>());
            mockDatos.Setup(servicio => servicio.SelectListPublicos()).Returns(new List<SelectListItem>());

            return mockDatos;
        }

        private Mock<ReportesInterfaz> CrearMockDeReportes()
        {
            var mockReportes = new Mock<ReportesInterfaz>();

            mockReportes.Setup(servicio => servicio.ObtenerTodasLasCategorias()).Returns(new List<string>());

            string fechaInicio = "2021/01/01";
            string fechaFinal = "2021/12/31";
            string orden = "ASC";
            mockReportes.Setup(servicio => servicio.ObtenerTodosLosProductosFiltradosPorRanking(fechaInicio, fechaFinal, orden)).Returns(new List<object>());

            return mockReportes;
        }

        [TestMethod]
        public void ReportesSinFiltrosNoEsNulo()
        {
            var mockReportes = CrearMockDeReportes();
            var mockDatos = CrearMockDeDatos();

            ReportesController reportesController = new ReportesController(mockReportes.Object, mockDatos.Object);

            ViewResult vistaResultado = reportesController.Reporte() as ViewResult;

            Assert.IsNotNull(vistaResultado);
        }

        [TestMethod]
        public void ReportesSinFiltrosListaDeCategoriasNoEsNulo()
        {
            var mockReportes = CrearMockDeReportes();
            var mockDatos = CrearMockDeDatos();

            ReportesController reportesController = new ReportesController(mockReportes.Object, mockDatos.Object);

            ViewResult vistaResultado = reportesController.Reporte() as ViewResult;
            var listaDeCategorias = vistaResultado.ViewBag.listaDeCategorias;

            Assert.IsNotNull(listaDeCategorias);
        }

        [TestMethod]
        public void ReportesSinFiltrosListaDeCategoriasEsTipoLista()
        {
            var mockReportes = CrearMockDeReportes();
            var mockDatos = CrearMockDeDatos();

            ReportesController reportesController = new ReportesController(mockReportes.Object, mockDatos.Object);

            ViewResult vistaResultado = reportesController.Reporte() as ViewResult;
            var listaDeCategorias = vistaResultado.ViewBag.listaDeCategorias;

            Assert.IsInstanceOfType(listaDeCategorias, typeof(List<string>));
        }

        [TestMethod]
        public void ReportesSinFiltrosListaDeCategoriasNoTieneNulos()
        {
            var mockReportes = CrearMockDeReportes();
            var mockDatos = CrearMockDeDatos();

            ReportesController reportesController = new ReportesController(mockReportes.Object, mockDatos.Object);

            ViewResult vistaResultado = reportesController.Reporte() as ViewResult;
            var listaDeCategorias = vistaResultado.ViewBag.listaDeCategorias;

            CollectionAssert.AllItemsAreNotNull(listaDeCategorias);
        }

        [TestMethod]
        public void ReportesFiltradosPorRankingNoEsNulo()
        {
            var mockReportes = CrearMockDeReportes();
            var mockDatos = CrearMockDeDatos();
            string fechaInicio = "2021/01/01";
            string fechaFinal = "2021/12/31";
            string orden = "ASC";

            ReportesController reportesController = new ReportesController(mockReportes.Object, mockDatos.Object);

            ViewResult vistaResultado = reportesController.Reporte(fechaInicio, fechaFinal, orden) as ViewResult;

            Assert.IsNotNull(vistaResultado);
        }

        [TestMethod]
        public void ReportesFiltradosPorRankingListaDeCategoriasNoEsNulo()
        {
            var mockReportes = CrearMockDeReportes();
            var mockDatos = CrearMockDeDatos();
            string fechaInicio = "2021/01/01";
            string fechaFinal = "2021/12/31";
            string orden = "ASC";

            ReportesController reportesController = new ReportesController(mockReportes.Object, mockDatos.Object);

            ViewResult vistaResultado = reportesController.Reporte(fechaInicio, fechaFinal, orden) as ViewResult;
            var listaDeCategorias = vistaResultado.ViewBag.listaDeCategorias;

            Assert.IsNotNull(listaDeCategorias);
        }

  
        [TestMethod]
        public void ReportesFiltradosPorRankingListaDeCategoriasEsTipoLista()
        {
            var mockReportes = CrearMockDeReportes();
            var mockDatos = CrearMockDeDatos();
            string fechaInicio = "2021/01/01";
            string fechaFinal = "2021/12/31";
            string orden = "ASC";

            ReportesController reportesController = new ReportesController(mockReportes.Object, mockDatos.Object);

            ViewResult vistaResultado = reportesController.Reporte(fechaInicio, fechaFinal, orden) as ViewResult;
            var listaDeCategorias = vistaResultado.ViewBag.listaDeCategorias;

            Assert.IsInstanceOfType(listaDeCategorias, typeof(List<string>));
        }

        [TestMethod]
        public void ReportesFiltradosPorRankingListaDeCategoriasNoTieneNulos()
        {
            var mockReportes = CrearMockDeReportes();
            var mockDatos = CrearMockDeDatos();
            string fechaInicio = "2021/01/01";
            string fechaFinal = "2021/12/31";
            string orden = "ASC";

            ReportesController reportesController = new ReportesController(mockReportes.Object, mockDatos.Object);

            ViewResult vistaResultado = reportesController.Reporte(fechaInicio, fechaFinal, orden) as ViewResult;
            var listaDeCategorias = vistaResultado.ViewBag.listaDeCategorias;

            CollectionAssert.AllItemsAreNotNull(listaDeCategorias);
        }     

        [TestMethod]
        public void ReportesFiltradosPorCategoriaNoEsNulo()
        {
            string categoria = "Telescopio";
            var mockReportes = CrearMockDeReportes();
            var mockDatos = CrearMockDeDatos();
            string fechaInicio = "2021/01/01";
            string fechaFinal = "2021/12/31";

            ReportesController reportesController = new ReportesController(mockReportes.Object, mockDatos.Object);

            ViewResult vistaResultado = reportesController.Reporte(categoria, fechaInicio, fechaFinal) as ViewResult;

            Assert.IsNotNull(vistaResultado);
        }

        [TestMethod]
        public void ReportesFiltradosPorCategoriaListaDeCategoriasNoEsNulo()
        {
            string categoria = "Telescopio";
            var mockReportes = CrearMockDeReportes();
            var mockDatos = CrearMockDeDatos();
            string fechaInicio = "2021/01/01";
            string fechaFinal = "2021/12/31";

            ReportesController reportesController = new ReportesController(mockReportes.Object, mockDatos.Object);

            ViewResult vistaResultado = reportesController.Reporte(categoria, fechaInicio, fechaFinal) as ViewResult;
            var listaDeCategorias = vistaResultado.ViewBag.listaDeCategorias;

            Assert.IsNotNull(listaDeCategorias);
        }

        public void ReportesFiltradosPorCategoriaListaDeCategoriasEstipoLista()
        {
            string categoria = "Telescopio";
            var mockReportes = CrearMockDeReportes();
            var mockDatos = CrearMockDeDatos();
            string fechaInicio = "2021/01/01";
            string fechaFinal = "2021/12/31";

            ReportesController reportesController = new ReportesController(mockReportes.Object, mockDatos.Object);

            ViewResult vistaResultado = reportesController.Reporte(categoria, fechaInicio, fechaFinal) as ViewResult;
            var listaDeCategorias = vistaResultado.ViewBag.listaDeCategorias;

            Assert.IsInstanceOfType(listaDeCategorias, typeof(List<string>));
        }

        public void ReportesFiltradosPorCategoriaListaDeCategoriasNotieneNulos()
        {
            string categoria = "Telescopio";
            var mockReportes = CrearMockDeReportes();
            var mockDatos = CrearMockDeDatos();
            string fechaInicio = "2021/01/01";
            string fechaFinal = "2021/12/31";

            ReportesController reportesController = new ReportesController(mockReportes.Object, mockDatos.Object);

            ViewResult vistaResultado = reportesController.Reporte(categoria, fechaInicio, fechaFinal) as ViewResult;
            var listaDeCategorias = vistaResultado.ViewBag.listaDeCategorias;

            CollectionAssert.AllItemsAreNotNull(listaDeCategorias);
        }

        public void ReportesFiltradosPorCategoriaListadoFiltroPorCategoriaNoEsNulo()
        {
            string categoria = "Telescopio";
            var mockReportes = CrearMockDeReportes();
            var mockDatos = CrearMockDeDatos();
            string fechaInicio = "2021/01/01";
            string fechaFinal = "2021/12/31";

            ReportesController reportesController = new ReportesController(mockReportes.Object, mockDatos.Object);

            ViewResult vistaResultado = reportesController.Reporte(categoria, fechaInicio, fechaFinal) as ViewResult;
            var listadoFiltroPorCategoria = vistaResultado.ViewBag.listadoFiltroPorCategoria;

            Assert.IsNotNull(listadoFiltroPorCategoria);
        }

        public void ReportesFiltradosPorCategoriaListadoFiltroPorCategoriaEstipoLista()
        {
            string categoria = "Telescopio";
            var mockReportes = CrearMockDeReportes();
            var mockDatos = CrearMockDeDatos();
            string fechaInicio = "2021/01/01";
            string fechaFinal = "2021/12/31";

            ReportesController reportesController = new ReportesController(mockReportes.Object, mockDatos.Object);

            ViewResult vistaResultado = reportesController.Reporte(categoria, fechaInicio, fechaFinal) as ViewResult;
            var listadoFiltroPorCategoria = vistaResultado.ViewBag.listadoFiltroPorCategoria;

            Assert.IsInstanceOfType(listadoFiltroPorCategoria, typeof(List<string>));
        }

        public void ReportesFiltradosPorCategoriaListadoFiltroPorCategoriaNotieneNulos()
        {
            string categoria = "Telescopio";
            var mockReportes = CrearMockDeReportes();
            var mockDatos = CrearMockDeDatos();
            string fechaInicio = "2021/01/01";
            string fechaFinal = "2021/12/31";
            string orden = "ASC";

            ReportesController reportesController = new ReportesController(mockReportes.Object, mockDatos.Object);

            ViewResult vistaResultado = reportesController.Reporte(categoria, fechaInicio, fechaFinal) as ViewResult;
            var listadoFiltroPorCategoria = vistaResultado.ViewBag.listadoFiltroPorCategoria;

            CollectionAssert.AllItemsAreNotNull(listadoFiltroPorCategoria);
        }

        [TestMethod]
        public void ReporteMercadeoNoEsNulo()
        {
            var mockReportes = new Mock<ReportesInterfaz>();
            var mockReportesA = new Mock<DatosInterfaz>();
            mockReportes.Setup(servicio => servicio.ObtenerTodasLasCategorias()).Returns(new List<string>());
            mockReportesA.Setup(servicio => servicio.SelectListGeneros()).Returns(new List<SelectListItem>());
            mockReportesA.Setup(servicio => servicio.SelectListPublicos()).Returns(new List<SelectListItem>());

            ReportesController reportesController = new ReportesController(mockReportes.Object, mockReportesA.Object);

            ViewResult vistaResultado = reportesController.ReporteMercadeo() as ViewResult;

            Assert.IsNotNull(vistaResultado);
        }

        [TestMethod]
        public void ObtenerParesProductosNoEsNulo()
        {
            string publico = "Adultos";
            string membresia = "Lunar";
            var mockReportes = new Mock<ReportesInterfaz>();
            var mockReportesA = new Mock<DatosInterfaz>();
            mockReportes.Setup(servicio => servicio.ConsultaProductosCompradosJuntos(publico, membresia)).Returns(new List<object>());
            ReportesController reportesController = new ReportesController(mockReportes.Object, mockReportesA.Object);

            JsonResult resultado = reportesController.ObtenerParesProductos(publico, membresia);

            Assert.IsNotNull(resultado);
        }

        [TestMethod]
        public void ObtenerDatosPorGeneroYEdadNoEsNulo()
        {
            string categoria = "Ropa";
            string genero = "Femenino";
            string publico = "Adulto";
            var mockReportes = new Mock<ReportesInterfaz>();
            var mockReportesA = new Mock<DatosInterfaz>();
            mockReportes.Setup(servicio => servicio.ConsultaPorCategoriaProductoGeneroEdad(categoria, genero, publico)).Returns(new List<object>());
            ReportesController reportesController = new ReportesController(mockReportes.Object, mockReportesA.Object);

            JsonResult resultado = reportesController.ObtenerDatosPorGeneroYEdad(categoria, publico, genero);

            Assert.IsNotNull(resultado);
        }

        [TestMethod]
        public void ObtenerDatosExtranjerosNoEsNulo()
        {
            string categoria = "Ropa";
            var mockReportes = new Mock<ReportesInterfaz>();
            var mockReportesA = new Mock<DatosInterfaz>();
            mockReportes.Setup(servicio => servicio.ConsultaPorCategoriasPersonaExtranjeras(categoria)).Returns(new List<object>());
            ReportesController reportesController = new ReportesController(mockReportes.Object, mockReportesA.Object);

            JsonResult resultado = reportesController.ObtenerDatosExtranjeros(categoria);

            Assert.IsNotNull(resultado);
        }

        [TestMethod]
        public void ObtenerFiltroPorRankingNoEsNulo()
        {
            string fechaInicial = "1998-01-01";
            string fechaFinal = "2021-01-01";
            string orden = "DESC";
            var mockReportes = new Mock<ReportesInterfaz>();
            var mockReportesA = new Mock<DatosInterfaz>();
            mockReportes.Setup(servicio => servicio.ObtenerTodosLosProductosFiltradosPorRanking(fechaInicial, fechaFinal, orden)).Returns(new List<object>());
            ReportesController reportesController = new ReportesController(mockReportes.Object, mockReportesA.Object);

            JsonResult resultado = reportesController.ObtenerFiltroPorRanking(orden, fechaInicial, fechaFinal);

            Assert.IsNotNull(resultado);
        }
    }
}
