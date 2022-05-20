using System;
using System.Collections.Generic;
using System.Data;
using Planetario.Models;
using System.Web;

namespace Planetario.Handlers
{
    public class MaterialesEducativosHandler
    {
        private readonly BaseDatosHandler BaseDatos;
        private string Consulta;

        public MaterialesEducativosHandler()
        {
            BaseDatos = new BaseDatosHandler();
        }

        public bool AlmacenarMaterialEducativo(MaterialEducativoModel material)
        {
            ArchivosHandler manejadorArchivo = new ArchivosHandler();
            string columnas, valores;
            object archivoVistaPrevia = System.Data.SqlTypes.SqlBinary.Null;
            object tipoArchivoVistaPrevia = System.Data.SqlTypes.SqlBinary.Null; ;

            columnas = "( tituloMaterialEducativoPK, categoriaMaterialEducativo, imagenVistaPrevia, " +
                "tipoArchivoVistaPrevia, materialArchivo, materialTipoArchivo, correoFuncionarioFK, publicoDirigidoMaterial )";
            valores  = "( @titulo, @categoria, @imagenVistaPrevia, @tipoArchivoVistaPrevia, @archivo, " +
                "@tipoArchivo, @correoResponsable, @publicoDirigidoMaterial ); ";
            Consulta = "INSERT INTO MaterialEducativo " + columnas + " VALUES " + valores;

            Dictionary<string, object> valoresParametros = new Dictionary<string, object>
            {
                { "@titulo", material.Titulo },
                { "@categoria", material.Categoria },
                { "@correoResponsable",  HttpContext.Current.User.Identity.Name},
                { "@tipoArchivo", material.MaterialArchivo.ContentType },
                { "@publicoDirigidoMaterial", material.PublicoDirigido }
            };
            valoresParametros.Add("@archivo", manejadorArchivo.ConvertirArchivoABytes(material.MaterialArchivo));

            if (material.HayVistaPrevia())
            {
                archivoVistaPrevia = manejadorArchivo.ConvertirArchivoABytes(material.ImagenVistaPrevia);
                tipoArchivoVistaPrevia = material.ImagenVistaPrevia.ContentType;
            }
            valoresParametros.Add("@imagenVistaPrevia", archivoVistaPrevia);
            valoresParametros.Add("@tipoArchivoVistaPrevia", tipoArchivoVistaPrevia);

            bool exito = BaseDatos.InsertarEnBaseDatos(Consulta, valoresParametros);

            return exito;
        }

        public List<MaterialEducativoModel> obtenerMateriales()
        {
            List<MaterialEducativoModel> material = new List<MaterialEducativoModel>();
            Consulta = "SELECT E.tituloMaterialEducativoPK, E.categoriaMaterialEducativo, E.correoFuncionarioFK, " +
                                "E.publicoDirigidoMaterial, F.nombre + F.apellido1 AS nombre " +
                                "FROM MaterialEducativo E " +
                                "JOIN Funcionario F " +
                                "ON E.correoFuncionarioFK = F.correoPK ;";
            DataTable tablaResultado = BaseDatos.LeerBaseDeDatos(Consulta);
            foreach (DataRow columna in tablaResultado.Rows)
            {
                MaterialEducativoModel modelo = new MaterialEducativoModel
                {
                    Titulo = Convert.ToString(columna["tituloMaterialEducativoPK"]),
                    Categoria = Convert.ToString(columna["categoriaMaterialEducativo"]),
                    CorreoResponsable = Convert.ToString(columna["correoFuncionarioFK"]),
                    PublicoDirigido = Convert.ToString(columna["publicoDirigidoMaterial"]),
                    NombreResponsable = Convert.ToString(columna["nombre"])
                };
                material.Add(modelo);
            }
            return material;
        }

        public Tuple<byte[], string> descargarContenido(string titulo)
        {
            string nombreArchivo = "materialArchivo", tipoArchivo = "materialTipoArchivo";
            Consulta = "SELECT "+ nombreArchivo +", "+ tipoArchivo + ", tituloMaterialEducativoPK FROM MaterialEducativo WHERE tituloMaterialEducativoPK = @titulo";
            KeyValuePair<string, object> valoresParametros = new KeyValuePair<string, object>( "@titulo",  titulo);
            return BaseDatos.ObtenerArchivo(Consulta, valoresParametros, nombreArchivo, tipoArchivo);
        }

