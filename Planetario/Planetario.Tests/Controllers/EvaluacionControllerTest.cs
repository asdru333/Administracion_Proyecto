using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Planetario.Controllers;
using System.Web.Mvc;

namespace Planetario.Tests.Controllers
{
    [TestClass]
    public class EvaluacionControllerTest
    {
        [TestMethod]
        public void CuestionarioEvaluacionNoEsNulo()
        {
            EvaluacionController reportesController = new EvaluacionController();

            ViewResult vistaResultado = reportesController.CuestionarioEvaluacion() as ViewResult;

            Assert.IsNotNull(vistaResultado);
        }

    }
}
