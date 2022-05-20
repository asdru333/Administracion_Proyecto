using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Threading;

namespace PruebasUIPlanetario.UITesting
{
    [TestClass]
    public class CarritoTests
    {
        IWebDriver driver;

        private void ConfigurarDriver()
        {
            driver = new ChromeDriver();
            driver.Manage().Window.Maximize();
            driver.Url = "https://localhost:44368/Personas/IniciarSesion";
        }

        [TestMethod]
        public void TestIngresoACarritoComoFuncionario()
        {
            ConfigurarDriver();
            LogInFuncionario();
            int sleep = 1000;

            Thread.Sleep(sleep);

            IWebElement botonPerfil = driver.FindElement(By.Id("navbarDropdownMenuSettings"));
            botonPerfil.Click();

            IWebElement navbar = driver.FindElement(By.Id("carrito"));
            navbar.Click();

            IWebElement titulo = driver.FindElement(By.ClassName("titulo"));

            Assert.AreEqual("Carrito", titulo.Text);

            driver.Quit();
        }

        [TestMethod]
        public void TestAgregarYEliminarProductoEnfriadorDeLatas()
        {
            ConfigurarDriver();
            LogInCliente();
            int sleep = 1000;

            IWebElement navbar = driver.FindElement(By.Id("navTienda"));
            navbar.Click();

            Thread.Sleep(6000);

            IWebElement botonAgregar = driver.FindElement(By.Id("agregar_18"));
            botonAgregar.Click();

            Thread.Sleep(sleep);

            IWebElement botonSeguir = driver.FindElement(By.Id("seguirComprando"));
            botonSeguir.Click();

            Thread.Sleep(sleep);

            IWebElement botonPerfil = driver.FindElement(By.Id("navbarDropdownMenuSettings"));
            botonPerfil.Click();

            Thread.Sleep(sleep);

            navbar = driver.FindElement(By.Id("carrito"));
            navbar.Click();

            Thread.Sleep(sleep);

            IWebElement botonEliminar = driver.FindElement(By.Id("eliminar_18"));
            botonEliminar.Click();

            Thread.Sleep(sleep);

            IWebElement botonConfirmar = driver.FindElement(By.Id("BotonConfirmarSi"));
            botonConfirmar.Click();

            Thread.Sleep(3000);

            IWebElement titulo = driver.FindElement(By.Id("vacio"));

            Assert.AreEqual("No tienes artículos.", titulo.Text);

            driver.Quit();
        }

        [TestMethod]
        public void TestVerProductoEnfiradorDeLatas()
        {
            ConfigurarDriver();
            LogInCliente();
            int sleep = 1000;

            IWebElement navbar = driver.FindElement(By.Id("navTienda"));
            navbar.Click();

            Thread.Sleep(6000);

            IWebElement botonVer = driver.FindElement(By.Id("18"));
            botonVer.Click();

            Thread.Sleep(sleep);

            IWebElement titulo = driver.FindElement(By.Id("descripcion"));

            Assert.AreEqual("Los enfriadores de latas de acero inoxidable mantienen frías las latas de aluminio.", titulo.Text);

            driver.Quit();
        }

        [TestMethod]
        public void TestBuscarProductoTelescopio()
        {
            ConfigurarDriver();
            LogInCliente();
            int sleep = 1000;

            IWebElement navbar = driver.FindElement(By.Id("navTienda"));
            navbar.Click();

            Thread.Sleep(6000);

            IWebElement barraBusqueda = driver.FindElement(By.Id("busqueda"));
            barraBusqueda.SendKeys("tele");

            Thread.Sleep(sleep);

            IWebElement botonBuscar = driver.FindElement(By.Id("buscar"));
            botonBuscar.Click();

            Thread.Sleep(6000);

            IWebElement botonVer = driver.FindElement(By.Id("17"));
            botonVer.Click();

            Thread.Sleep(sleep);

            IWebElement titulo = driver.FindElement(By.Id("descripcion"));

            Assert.AreEqual("Imagen ultra clara con telescopio refractante actualizado: el telescopio astronómico cuenta con una longitud focal de 19.685 in.", titulo.Text);

            driver.Quit();
        }


        [TestMethod]
        private void LogInFuncionario()
        {
            IWebElement login = driver.FindElement(By.Id("login"));
            login.Click();

            IWebElement loginUsername = driver.FindElement(By.Id("correo"));
            IWebElement loginPassword = driver.FindElement(By.Id("contrasena"));
            IWebElement startSession = driver.FindElement(By.Id("ingresar"));

            loginUsername.SendKeys("danielmonge25@hotmail.com");
            loginPassword.SendKeys("password");
            startSession.Click();
        }

        [TestMethod]
        private void LogInCliente()
        {
            IWebElement login = driver.FindElement(By.Id("login"));
            login.Click();

            IWebElement loginUsername = driver.FindElement(By.Id("correo"));
            IWebElement loginPassword = driver.FindElement(By.Id("contrasena"));
            IWebElement startSession = driver.FindElement(By.Id("ingresar"));

            loginUsername.SendKeys("carl@gmail.com");
            loginPassword.SendKeys("password");
            startSession.Click();
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