        public Tuple<byte[], string> descargarVistaPrevia(string titulo)
        {
            string nombreColumnaArchivo = "imagenVistaPrevia", columnaTipoArchivo = "tipoArchivoVistaPrevia";
            Consulta = "SELECT " + nombreColumnaArchivo + ", " + columnaTipoArchivo + ", titulo FROM MaterialEducativo WHERE tituloMaterialEducativo = @titulo";

            KeyValuePair<string, object> valoresParametros = new KeyValuePair<string, object>( "@titulo",  titulo );

            return BaseDatos.ObtenerArchivo(Consulta, valoresParametros, nombreColumnaArchivo, columnaTipoArchivo);
        }

        public List<MaterialEducativoModel> obtenerMaterialBuscado(string palabra)
        {
            List<MaterialEducativoModel> material = new List<MaterialEducativoModel>();
            Consulta = "SELECT E.tituloMaterialEducativoPK, E.categoriaMaterialEducativo, E.correoFuncionarioFK, " +
                                "E.publicoDirigidoMaterial, F.nombre + F.apellido1 AS nombre " +
                                "FROM MaterialEducativo E " +
                                "JOIN Funcionario F " +
                                "ON E.correoFuncionarioFK = F.correoPK " +
                        "WHERE E.tituloMaterialEducativoPK LIKE '%" + palabra + "%';";
            DataTable tablaResultado = BaseDatos.LeerBaseDeDatos(Consulta);
            foreach (DataRow columna in tablaResultado.Rows)
            {
                MaterialEducativoModel modelo = new MaterialEducativoModel
                {
                    Titulo = Convert.ToString(columna["tituloMaterialEducativoPK"]),
                    Categoria = Convert.ToString(columna["categoriaMaterialEducativo"]),
                    CorreoResponsable = Convert.ToString(columna["correoFuncionarioFK"]),
                    PublicoDirigido = Convert.ToString(columna["publicoDirigidoMaterial"]),
                    NombreResponsable = Convert.ToString(columna["nombre"])
                };
                material.Add(modelo);
            }
            return material;
        }

        public List<MaterialEducativoModel> obtenerTodasLosMaterialesRecomendados(string publicoDirigido, string categoria)
        {
            List<MaterialEducativoModel> material = new List<MaterialEducativoModel>();
            string Consulta = "SELECT * FROM MaterialEducativo WHERE publicoDirigidoMaterial = '" + publicoDirigido + "'AND categoriaMaterialEducativo = '" + categoria + "';"; ;
            DataTable tablaResultado = BaseDatos.LeerBaseDeDatos(Consulta);
            foreach (DataRow columna in tablaResultado.Rows)
            {
                MaterialEducativoModel modelo = new MaterialEducativoModel
                {
                    Titulo = Convert.ToString(columna["tituloMaterialEducativoPK"]),
                    Categoria = Convert.ToString(columna["categoriaMaterialEducativo"]),
                    CorreoResponsable = Convert.ToString(columna["correoFuncionarioFK"]),
                    PublicoDirigido = Convert.ToString(columna["publicoDirigidoMaterial"]),
                };
                material.Add(modelo);
            }
            return material;
        }

         public MaterialEducativoModel buscarActividad(string nombre)
        {
            MaterialEducativoModel material = null;
            string Consulta = "Select * FROM MaterialEducativo WHERE tituloMaterialEducativoPK = '" + nombre + "';";
            DataTable tablaResultado = BaseDatos.LeerBaseDeDatos(Consulta);
            if (tablaResultado.Rows.Count >= 1)
            {
                material = new MaterialEducativoModel
                {
                    Titulo = Convert.ToString(tablaResultado.Rows[0]["tituloMaterialEducativoPK"]),
                    Categoria = Convert.ToString(tablaResultado.Rows[0]["categoriaMaterialEducativo"]),
                    CorreoResponsable = Convert.ToString(tablaResultado.Rows[0]["correoFuncionarioFK"]),
                    PublicoDirigido = Convert.ToString(tablaResultado.Rows[0]["publicoDirigidoMaterial"]),              
                };
            }
            return material;
        }
    }
}