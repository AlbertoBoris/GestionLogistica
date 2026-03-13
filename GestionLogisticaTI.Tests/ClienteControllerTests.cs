using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GestionLogisticaTI.ViewModels;
using DataValidator = System.ComponentModel.DataAnnotations.Validator;

namespace GestionLogisticaTI.Tests
{
    [TestClass]
    public class ClienteControllerTests
    {
        [TestMethod]
        public void ClienteViewModel_EsActivo_RetornaTrue_CuandoEstadoEsActivo()
        {
            var model = new ClienteViewModel { Estado = "Activo" };

            Assert.IsTrue(model.EsActivo, "EsActivo debe ser true cuando Estado es 'Activo'");
        }

        [TestMethod]
        public void ClienteViewModel_EsActivo_RetornaFalse_CuandoEstadoEsInactivo()
        {
            var model = new ClienteViewModel { Estado = "Inactivo" };

            Assert.IsFalse(model.EsActivo, "EsActivo debe ser false cuando Estado es 'Inactivo'");
        }

        [TestMethod]
        public void ClienteViewModel_EsInvalido_SiNombreEsNulo()
        {
            var model = new ClienteViewModel
            {
                Nombre = null,
                RucDni = "12345678",
                Telefono = "987654321",
                Direccion = "Av. Lima 123"
            };

            var contexto = new ValidationContext(model);
            var errores = new List<ValidationResult>();
            bool esValido = DataValidator.TryValidateObject(model, contexto, errores, true);

            Assert.IsFalse(esValido, "El modelo debe ser inválido si el nombre es nulo");
        }

        [TestMethod]
        public void ClienteViewModel_EsInvalido_SiRucDniSuperaLongitudMaxima()
        {
            var model = new ClienteViewModel
            {
                Nombre = "Cliente Test",
                RucDni = "123456789012",
                Telefono = "987654321",
                Direccion = "Av. Lima 123"
            };

            var contexto = new ValidationContext(model);
            var errores = new List<ValidationResult>();
            bool esValido = DataValidator.TryValidateObject(model, contexto, errores, true);

            Assert.IsFalse(esValido, "RucDni no debe aceptar más de 11 caracteres");
        }

        [TestMethod]
        public void ClienteViewModel_EsValido_ConDatosCorrectos()
        {
            var model = new ClienteViewModel
            {
                Nombre = "Empresa SAC",
                RucDni = "20123456789",
                Telefono = "987654321",
                Direccion = "Av. Lima 123"
            };

            var contexto = new ValidationContext(model);
            var errores = new List<ValidationResult>();
            bool esValido = DataValidator.TryValidateObject(model, contexto, errores, true);

            Assert.IsTrue(esValido, "El modelo debe ser válido con datos correctos");
        }

        [TestMethod]
        public void ClienteViewModel_EsInvalido_SiDireccionSuperaLongitudMaxima()
        {
            var model = new ClienteViewModel
            {
                Nombre = "Empresa SAC",
                RucDni = "20123456789",
                Telefono = "987654321",
                Direccion = new string('A', 61)
            };

            var contexto = new ValidationContext(model);
            var errores = new List<ValidationResult>();
            bool esValido = DataValidator.TryValidateObject(model, contexto, errores, true);

            Assert.IsFalse(esValido, "Direccion no debe aceptar más de 60 caracteres");
        }

        [TestMethod]
        public void Integracion_sp_Cliente_Listar_SinFiltros_RetornaDatos()
        {
            var connStr = ConfigurationManager
                .ConnectionStrings["ConexionLogistica"].ConnectionString;
            var resultados = new List<object>();

            using (var conn = new SqlConnection(connStr))
            using (var cmd = new SqlCommand("sp_Cliente_Listar", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@nombre", SqlDbType.VarChar).Value = DBNull.Value;
                cmd.Parameters.Add("@estado", SqlDbType.VarChar).Value = DBNull.Value;
                conn.Open();
                using (var reader = cmd.ExecuteReader())
                    while (reader.Read())
                        resultados.Add(new object());
            }

            Assert.IsTrue(resultados.Count > 0, "Debe retornar al menos un cliente");
        }

        [TestMethod]
        public void Integracion_sp_Cliente_Listar_TieneColumnasEsperadas()
        {
            var connStr = ConfigurationManager
                .ConnectionStrings["ConexionLogistica"].ConnectionString;

            using (var conn = new SqlConnection(connStr))
            using (var cmd = new SqlCommand("sp_Cliente_Listar", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@nombre", SqlDbType.VarChar).Value = DBNull.Value;
                cmd.Parameters.Add("@estado", SqlDbType.VarChar).Value = DBNull.Value;
                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    var columnas = new List<string>();
                    for (int i = 0; i < reader.FieldCount; i++)
                        columnas.Add(reader.GetName(i));

                    Assert.IsTrue(columnas.Contains("idCliente"), "Debe contener idCliente");
                    Assert.IsTrue(columnas.Contains("nombre"), "Debe contener nombre");
                    Assert.IsTrue(columnas.Contains("rucDni"), "Debe contener rucDni");
                    Assert.IsTrue(columnas.Contains("telefono"), "Debe contener telefono");
                    Assert.IsTrue(columnas.Contains("direccion"), "Debe contener direccion");
                    Assert.IsTrue(columnas.Contains("estado"), "Debe contener estado");
                }
            }
        }

        [TestMethod]
        public void Integracion_sp_Cliente_ObtenerPorId_RetornaDatos_ConIdValido()
        {
            var connStr = ConfigurationManager
                .ConnectionStrings["ConexionLogistica"].ConnectionString;
            bool retornoDatos = false;

            using (var conn = new SqlConnection(connStr))
            using (var cmd = new SqlCommand("sp_Cliente_ObtenerPorId", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@idCliente", 1);
                conn.Open();
                using (var reader = cmd.ExecuteReader())
                    retornoDatos = reader.Read();
            }

            Assert.IsTrue(retornoDatos, "Debe retornar datos para un cliente existente");
        }

        [TestMethod]
        public void Integracion_sp_Cliente_ObtenerPorId_NoRetornaDatos_ConIdInvalido()
        {
            var connStr = ConfigurationManager
                .ConnectionStrings["ConexionLogistica"].ConnectionString;
            bool retornoDatos = false;

            using (var conn = new SqlConnection(connStr))
            using (var cmd = new SqlCommand("sp_Cliente_ObtenerPorId", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@idCliente", 99999);
                conn.Open();
                using (var reader = cmd.ExecuteReader())
                    retornoDatos = reader.Read();
            }

            Assert.IsFalse(retornoDatos, "No debe retornar datos para un ID inexistente");
        }
    }
}