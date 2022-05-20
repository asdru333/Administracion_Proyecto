using System.Web.Mvc;
using Planetario.Handlers;
using Planetario.Models;
using Planetario.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Planetario.Controllers
{
    public class VentasController : Controller
    {
        readonly VentasInterfaz ventasInterfaz;
        readonly ProductosInterfaz productosInterfaz;
        readonly CookiesInterfaz cookiesInterfaz;
        readonly DescuentosInterfaz descuentosInterfaz;

        public VentasController()
        {
            ventasInterfaz = new VentasHandler();
            productosInterfaz = new ProductosHandler();
            cookiesInterfaz = new CookiesHandler();
            descuentosInterfaz = new DescuentosHandler();
        }

        public VentasController(VentasInterfaz _servicio)
        {
            ventasInterfaz = _servicio;
        }

        public VentasController(ProductosInterfaz _servicio)
        {
            productosInterfaz = _servicio;
        }

        public VentasController(DescuentosInterfaz _servicio)
        {
            descuentosInterfaz = _servicio;
        }

        public VentasController(VentasInterfaz ventas, CookiesInterfaz cookies)
        {
            ventasInterfaz = ventas;
            cookiesInterfaz = cookies;
        }

        public ActionResult ListaProductos()
        {
            List<SelectListItem> categorias = new List<SelectListItem>()
            {
                new SelectListItem(){Text="Telescopios",Value="Telescopios"}
            };
            ViewBag.categorias = categorias;
            return View("ListaProductos","_LayoutAlternativo");
        }

        [HttpGet]
        public ActionResult VerProducto(int id)
        {
            ViewBag.producto = productosInterfaz.ObtenerProducto(id);
            return View();
        }

        public JsonResult ListaProductosFiltrados(double precioMinimo, double precioMaximo, string categoria, string palabraBusqueda, string orden)
        {
            return Json(productosInterfaz.ObtenerProductosFiltrados(precioMinimo, precioMaximo, categoria, palabraBusqueda, orden), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Carrito()
        {            
            ActionResult resultado;
            if (cookiesInterfaz.SesionIniciada()){
                PersonaHandler personasHandler = new PersonaHandler();                          
                string correoUsuario = cookiesInterfaz.CorreoUsuario();
                string membresia = personasHandler.ObtenerMembresia(correoUsuario);
                ViewBag.Membresia = membresia;
                int cantidadEntradas = ventasInterfaz.ObtenerCantidadDeEntradasDelCarrito(correoUsuario);
                int cantidadProductos = ventasInterfaz.ObtenerCantidadDeProductosDelCarrito(correoUsuario);
                int cantidadItems = cantidadEntradas + cantidadProductos;
                double total = 0;
                Console.WriteLine(cantidadEntradas);
                ViewBag.CantidadItems = cantidadItems;

                if (cantidadEntradas != 0) 
                {
                    ViewBag.ListaEntradas = ventasInterfaz.ObtenerTodasLasEntradasDelCarrito(correoUsuario);
                    total += ventasInterfaz.ObtenerPrecioTotalDeEntradasDelCarrito(correoUsuario);
                    Console.WriteLine(total);
                }

                if (cantidadProductos != 0)
                {
                    ViewBag.ListaProductos = ventasInterfaz.ObtenerTodosLosProductosDelCarrito(correoUsuario);
                    total += ventasInterfaz.ObtenerPrecioTotalDeProductosDelCarrito(correoUsuario);
                }
                
                ViewBag.Precio = total;
                ViewBag.IVA = total * 0.13;
                ViewBag.PrecioTotal = ViewBag.Precio + ViewBag.IVA;
                resultado = View();
            }
            else
            {
                resultado = RedirectToAction("IniciarSesion", "Personas");                
            }
            return resultado;
        }

        [HttpGet]
        public ActionResult Pago(string formaDeCompra)
        {
            ActionResult resultado;
            if (cookiesInterfaz.SesionIniciada())
            {
                ViewBag.FormaDeCompra = formaDeCompra;
                PersonaHandler personasHandler = new PersonaHandler();
                string correoUsuario = cookiesInterfaz.CorreoUsuario();
                string membresia = personasHandler.ObtenerMembresia(correoUsuario);
                ViewBag.Membresia = membresia;
                int cantidadEntradas = ventasInterfaz.ObtenerCantidadDeEntradasDelCarrito(correoUsuario);
                int cantidadProductos = ventasInterfaz.ObtenerCantidadDeProductosDelCarrito(correoUsuario);
                int cantidadItems = cantidadEntradas + cantidadProductos;
                double total = 0;
                ViewBag.CantidadItems = cantidadItems;

                if (cantidadEntradas != 0)
                {
                    ViewBag.ListaEntradas = ventasInterfaz.ObtenerTodasLasEntradasDelCarrito(correoUsuario);
                    total += ventasInterfaz.ObtenerPrecioTotalDeEntradasDelCarrito(correoUsuario);
                    Console.WriteLine(total);
                }

                if (cantidadProductos != 0)
                {
                    ViewBag.ListaProductos = ventasInterfaz.ObtenerTodosLosProductosDelCarrito(correoUsuario);
                    total += ventasInterfaz.ObtenerPrecioTotalDeProductosDelCarrito(correoUsuario);
                }

                ViewBag.Precio = total;
                ViewBag.IVA = total * 0.13;
                if(formaDeCompra == "Express")
                {
                    ViewBag.PrecioTotal = ViewBag.Precio + ViewBag.IVA + 2000;
                }
                else
                {
                    ViewBag.PrecioTotal = ViewBag.Precio + ViewBag.IVA;
                }
                
                resultado = View();
            }
            else
            {
                resultado = RedirectToAction("IniciarSesion", "Personas");
            }
            return resultado;
        }

        [HttpPost]
        public ActionResult Pago(InscripcionModel datos)
        {
            ViewBag.FormaDeCompra = Request.Form["formaDeCompra"];
            string forma = ViewBag.FormaDeCompra;
            ViewBag.exito = false;
            ActionResult resultado = View();
            try
            {
                if(ModelState.IsValid)
                {
                    string correo = cookiesInterfaz.CorreoUsuario();
                    List<ComprableModel> listaProductos = ventasInterfaz.ObtenerTodosLosProductosDelCarrito(correo);
                    List<ComprableModel> listaEntradas = ventasInterfaz.ObtenerTodasLasEntradasDelCarrito(correo);
                    Dictionary<int, int> comprables = new Dictionary<int, int>();
                    foreach(ComprableModel producto in listaProductos)
                    {
                        comprables.Add(producto.Id, producto.CantidadCarrito);
                        EliminarElementoDelCarritoDelUsuario(producto.Id);
                    }
                    foreach (ComprableModel entrada in listaEntradas)
                    {
                        comprables.Add(entrada.Id, entrada.CantidadCarrito);
                        EliminarElementoDelCarritoDelUsuario(entrada.Id);
                    }
                    FacturasHandler facturasHandler = new FacturasHandler();
                    facturasHandler.InsertarFactura(correo,comprables);
                    resultado = RedirectToAction("CompraExitosa", "Ventas");
                }
                else
                {
                    return Pago(forma);
                }
            }
            catch
            {
                return Pago(forma);
            }
            return resultado;
        }

        [HttpGet]
        public ActionResult AgregarCupon()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AgregarCupon(DescuentoModel cupon)
        {
            ViewBag.ExitoAlCrear = false;
            try
            {
                DescuentosHandler descuentoHandler = new DescuentosHandler();
                ViewBag.ExitoAlCrear = descuentoHandler.InsertarDescuento(cupon);
                if (ViewBag.ExitoAlCrear)
                {
                    ViewBag.Message = "El cupón " + cupon.Codigo + " fue creado con éxito";
                }

                return View();
            }
            catch
            {
                ViewBag.Message = "Algo salió mal y no fue posible crear el cupón.";
                return View();
            }
        }

        [HttpGet]
        public ActionResult ComprarAhora(int id,int cantidad,string formaDeCompra)
        {
            ActionResult resultado = RedirectToAction("IniciarSesion", "Personas");
            if(cookiesInterfaz.SesionIniciada())
            {
                ViewBag.FormaDeCompra = formaDeCompra;
                string correoUsuario = cookiesInterfaz.CorreoUsuario();
                PersonaHandler personaHandler = new PersonaHandler();
                ViewBag.membresia = personaHandler.ObtenerMembresia(correoUsuario);
                ViewBag.cantidad = cantidad;
                ViewBag.Id = id;
                ComprableModel comprable = ventasInterfaz.ObtenerComprable(id);
                ViewBag.Precio = comprable.Precio;
                ViewBag.IVA = comprable.Precio * 0.13;
                ViewBag.PrecioTotal = ViewBag.Precio + ViewBag.IVA;
                ViewBag.Nombre = comprable.Nombre;
                resultado = View();
            }
            return resultado;
        }

        [HttpPost]
        public ActionResult ComprarAhora(PagoModel datos)
        {
            if (datos.Direccion == null)
                datos.Direccion = "Sin direccion";
            if (datos.Telefono == null)
                datos.Telefono = "0";
            if (datos.Nombre == null)
                datos.Nombre = "Sin nombre";
            int id = datos.comprable;
            int cantidad = datos.cantidadCompra;
            string formaDeCompra = Request.Form["formaDeCompra"];
            ViewBag.FormaDeCompra = formaDeCompra;
            string correoUsuario = cookiesInterfaz.CorreoUsuario();
            PersonaHandler personaHandler = new PersonaHandler();
            ViewBag.membresia = personaHandler.ObtenerMembresia(correoUsuario);
            ViewBag.cantidad = cantidad;
            ViewBag.Id = id;
            ComprableModel comprable = ventasInterfaz.ObtenerComprable(id);
            ViewBag.Precio = comprable.Precio;
            ViewBag.IVA = comprable.Precio * 0.13;
            ViewBag.PrecioTotal = ViewBag.Precio + ViewBag.IVA;
            ViewBag.Nombre = comprable.Nombre;

            ActionResult resultado = View();
            try
            {
                if (ModelState.IsValid)
                {
                    string correo = cookiesInterfaz.CorreoUsuario();
                    Dictionary<int, int> diccionario = new Dictionary<int, int>();
                    diccionario.Add(datos.comprable, datos.cantidadCompra);
                    Debug.WriteLine(datos.comprable + "\t\t" + datos.cantidadCompra);
                    FacturasHandler facturasHandler = new FacturasHandler();
                    facturasHandler.InsertarFactura(correo, diccionario);
                    resultado = RedirectToAction("CompraExitosa", "Ventas");
                }
                else
                {
                    resultado = ComprarAhora(id, cantidad, formaDeCompra);
                }
            }
            catch
            {
                ViewBag.Mensaje = "Error con el servidor";
                resultado = ComprarAhora(id, cantidad, formaDeCompra);
            }
            return resultado;
        }

        [HttpGet]
        public JsonResult EliminarElementoDelCarritoDelUsuario(int idComprable)
        {
            bool exito = false;
            if(cookiesInterfaz.SesionIniciada())
            {
                string correoUsuario = cookiesInterfaz.CorreoUsuario();
                exito = ventasInterfaz.EliminarDelCarrito(correoUsuario, idComprable);
            }            
            return Json(new { Exito = exito }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult DisminiuirLaCantidadDelElementoDelCarritoDelUsuario(string correoUsuario, int idComprable)
        {
            var exito = ventasInterfaz.DisminiuirLaCantidadDelElementoDelCarrito(correoUsuario, idComprable);
            return Json(new { Exito = exito }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult AumentarLaCantidadDelElementoDelCarritoDelUsuario(string correoUsuario, int idComprable)
        {
            var exito = ventasInterfaz.AumentarLaCantidadDelElementoDelCarrito(correoUsuario, idComprable);
            return Json(new { Exito = exito }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult AgregarProducto()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AgregarProducto(ProductoModel producto)
        {
            ViewBag.ExitoAlCrear = false;
            try
            {
                if (ModelState.IsValid)
                {
                    ViewBag.ExitoAlCrear = productosInterfaz.InsertarProducto(producto);
                    if (ViewBag.ExitoAlCrear)
                    {
                        ViewBag.Mensaje = "El producto" + " " + producto.Nombre + " fue agregado con éxito";
                        ModelState.Clear();
                    }
                    else
                    {
                        ViewBag.Mensaje = "Hubo un error en el servidor";
                    }
                } 
                else
                {
                    ViewBag.Mensaje = "Hubo un error en los datos ingresados";
                }
                return View();
            }
            catch
            {
                ViewBag.Mensaje = "Hubo un error en el servidor";
                return View();
            }
        }

        [HttpGet]
        public ActionResult RealizarCompra()
        {
            DatosHandler datosHandler = new DatosHandler();
            ViewBag.paises = datosHandler.SelectListPaises();
            ViewBag.nivelesEducativos = datosHandler.SelectListNivelesEducativos();
            ViewBag.generos = datosHandler.SelectListGeneros();
            return View();
        }

        [HttpGet]
        public JsonResult AgregarAlCarrito(int idComprable, int cantidad)
        {
            int cantidadProducto = 0;
            var exito = false;
            try
            {
                string correoUsuario = cookiesInterfaz.CorreoUsuario();
                cantidadProducto = ventasInterfaz.ObtenerCantidadDeProductoEspecifico(idComprable);
                if(cantidadProducto != 0) { 
                    for(int i = 0; i < cantidad; i++)
                    {
                        ventasInterfaz.AumentarLaCantidadDelElementoDelCarrito(correoUsuario, idComprable);
                    }
                    exito = true;
                }
                else
                {
                    exito = ventasInterfaz.AgregarAlCarrito(idComprable, cantidad);
                }
            }
            catch
            {
                exito = ventasInterfaz.AgregarAlCarrito(idComprable, cantidad);
            }
            
            return Json(new { Exito = exito }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult ObtenerImagen(int id)
        {
            ProductosHandler productosHandler = new ProductosHandler();
            var tupla = productosHandler.ObtenerFoto(id);
            return File(tupla.Item1, tupla.Item2);
        }

        [HttpGet]
        public JsonResult ObtenerPorcentajeDescuento(string codigo)
        {
            var resultado = Json(descuentosInterfaz.ObtenerPorcentajeDescuento(codigo), JsonRequestBehavior.AllowGet);
            descuentosInterfaz.EliminarDescuento(codigo);
            return resultado;
        }

        [HttpGet]
        public JsonResult ObtenerMembresia()
        {
            PersonaHandler personasHandler = new PersonaHandler();
            string correoUsuario = cookiesInterfaz.CorreoUsuario();
            string membresia = personasHandler.ObtenerMembresia(correoUsuario);
            return Json(new { membresia = membresia }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ObtenerCantidadCarrito()
        {
            VentasHandler ventasHandler = new VentasHandler();
            string correoUsuario = cookiesInterfaz.CorreoUsuario();
            int cantidadProductos = ventasHandler.ObtenerCantidadDeProductosDelCarrito(correoUsuario);
            return Json(new { cantidad = cantidadProductos }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult CompraExitosa()
        {
            return View();
        }
    }
}