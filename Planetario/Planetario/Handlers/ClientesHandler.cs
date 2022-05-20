using System;
using System.Collections.Generic;
using System.Data;
using Planetario.Models;

namespace Planetario.Handlers
{
    public class ClientesHandler: BaseDatosHandler
    {
        private List<ClienteModel> ConvertirTablaALista(DataTable tabla)
        {
            List<ClienteModel> clientes = new List<ClienteModel>();
            foreach (DataRow columna in tabla.Rows)
            {
                clientes.Add(
                new ClienteModel
                {
                    correo = Convert.ToString(columna["correoPK"]),
                    nombre = Convert.ToString(columna["nombre"]),
                    apellido1 = Convert.ToString(columna["apellido1"]),
                    apellido2 = Convert.ToString(columna["apellido2"]),
                    pais = Convert.ToString(columna["pais"]),
                    fechaNacimiento = Convert.ToString(columna["fechaNacimiento"]),
                    genero = Convert.ToString(columna["genero"]),
                    nivelEducativo = Convert.ToString(columna["nivelEducativo"])
                });
            }
            return clientes;
        }

        private List<ClienteModel> ObtenerClientes(string consulta)
        {
            DataTable tabla = LeerBaseDeDatos(consulta);
            List<ClienteModel> lista = ConvertirTablaALista(tabla);
            return lista;
        }

        public List<ClienteModel> ObtenerTodosLosClientes()
        {
            string consulta = "SELECT * FROM Cliente C JOIN Persona P ON C.correoClientePK = P.correoPersonaPK";
            return (ObtenerClientes(consulta));
        }

        public ClienteModel ObtenerCliente(string correo)
        {
            string consulta = "SELECT * FROM Cliente C JOIN Persona P ON C.correoClientePK = P.correoPersonaPK WHERE C.correoClientePK  = '" + correo + "';";
            return (ObtenerClientes(consulta)[0]);
        }

        public bool InsertarCliente(PersonaModel persona)
        {

            string consultaTablaPersona = "INSERT INTO Persona ( correoPersonaPK, nombre, apellido1, apellido2, genero, pais, fechaNacimiento, membresia ) "
                + "VALUES ( @correo, @nombre, @apellido1, @apellido2, @genero, @pais, @nacimiento, @membresia );";

            Dictionary<string, object> parametrosPersona = new Dictionary<string, object> {
                {"@correo", persona.correo },
                {"@nombre", persona.nombre },
                {"@apellido1", persona.apellido1 },
                {"@apellido2", persona.apellido2 },
                {"@genero", persona.genero },
                {"@pais", persona.pais },
                {"@nacimiento", persona.fechaNacimiento},
                {"@membresia", "Terrestre" }
            };

            if (persona.apellido2 == null)
            {
                consultaTablaPersona = "INSERT INTO Persona ( correoPersonaPK, nombre, apellido1, genero, pais, fechaNacimiento, membresia ) "
                + "VALUES ( @correo, @nombre, @apellido1, @genero, @pais, @nacimiento, @membresia );";

                parametrosPersona = new Dictionary<string, object> {
                {"@correo", persona.correo },
                {"@nombre", persona.nombre },
                {"@apellido1", persona.apellido1 },
                {"@genero", persona.genero },
                {"@pais", persona.pais },
                {"@nacimiento", persona.fechaNacimiento},
                {"@membresia", "Terrestre" }
            };
            }


            string consultaTablaCliente = "INSERT INTO Cliente ( correoClientePK, nivelEducativo ) "
                + "VALUES ( @correo, @nivelEducativo );";

            string consultaTablaCredencial = "INSERT INTO Credenciales (correoPersonaFK, contraseña) VALUES (@correo, PWDENCRYPT(@contrasena))";

            Dictionary<string, object> parametrosCliente = new Dictionary<string, object>
            {
                {"@correo", persona.correo },
                {"@nivelEducativo", persona.nivelEducativo },
            };

            Dictionary<string, object> parametrosCredencial = new Dictionary<string, object>
            {
                {"@correo", persona.correo },
                {"@contrasena", persona.contrasena },
            };

            return (InsertarEnBaseDatos(consultaTablaPersona, parametrosPersona) && InsertarEnBaseDatos(consultaTablaCliente, parametrosCliente) && InsertarEnBaseDatos(consultaTablaCredencial, parametrosCredencial));
        }
    }
}