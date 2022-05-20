using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace PruebasUIPlanetario.UITesting
{
    [TestClass]
    public class ReportesTests
    {
        IWebDriver driver;
        string avanzadoURL = "https://localhost:44368/Reportes/ReporteMercadeo";
        string simpleURL = "https://localhost:44368/Reportes/Reporte";

        private void ConfigurarDriver(string URL)
        {
            driver = new ChromeDriver();
            driver.Manage().Window.Maximize();
            driver.Url = URL;
        }

        [TestMethod]
        public void ReporteDeVentasTituloEsCorrecto()
        {
            ConfigurarDriver(simpleURL);

            IWebElement titulo = driver.FindElement(By.ClassName("titulo"));

            Assert.AreEqual("Reporte de ventas simple", titulo.Text);
        }

        [TestMethod]
        public void TituloVistaReportesAvanzadosEsCorrecto()
        {
            ConfigurarDriver(avanzadoURL);

            IWebElement titulo = driver.FindElement(By.ClassName("titulo"));

            Assert.AreEqual("Reporte de Mercadeo", titulo.Text);
        }

        [TestMethod]
        public void AcordionExtranjeroFuncionaCorrectamente()
        {
            ConfigurarDriver(avanzadoURL);

            IWebElement acordionExtranjeros = driver.FindElement(By.Id("acordion-extranjeros"));
            acordionExtranjeros.Click();

            Assert.AreEqual("Datos sobre los productos que compran los extranjeros clientes extranjeros", acordionExtranjeros.Text);
        }

        [TestMethod]
        public void AcordionFiltrosFuncionaCorrectamente()
        {
            ConfigurarDriver(avanzadoURL);

            IWebElement acordionFiltros = driver.FindElement(By.Id("acordion-filtros"));
            acordionFiltros.Click();

            Assert.AreEqual("Productos por filtros", acordionFiltros.Text);
        }

        [TestMethod]
        public void AcordionParejasFuncionaCorrectamente()
        {
            ConfigurarDriver(avanzadoURL);

            IWebElement acordionParejas = driver.FindElement(By.Id("acordion-parejas"));
            acordionParejas.Click();

            Assert.AreEqual("Parejas de productos que se suelen comprar juntos", acordionParejas.Text);
        }

        [TestCleanup]
        public void TearDown()
        {
            if (driver != null)
            {
                driver.Quit();
            }
        }
    }
}