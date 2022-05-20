using Microsoft.VisualStudio.TestTools.UnitTesting;
using Planetario.Models;
using Planetario.Controllers;
using System.Web.Mvc;
using Moq;
using System.Collections.Generic;
using Planetario.Interfaces;

namespace PruebasPlanetarios.Controllers
{
    [TestClass]
    public class EvaluacionControllerTest
    {
        string evaluacion = "Califica tu experiencia";
        CuestionarioEvaluacionRecibirModel modelo = new CuestionarioEvaluacionRecibirModel { Respuestas = new List<string> { "respuesta" }, Comentario = new List<string> { "comentario" } };


        [TestMethod]
        public void VerCuestionarioEvaluacionNoEsNula()
        {
            var mockEvaluacion = new Mock<EvaluacionInterfaz>();
            mockEvaluacion.Setup(servicio => servicio.ObtenerCuestionarioRecibir(evaluacion)).Returns(new CuestionarioEvaluacionRecibirModel());
            EvaluacionController evaluacionController = new EvaluacionController(mockEvaluacion.Object);

            ViewResult vistaResultado = evaluacionController.CuestionarioEvaluacion() as ViewResult;

            Assert.IsNotNull(vistaResultado);
        }

        [TestMethod]
        public void VerCuestionarioEvaluacionEvaluacionNoEsNula()
        {
            var mockEvaluacion = new Mock<EvaluacionInterfaz>();
            mockEvaluacion.Setup(servicio => servicio.ObtenerCuestionarioRecibir(evaluacion)).Returns(new CuestionarioEvaluacionRecibirModel());
            EvaluacionController evaluacionController = new EvaluacionController(mockEvaluacion.Object);

            ViewResult vistaResultado = evaluacionController.CuestionarioEvaluacion() as ViewResult;
            var cuestionario = vistaResultado.ViewBag.Cuestionario;

            Assert.IsNotNull(cuestionario);
        }

        [TestMethod]
        public void VerCuestionarioEvaluacionEvaluacionEsTipoModel()
        {
            var mockEvaluacion = new Mock<EvaluacionInterfaz>();
            mockEvaluacion.Setup(servicio => servicio.ObtenerCuestionarioRecibir(evaluacion)).Returns(new CuestionarioEvaluacionRecibirModel());
            EvaluacionController evaluacionController = new EvaluacionController(mockEvaluacion.Object);

            ViewResult vistaResultado = evaluacionController.CuestionarioEvaluacion() as ViewResult;
            var cuestionario = vistaResultado.ViewBag.Cuestionario;

            Assert.IsInstanceOfType(cuestionario, typeof(CuestionarioEvaluacionRecibirModel));
        }

        [TestMethod]
        public void VerCuestionarioEvaluacionPostNoEsNula()
        {
            var mockEvaluacion = new Mock<EvaluacionInterfaz>();
            mockEvaluacion.Setup(servicio => servicio.ObtenerCuestionarioRecibir(evaluacion)).Returns(new CuestionarioEvaluacionRecibirModel());
            EvaluacionController evaluacionController = new EvaluacionController(mockEvaluacion.Object);

            ViewResult vistaResultado = evaluacionController.CuestionarioEvaluacion(modelo) as ViewResult;

            Assert.IsNotNull(vistaResultado);
        }

        [TestMethod]
        public void VerCuestionarioEvaluacionPostEvaluacionNoEsNula()
        {
            var mockEvaluacion = new Mock<EvaluacionInterfaz>();
            mockEvaluacion.Setup(servicio => servicio.ObtenerCuestionarioRecibir(evaluacion)).Returns(new CuestionarioEvaluacionRecibirModel());
            EvaluacionController evaluacionController = new EvaluacionController(mockEvaluacion.Object);

            ViewResult vistaResultado = evaluacionController.CuestionarioEvaluacion(modelo) as ViewResult;
            var cuestionario = vistaResultado.ViewBag.Cuestionario;

            Assert.IsNotNull(cuestionario);
        }

        [TestMethod]
        public void VerCuestionarioEvaluacionEvaluacionPostEsTipoModel()
        {
            var mockEvaluacion = new Mock<EvaluacionInterfaz>();
            mockEvaluacion.Setup(servicio => servicio.ObtenerCuestionarioRecibir(evaluacion)).Returns(new CuestionarioEvaluacionRecibirModel());
            EvaluacionController evaluacionController = new EvaluacionController(mockEvaluacion.Object);

            ViewResult vistaResultado = evaluacionController.CuestionarioEvaluacion(modelo) as ViewResult;
            var cuestionario = vistaResultado.ViewBag.Cuestionario;

            Assert.IsInstanceOfType(cuestionario, typeof(CuestionarioEvaluacionRecibirModel));
        }

        [TestMethod]
        public void VerCuestionarioEvaluacionPostExitoNoEsNulo()
        {
            var mockEvaluacion = new Mock<EvaluacionInterfaz>();
            mockEvaluacion.Setup(servicio => servicio.ObtenerCuestionarioRecibir(evaluacion)).Returns(new CuestionarioEvaluacionRecibirModel());
            mockEvaluacion.Setup(servicio => servicio.InsertarRespuestas(modelo)).Returns(new bool());
            mockEvaluacion.Setup(servicio => servicio.InsertarComentario(modelo)).Returns(new bool());
            mockEvaluacion.Setup(servicio => servicio.InsertarFuncionalidadesEvaluadas(modelo)).Returns(new bool());
            EvaluacionController evaluacionController = new EvaluacionController(mockEvaluacion.Object);

            ViewResult vistaResultado = evaluacionController.CuestionarioEvaluacion(modelo) as ViewResult;
            var exito = vistaResultado.ViewBag.ExitoAlCrear;

            Assert.IsNotNull(exito);
        }

        [TestMethod]
        public void VerCuestionarioEvaluacionPostExitoEsTipoBool()
        {
            var mockEvaluacion = new Mock<EvaluacionInterfaz>();
            mockEvaluacion.Setup(servicio => servicio.ObtenerCuestionarioRecibir(evaluacion)).Returns(new CuestionarioEvaluacionRecibirModel());
            mockEvaluacion.Setup(servicio => servicio.InsertarRespuestas(modelo)).Returns(new bool());
            mockEvaluacion.Setup(servicio => servicio.InsertarComentario(modelo)).Returns(new bool());
            mockEvaluacion.Setup(servicio => servicio.InsertarFuncionalidadesEvaluadas(modelo)).Returns(new bool());
            EvaluacionController evaluacionController = new EvaluacionController(mockEvaluacion.Object);

            ViewResult vistaResultado = evaluacionController.CuestionarioEvaluacion(modelo) as ViewResult;
            var exito = vistaResultado.ViewBag.ExitoAlCrear;

            Assert.IsInstanceOfType(exito, typeof(bool));
        }
    }
}