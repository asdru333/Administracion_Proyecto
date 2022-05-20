using System;
using System.Data;

namespace Planetario.Handlers
{
    public class PersonaHandler: BaseDatosHandler
    {
        public bool EsUsuarioValido(string correo, string contrasena)
        {
            bool esValido = false;
            string contrasenaFuncionario;

            string consulta = "SELECT [dbo].UFN_compararContrasenas('" + contrasena + "', contraseña) AS 'resultado' FROM Credenciales WHERE correoPersonaFK = '" + correo + "';";

            DataTable tablaResultados = LeerBaseDeDatos(consulta);

            if (tablaResultados.Rows.Count > 0)
            {
                foreach (DataRow columna in tablaResultados.Rows)
                {
                    contrasenaFuncionario = Convert.ToString(columna["resultado"]);
                    if (contrasenaFuncionario == "correcta") { esValido = true; }
                }
            }
            return esValido;
        }

        public bool EsFuncionario(string correo)
        {
            FuncionariosHandler funcionariosHandler = new FuncionariosHandler();
            return (funcionariosHandler.EstaEnTabla(correo));
        }

        public string ObtenerTipoUsuario(string correo)
        {
            FuncionariosHandler funcionariosHandler = new FuncionariosHandler();
            string tipoUsuario;
            if (funcionariosHandler.EstaEnTabla(correo))
            {
                tipoUsuario = "Funcionario";
            }
            else
            {
                tipoUsuario = "Cliente";
            }
            return tipoUsuario;
        }

        public string ObtenerMembresia(string correo) 
        {
            string consultaTablaPersona = "SELECT membresia " +
                                          "FROM Persona " +
                                          "WHERE correoPersonaPK = '" + correo + "' ";
            string membresia;
            DataTable tabla = LeerBaseDeDatos(consultaTablaPersona);
            try { 
            DataRow columna = tabla.Rows[0];
            membresia = Convert.ToString(columna["membresia"]);
            }
            catch
            {
                membresia = "No Disponible";
            }

            return membresia;
        }

        public bool ActualizarMembresia(string correo, string membresia)
        {
            string consultaTablaPersona = "UPDATE Persona " +
                                          "SET membresia = '" + membresia + "', " +
                                          "compraMembresia = GETDATE() " +
                                          "WHERE correoPersonaPK = '" + correo + "' ";

            return (ActualizarEnBaseDatos(consultaTablaPersona, null));
        }
    }
}