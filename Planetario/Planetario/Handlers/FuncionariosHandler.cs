using System;
using System.Collections.Generic;
using System.Data;
using Planetario.Models;

namespace Planetario.Handlers
{
    public class FuncionariosHandler : BaseDatosHandler
    {
        ArchivosHandler manejadorDeImagen = new ArchivosHandler();

        private List<FuncionarioModel> ConvertirTablaALista(DataTable tabla)
        {
            List<FuncionarioModel> funcionarios = new List<FuncionarioModel>();
            foreach (DataRow columna in tabla.Rows)
            {
                funcionarios.Add(
                new FuncionarioModel
                {
                    correo = Convert.ToString(columna["correoPK"]),
                    nombre = Convert.ToString(columna["nombre"]),
                    apellido1 = Convert.ToString(columna["apellido1"]),
                    apellido2 = Convert.ToString(columna["apellido2"]),
                    descripcion = Convert.ToString(columna["descripcion"]),
                    pais = Convert.ToString(columna["pais"]),
                    fechaIncorporacion = Convert.ToString(columna["fechaIncorporacion"]),
                    fechaNacimiento = Convert.ToString(columna["fechaNacimiento"]),
                    genero = Convert.ToString(columna["genero"]),
                    areaExpertis = Convert.ToString(columna["areaExpertis"])
                });
            }
            return funcionarios;
        }

        private List<FuncionarioModel> ObtenerFuncionarios(string consulta)
        {
            DataTable tabla = LeerBaseDeDatos(consulta);
            List<FuncionarioModel> lista = ConvertirTablaALista(tabla);
            return lista;
        }

        public List<FuncionarioModel> ObtenerTodosLosFuncionarios()
        {
            string consulta = "Select * FROM Funcionario F JOIN Persona P ON F.correoPK = P.correoPersonaPK";
            return (ObtenerFuncionarios(consulta));
        }

        public FuncionarioModel ObtenerFuncionario(string correo)
        {
            string consulta = "Select * FROM Funcionario F JOIN Persona P ON F.correoPK = P.correoPersonaPK WHERE correoPK = '" + correo + "';";
            return (ObtenerFuncionarios(consulta)[0]);
        }

        public bool EstaEnTabla(string correo)
        {
            string consulta = "Select * FROM Funcionario F JOIN Persona P ON F.correoPK = P.correoPersonaPK WHERE correoPK = '" + correo + "';";
            return (ObtenerFuncionarios(consulta).Count > 0);
        }

        public bool InsertarFuncionario(FuncionarioModel funcionario)
        {
            string consultaTablaPersona = "INSERT INTO Persona ( correoPersonaPK, nombre, apellido1, apellido2, genero, pais, fechaNacimiento ) "
                + "VALUES ( @correo, @nombre, @apellido1, @apellido2, @genero, @pais, @nacimiento );";

            string consultaTablaFuncionario = "INSERT INTO Funcionario ( correoPK, areaExpertis, fechaIncorporacion, fotoTipo, fotoArchivo, descripcion ) "
                + "VALUES ( @correo, @area, @fechaIncorporacion, @fotoTipo, @fotoArchivo, descripcion);";

            Dictionary<string, object> parametrosPersona = new Dictionary<string, object> {
                {"@correo", funcionario.correo },
                {"@nombre", funcionario.nombre },
                {"@apellido1", funcionario.apellido1 },
                {"@apellido2", funcionario.apellido2 },
                {"@genero", funcionario.genero }, 
                {"@pais", funcionario.pais },
                {"@nacimiento", funcionario.fechaNacimiento}
            };

            Dictionary<string, object> parametrosFuncionario = new Dictionary<string, object>
            {
                {"@correo", funcionario.correo },
                {"@area", funcionario.areaExpertis },
                {"@fechaIncorporacion", funcionario.fechaIncorporacion },
                {"@descripcion", funcionario.descripcion },
                {"@fotoTipo", funcionario.FotoArchivo.ContentType },
            };
            parametrosFuncionario.Add("@fotoArchivo", manejadorDeImagen.ConvertirArchivoABytes(funcionario.FotoArchivo));

            return (InsertarEnBaseDatos(consultaTablaPersona,parametrosPersona) && InsertarEnBaseDatos(consultaTablaFuncionario, parametrosFuncionario));
        }

        public bool InsertarIdiomas(string idioma, string correo)
        {
            string Consulta = "INSERT INTO FuncionarioIdioma VALUES (@correo, @idioma)";

            Dictionary<string, object> valoresParametros = new Dictionary<string, object> {
                {"@correo", correo },
                {"@idioma", idioma }
            };
            return (InsertarEnBaseDatos(Consulta, valoresParametros));
        }

        public IList<string> ObtenerIdiomasFuncionario(string correo) 
        {
            string consulta = "SELECT FI.idioma FROM FuncionarioIdioma FI WHERE FI.correoFuncionarioFK = '" + correo + "';";
            DataTable tablaResultados = LeerBaseDeDatos(consulta);
            List<string> idiomas = new List<string>();

            foreach(DataRow fila in tablaResultados.Rows)
            {
                idiomas.Add(Convert.ToString(fila["idioma"]));
            }
            return idiomas;
        }

        public IList<string> ObtenerTitulosFuncionario(string correo) { 
            string consulta = "SELECT FT.titulo FROM FuncionarioTitulo FT WHERE FT.correoFuncionarioFK = '" + correo + "';";
            DataTable tablaResultados = LeerBaseDeDatos(consulta);
            List<string> titulos = new List<string>();

            foreach(DataRow fila in tablaResultados.Rows)
            {
                titulos.Add(Convert.ToString(fila["titulo"]));
            }
            return titulos;
        }

        public IList<string> ObtenerRolesFuncionario(string correo) { 
            string consulta = "SELECT FR.rol FROM FuncionarioRol FR WHERE FR.correoFuncionarioFK = '" + correo + "';";
            DataTable tablaResultados = LeerBaseDeDatos(consulta);
            List<string> roles = new List<string>();

            foreach(DataRow fila in tablaResultados.Rows)
            {
                roles.Add(Convert.ToString(fila["rol"]));
            }
            return roles;
        }

        public Tuple<byte[], string> ObtenerFoto(string correo)
        {
            string columnaContenido = "fotoArchivo";
            string columnaTipo = "fotoTipo";
            string consulta = "SELECT " + columnaContenido + ", "+ columnaTipo + " FROM Funcionario WHERE correoPK = @correo";
            KeyValuePair<string, object> parametro = new KeyValuePair<string, object>("@correo", correo);
            return ObtenerArchivo(consulta, parametro, columnaContenido, columnaTipo);
        }       
    }
}