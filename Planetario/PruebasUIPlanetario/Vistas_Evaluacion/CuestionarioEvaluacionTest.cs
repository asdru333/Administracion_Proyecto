using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace PruebasUIPlanetario.UITesting
{
    [TestClass]
    public class CuestionarioEvaluacionTest
    {
        IWebDriver driver;

        private void ConfigurarDriver()
        {
            driver = new ChromeDriver();
            driver.Manage().Window.Maximize();
            driver.Url = "https://localhost:44368/Evaluacion/CuestionarioEvaluacion";
        }

        [TestMethod]
        public void TituloVistaCuestionarioEsCorrecto()
        {
            ConfigurarDriver();
            
            IWebElement titulo = driver.FindElement(By.ClassName("titulo"));
            
            Assert.AreEqual("Califica tu experiencia", titulo.Text);
        }

        [TestMethod]
        public void EnviarFormularioVacioDaError()
        {
            ConfigurarDriver();

            IWebElement botonSubmit = driver.FindElement(By.Id("botonEnviar"));
            botonSubmit.Submit();
            IWebElement titulo = driver.FindElement(By.CssSelector(".alert.alert-warning"));

            Assert.AreEqual("El cuestionario tiene errores. Por favor revise sus respuestas.", titulo.Text);
        }


        [TestMethod]
        public void EviarFormularioIncorrectoDaError()
        {
            ConfigurarDriver();

            IWebElement botonSubmit1 = driver.FindElement(By.CssSelector("input[type=radio]"));
            botonSubmit1.Click();

            IWebElement botonSubmit = driver.FindElement(By.CssSelector("input[type=submit]"));
            botonSubmit.Click();
            IWebElement titulo = driver.FindElement(By.CssSelector(".alert.alert-warning"));

            Assert.AreEqual("El cuestionario tiene errores. Por favor revise sus respuestas.", titulo.Text);
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