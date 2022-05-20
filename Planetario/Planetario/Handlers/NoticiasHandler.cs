using Planetario.Models;
using System;
using System.Collections.Generic;
using System.Web;
using System.Data;

namespace Planetario.Handlers
{
    public class NoticiasHandler : BaseDatosHandler
    {
        private List<NoticiaModel> ConvertirTablaALista(DataTable tabla)
        {
            List<NoticiaModel> noticias = new List<NoticiaModel>();
            foreach (DataRow columna in tabla.Rows)
                noticias.Add(
                new NoticiaModel
                {
                    id = Convert.ToInt32(columna["idNoticiaPK"]),
                    Titulo = Convert.ToString(columna["titulo"]),
                    Cuerpo = Convert.ToString(columna["cuerpo"]),
                    CorreoAutor = Convert.ToString(columna["correoFuncionarioAutorFK"]),
                    Fecha = Convert.ToString(columna["fecha"]),
                    CategoriaNoticia = Convert.ToString(columna["categoriaNoticia"])
                });
            return noticias;
        }

        private List<NoticiaModel> ObtenerNoticias(string consulta)
        {
            DataTable tabla = LeerBaseDeDatos(consulta);
            List<NoticiaModel> lista = ConvertirTablaALista(tabla);
            return lista;
        }

        public List<NoticiaModel> ObtenerTodasLasNoticias()
        {
            string consulta = "SELECT * FROM Noticia ORDER BY fecha DESC";
            return (ObtenerNoticias(consulta));
        }

        public NoticiaModel ObtenerNoticia(string stringId)
        {
            string consulta = "SELECT * FROM Noticia WHERE idNoticiaPK = '" + stringId + "';";
            return (ObtenerNoticias(consulta)[0]);
        }

        public IList<string> ObtenerTopicos(string idNoticiaPK)
        {
            string consulta = "SELECT TN.topicosNoticia FROM NoticiaTopicos TN WHERE TN.idNoticiaFK = '" + idNoticiaPK + "';";
            DataTable tablaResultados = LeerBaseDeDatos(consulta);
            List<string> topicos = new List<string>();
            foreach (DataRow fila in tablaResultados.Rows)
            {
                topicos.Add(Convert.ToString(fila["topicosNoticia"]));
            }
            return topicos;
        }

        public bool InsertarNoticia(NoticiaModel noticia)
        {
            string consulta =
            "INSERT INTO dbo.Noticia(titulo, cuerpo, fecha, correoFuncionarioAutorFK, categoriaNoticia, Imagen1, tipoImagen1, Imagen2, tipoImagen2) VALUES(@titulo, @cuerpo, @fecha, @correo, @categoriaNoticia, " +
            "@imagen1, @tipoImagen1, @imagen2, @tipoImagen2);" +
            "DECLARE @identity int = scope_identity();" +
            "INSERT INTO dbo.NoticiaTopicos(idNoticiaFK, topicosNoticia) VALUES(@identity, @topicoNoticia);";
            Dictionary<string, object> valoresParametros = new Dictionary<string, object>
            {
                { "@topicoNoticia",     "Placeholder"}, //noticia.TopicosNoticia 
                { "@categoriaNoticia",  noticia.CategoriaNoticia },
                { "@titulo",            noticia.Titulo },
                { "@cuerpo",            noticia.Cuerpo },
                { "@correo",            HttpContext.Current.User.Identity.Name},
                { "@fecha",             noticia.Fecha},
                { "@imagen1",           noticia.Imagen1},
                { "@imagen2",           noticia.Imagen2},
                { "@tipoImagen1",       noticia.TipoImagen1},
                { "@tipoImagen2",       noticia.TipoImagen2}
            };
            return (InsertarEnBaseDatos(consulta, valoresParametros));
        }

        public Tuple<byte[], string> ObtenerFoto(string numNoticia)
        {
            string nombreArchivo = "imagen", tipoArchivo = "tipoImagen";
            String Consulta  = "SELECT " + nombreArchivo + ", " + tipoArchivo + " FROM Noticia WHERE idNoticiaPK = @id";
            int id = Int32.Parse(numNoticia);
            KeyValuePair<string, object> valoresParametros = new KeyValuePair<string, object>("@id", id );
            return ObtenerArchivo(Consulta, valoresParametros, nombreArchivo, tipoArchivo);
        }
    }
}