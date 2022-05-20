using Microsoft.VisualStudio.TestTools.UnitTesting;
using Planetario.Controllers;
using Planetario.Models;
using Moq;
using System.Web.Mvc;
using System.Collections.Generic;
using Planetario.Interfaces;

namespace PruebasPlanetarios.Controllers
{
    [TestClass]
    public class ActividadesControllerTest
    {
        [TestMethod]
        public void CrearActividadNoDevuelveVistaNula()
        {
            var mockActividades = new Mock<ActividadesInterfaz>();
            
            ActividadesController actividadesController = new ActividadesController(mockActividades.Object);

            ViewResult vistaResultado = actividadesController.CrearActividad() as ViewResult;

            Assert.IsNotNull(vistaResultado);
        }

        [TestMethod]
        public void CrearActividadNoDevuelveVistaNulaCuandoRecibeUnModelo()
        {
            var mockActividades = new Mock<ActividadesInterfaz>();
            var actividad = new ActividadModel();
            string topicos = "Topico,Topico";

            ActividadesController actividadesController = new ActividadesController(mockActividades.Object);
            mockActividades.Setup( mock => mock.InsertarActividad(actividad)).Returns(true);
            mockActividades.Setup( mock => mock.InsertarTopico("Nombre", topicos)).Returns(true);


            ViewResult vistaResultado = actividadesController.CrearActividad(actividad, topicos) as ViewResult;

            Assert.IsNotNull(vistaResultado);
        }

        [TestMethod]
        public void CrearActividadDevuelveMensajeCorrectoCuandoInsercionCorrecto()
        {
            var mockActividades = new Mock<ActividadesInterfaz>();
            var actividad = new ActividadModel() { NombreActividad = "Nombre" };
            string topicos = "Topico,Topico";

            ActividadesController actividadesController = new ActividadesController(mockActividades.Object);
            mockActividades.Setup(mock => mock.InsertarActividad(actividad)).Returns(true);
            mockActividades.Setup(mock => mock.InsertarTopico(actividad.NombreActividad, topicos)).Returns(true);

            ViewResult vistaResultado = actividadesController.CrearActividad(actividad, topicos) as ViewResult;

            Assert.AreEqual("La actividad " + actividad.NombreActividad + " fue creada con éxito.", vistaResultado.ViewBag.Message);
        }

        [TestMethod]
        public void CrearActividadDevuelveMensajeCorrectoCuandoInsercionIncorrecto()
        {
            var mockActividades = new Mock<ActividadesInterfaz>();
            var actividad = new ActividadModel() { NombreActividad = "Nombre" };
            string topicos = "Topico,Topico";

            ActividadesController actividadesController = new ActividadesController(mockActividades.Object);
            mockActividades.Setup(mock => mock.InsertarActividad(actividad)).Returns(false);
            mockActividades.Setup(mock => mock.InsertarTopico(actividad.NombreActividad, topicos)).Returns(false);

            ViewResult vistaResultado = actividadesController.CrearActividad(actividad, topicos) as ViewResult;

            Assert.AreEqual("Hubo un error al guardar los datos ingresados.", vistaResultado.ViewBag.Message);
        }

        [TestMethod]
        public void CrearActividadDevuelveMensajeCorrectoCuandoModeloEsInvalido()
        {
            var mockActividades = new Mock<ActividadesInterfaz>();
            var actividad = new ActividadModel() { NombreActividad = "Nombre" };
            string topicos = "Topico,Topico";

            ActividadesController actividadesController = new ActividadesController(mockActividades.Object);
            actividadesController.ModelState.AddModelError("NombreActividad", "Es necesario ingresar un nombre");
            mockActividades.Setup(mock => mock.InsertarActividad(actividad)).Returns(false);
            mockActividades.Setup(mock => mock.InsertarTopico(actividad.NombreActividad, topicos)).Returns(false);

            ViewResult vistaResultado = actividadesController.CrearActividad(actividad, topicos) as ViewResult;

            Assert.AreEqual("Hay un error en los datos ingresados", vistaResultado.ViewBag.Message);
        }

        [TestMethod]
        public void ListadoDeActividadesDevuelveVistaNoNula()
        {
            var mockActividades = new Mock<ActividadesInterfaz>();
            var actividad = new ActividadModel() { NombreActividad = "Nombre" };
            List<ActividadModel> listaActividades = new List<ActividadModel>()
            {
                new ActividadModel() {}
            };

            ActividadesController actividadesController = new ActividadesController(mockActividades.Object);
            actividadesController.ModelState.AddModelError("NombreActividad", "Es necesario ingresar un nombre");
            mockActividades.Setup(mock => mock.ObtenerActividadesAprobadas()).Returns(listaActividades);

            ViewResult vistaResultado = actividadesController.ListadoDeActividades() as ViewResult;

            Assert.IsNotNull(vistaResultado);
        }

        [TestMethod]
        public void ListadoDeActividadesDevuelveListaActividadesNoNula()
        {
            var mockActividades = new Mock<ActividadesInterfaz>();
            var actividad = new ActividadModel() { NombreActividad = "Nombre" };
            List<ActividadModel> listaActividades = new List<ActividadModel>()
            {
                new ActividadModel() {}
            };

            ActividadesController actividadesController = new ActividadesController(mockActividades.Object);
            actividadesController.ModelState.AddModelError("NombreActividad", "Es necesario ingresar un nombre");
            mockActividades.Setup(mock => mock.ObtenerActividadesAprobadas()).Returns(listaActividades);

            ViewResult vistaResultado = actividadesController.ListadoDeActividades() as ViewResult;

            Assert.IsNotNull(vistaResultado.ViewBag.actividades);
        }

