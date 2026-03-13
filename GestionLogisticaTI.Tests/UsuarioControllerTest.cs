using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GestionLogisticaTI.ViewModels;
using GestionLogisticaTI.Helpers;
using DataValidator = System.ComponentModel.DataAnnotations.Validator;

namespace GestionLogisticaTI.Tests
{
    [TestClass]
    public class UsuarioControllerTests
    {
        [TestMethod]
        public void UsuarioViewModel_EsInvalido_SiNombreEsNulo()
        {
            var model = new UsuarioViewModel
            {
                Nombre = null,
                Correo = "test@empresa.com",
                NombreRol = "Logistica"
            };

            var contexto = new ValidationContext(model);
            var errores = new List<ValidationResult>();
            bool esValido = DataValidator.TryValidateObject(model, contexto, errores, true);

            Assert.IsFalse(esValido, "El nombre es obligatorio");
        }

        [TestMethod]
        public void UsuarioViewModel_EsInvalido_SiCorreoEsInvalido()
        {
            var model = new UsuarioViewModel
            {
                Nombre = "Juan Perez",
                Correo = "correo-sin-arroba",
                NombreRol = "Logistica"
            };

            var contexto = new ValidationContext(model);
            var errores = new List<ValidationResult>();
            bool esValido = DataValidator.TryValidateObject(model, contexto, errores, true);

            Assert.IsFalse(esValido, "Un correo sin formato válido debe ser inválido");
        }

        [TestMethod]
        public void UsuarioViewModel_EsInvalido_SiPasswordMenorA6Caracteres()
        {
            var model = new UsuarioViewModel
            {
                Nombre = "Juan Perez",
                Correo = "juan@empresa.com",
                NombreRol = "Logistica",
                Password = "123"
            };

            var contexto = new ValidationContext(model);
            var errores = new List<ValidationResult>();
            bool esValido = DataValidator.TryValidateObject(model, contexto, errores, true);

            Assert.IsFalse(esValido, "Password menor a 6 caracteres debe ser inválido");
        }

        [TestMethod]
        public void UsuarioViewModel_EsValido_SiPasswordEsNulo()
        {
            var model = new UsuarioViewModel
            {
                Nombre = "Juan Perez",
                Correo = "juan@empresa.com",
                NombreRol = "Logistica",
                Password = null
            };

            var contexto = new ValidationContext(model);
            var errores = new List<ValidationResult>();
            bool esValido = DataValidator.TryValidateObject(model, contexto, errores, true);

            Assert.IsTrue(esValido, "Password nulo debe ser válido para edición sin cambio de contraseña");
        }

        [TestMethod]
        public void UsuarioViewModel_EsValido_ConDatosCorrectos()
        {
            var model = new UsuarioViewModel
            {
                Nombre = "Juan Perez",
                Correo = "juan@empresa.com",
                NombreRol = "Logistica",
                Password = "123456"
            };

            var contexto = new ValidationContext(model);
            var errores = new List<ValidationResult>();
            bool esValido = DataValidator.TryValidateObject(model, contexto, errores, true);

            Assert.IsTrue(esValido, "El modelo debe ser válido con datos correctos");
        }

        [TestMethod]
        public void PasswordHelper_HashPassword_ProduceHashConsistente()
        {
            string hash1 = PasswordHelper.HashPassword("123456");
            string hash2 = PasswordHelper.HashPassword("123456");

            Assert.AreEqual(hash1, hash2, "El mismo password debe producir siempre el mismo hash");
        }

        [TestMethod]
        public void PasswordHelper_HashPassword_ProduceHashDiferente_ParaPasswordDistinto()
        {
            string hash1 = PasswordHelper.HashPassword("123456");
            string hash2 = PasswordHelper.HashPassword("654321");

            Assert.AreNotEqual(hash1, hash2, "Passwords distintos deben producir hashes distintos");
        }

        [TestMethod]
        public void Integracion_sp_Usuario_Listar_RetornaDatos()
        {
            var connStr = ConfigurationManager
                .ConnectionStrings["ConexionLogistica"].ConnectionString;
            var resultados = new List<object>();

            using (var conn = new SqlConnection(connStr))
            using (var cmd = new SqlCommand("sp_Usuario_Listar", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                conn.Open();
                using (var reader = cmd.ExecuteReader())
                    while (reader.Read())
                        resultados.Add(new object());
            }

            Assert.IsTrue(resultados.Count > 0, "Debe retornar al menos un usuario");
        }

        [TestMethod]
        public void Integracion_sp_Usuario_Listar_TieneColumnasEsperadas()
        {
            var connStr = ConfigurationManager
                .ConnectionStrings["ConexionLogistica"].ConnectionString;

            using (var conn = new SqlConnection(connStr))
            using (var cmd = new SqlCommand("sp_Usuario_Listar", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    var columnas = new List<string>();
                    for (int i = 0; i < reader.FieldCount; i++)
                        columnas.Add(reader.GetName(i));

                    Assert.IsTrue(columnas.Contains("idUsuario"), "Debe contener idUsuario");
                    Assert.IsTrue(columnas.Contains("nombre"), "Debe contener nombre");
                    Assert.IsTrue(columnas.Contains("correo"), "Debe contener correo");
                    Assert.IsTrue(columnas.Contains("estado"), "Debe contener estado");
                    Assert.IsTrue(columnas.Contains("idRol"), "Debe contener idRol");
                    Assert.IsTrue(columnas.Contains("nombreRol"), "Debe contener nombreRol");
                }
            }
        }

        [TestMethod]
        public void Integracion_sp_Usuario_ObtenerPorId_RetornaDatos_ConIdValido()
        {
            var connStr = ConfigurationManager
                .ConnectionStrings["ConexionLogistica"].ConnectionString;
            bool retornoDatos = false;

            using (var conn = new SqlConnection(connStr))
            using (var cmd = new SqlCommand("sp_Usuario_ObtenerPorId", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@idUsuario", 1);
                conn.Open();
                using (var reader = cmd.ExecuteReader())
                    retornoDatos = reader.Read();
            }

            Assert.IsTrue(retornoDatos, "Debe retornar datos para un usuario existente");
        }

        [TestMethod]
        public void Integracion_sp_Usuario_ObtenerPorId_NoRetornaDatos_ConIdInvalido()
        {
            var connStr = ConfigurationManager
                .ConnectionStrings["ConexionLogistica"].ConnectionString;
            bool retornoDatos = false;

            using (var conn = new SqlConnection(connStr))
            using (var cmd = new SqlCommand("sp_Usuario_ObtenerPorId", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@idUsuario", 99999);
                conn.Open();
                using (var reader = cmd.ExecuteReader())
                    retornoDatos = reader.Read();
            }

            Assert.IsFalse(retornoDatos, "No debe retornar datos para un ID inexistente");
        }
    }
}