        [TestMethod]
        public void VerActividadDevuelveVistaNoNula()
        {
            var mockActividades = new Mock<ActividadesInterfaz>();
            var actividad = new ActividadModel() { NombreActividad = "Nombre" };
            var nombreActividad = "Nombre";
            var topicos  = new List<string>();
            var listaActividades = new List<ActividadModel>();
            var listaAscientos = new List<AsientoModel>();

            ActividadesController actividadesController = new ActividadesController(mockActividades.Object);
            mockActividades.Setup(mock => mock.ObtenerActividad(nombreActividad)).Returns(actividad);
            mockActividades.Setup(mock => mock.ObtenerTopicosActividad(nombreActividad)).Returns(topicos);
            mockActividades.Setup(mock => mock.ObtenerActividadesRecomendadas("Publico Dirigido","Complejidad")).Returns(listaActividades);
            mockActividades.Setup(mock => mock.ObtenerEntradasDisponiblesPorActividad(nombreActividad)).Returns(0);
            mockActividades.Setup(mock => mock.ObtenerAsientos(nombreActividad)).Returns(listaAscientos);

            ViewResult vistaResultado = actividadesController.ListadoDeActividades() as ViewResult;

            Assert.IsNotNull(vistaResultado);
        }

        [TestMethod]
        public void VerActividadDevuelveActividadNoNula()
        {
            var mockActividades = new Mock<ActividadesInterfaz>();
            var actividad = new ActividadModel() { NombreActividad = "Nombre" };
            var nombreActividad = "Nombre";
            var topicos = new List<string>();
            var listaActividades = new List<ActividadModel>();
            var listaAscientos = new List<AsientoModel>();

            ActividadesController actividadesController = new ActividadesController(mockActividades.Object);
            mockActividades.Setup(mock => mock.ObtenerActividad(nombreActividad)).Returns(actividad);
            mockActividades.Setup(mock => mock.ObtenerTopicosActividad(nombreActividad)).Returns(topicos);
            mockActividades.Setup(mock => mock.ObtenerActividadesRecomendadas("Publico Dirigido", "Complejidad")).Returns(listaActividades);
            mockActividades.Setup(mock => mock.ObtenerEntradasDisponiblesPorActividad(nombreActividad)).Returns(0);
            mockActividades.Setup(mock => mock.ObtenerAsientos(nombreActividad)).Returns(listaAscientos);

            ViewResult vistaResultado = actividadesController.VerActividad(nombreActividad) as ViewResult;

            Assert.IsNotNull(vistaResultado.ViewBag.actividad);
        }

        [TestMethod]
        public void ComprarEntradasDevuelveVistaNoNula()
        {
            var mockActividades = new Mock<ActividadesInterfaz>();
            var actividad = new ActividadModel() { NombreActividad = "Nombre" };
            var nombreActividad = "Nombre";
            var topicos = new List<string>();
            var listaActividades = new List<ActividadModel>();
            var listaAscientos = new List<AsientoModel>();

            ActividadesController actividadesController = new ActividadesController(mockActividades.Object);
            mockActividades.Setup(mock => mock.ObtenerActividad(nombreActividad)).Returns(actividad);
            mockActividades.Setup(mock => mock.ObtenerTopicosActividad(nombreActividad)).Returns(topicos);
            mockActividades.Setup(mock => mock.ObtenerActividadesRecomendadas("Publico Dirigido", "Complejidad")).Returns(listaActividades);
            mockActividades.Setup(mock => mock.ObtenerEntradasDisponiblesPorActividad(nombreActividad)).Returns(0);
            mockActividades.Setup(mock => mock.ObtenerAsientos(nombreActividad)).Returns(listaAscientos);

            ViewResult vistaResultado = actividadesController.ComprarEntradas(nombreActividad) as ViewResult;

            Assert.IsNotNull(vistaResultado);
        }

        [TestMethod]
        public void BuscarActividadNoDevuelveVistaNulaCuandoNoRecibePalabra()
        {
            var mockActividades = new Mock<ActividadesInterfaz>();

            ActividadesController actividadesController = new ActividadesController(mockActividades.Object);

            ViewResult vistaResultado = actividadesController.BuscarActividad() as ViewResult;

            Assert.IsNotNull(vistaResultado);
        }

        [TestMethod]
        public void BuscarActividadNoDevuelveVistaNula()
        {
            var mockActividades = new Mock<ActividadesInterfaz>();
            var actividad = new ActividadModel() { NombreActividad = "Nombre" };
            var palabra = "Nombre";
            var listaActividades = new List<ActividadModel>();

            ActividadesController actividadesController = new ActividadesController(mockActividades.Object);
            mockActividades.Setup(mock => mock.ObtenerActividadesPorBusqueda(palabra)).Returns(listaActividades);

            ViewResult vistaResultado = actividadesController.BuscarActividad(palabra) as ViewResult;

            Assert.IsNotNull(vistaResultado);
        }

        [TestMethod]
        public void BuscarActividadNoDevuelveListaSinActividadesNulas()
        {
            var mockActividades = new Mock<ActividadesInterfaz>();
            var actividad = new ActividadModel() { NombreActividad = "Nombre" };
            var palabra = "Nombre";
            var listaActividades = new List<ActividadModel>();

            ActividadesController actividadesController = new ActividadesController(mockActividades.Object);
            mockActividades.Setup(mock => mock.ObtenerActividadesPorBusqueda(palabra)).Returns(listaActividades);

            ViewResult vistaResultado = actividadesController.BuscarActividad(palabra) as ViewResult;

            CollectionAssert.AllItemsAreNotNull(vistaResultado.ViewBag.actividadesUnicas);
        }
    }
}
