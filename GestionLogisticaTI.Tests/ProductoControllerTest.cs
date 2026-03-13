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
    public class ProductoControllerTests
    {
        [TestMethod]
        public void ProductoViewModel_EsInvalido_SiNombreEsNulo()
        {
            var model = new ProductoViewModel
            {
                Nombre = null,
                StockMinimo = 5,
                Ubicacion = "A1"
            };

            var contexto = new ValidationContext(model);
            var errores = new List<ValidationResult>();
            bool esValido = DataValidator.TryValidateObject(model, contexto, errores, true);

            Assert.IsFalse(esValido, "El nombre es obligatorio");
        }

        [TestMethod]
        public void ProductoViewModel_EsInvalido_SiNombreSuperaLongitudMaxima()
        {
            var model = new ProductoViewModel
            {
                Nombre = new string('A', 101),
                StockMinimo = 5,
                Ubicacion = "A1"
            };

            var contexto = new ValidationContext(model);
            var errores = new List<ValidationResult>();
            bool esValido = DataValidator.TryValidateObject(model, contexto, errores, true);

            Assert.IsFalse(esValido, "Nombre no debe aceptar más de 100 caracteres");
        }

        [TestMethod]
        public void ProductoViewModel_EsInvalido_SiStockMinimoEsCero()
        {
            var model = new ProductoViewModel
            {
                Nombre = "Laptop Dell",
                StockMinimo = 0,
                Ubicacion = "A1"
            };

            var contexto = new ValidationContext(model);
            var errores = new List<ValidationResult>();
            bool esValido = DataValidator.TryValidateObject(model, contexto, errores, true);

            Assert.IsFalse(esValido, "StockMinimo 0 debe ser inválido");
        }

        [TestMethod]
        public void ProductoViewModel_EsInvalido_SiUbicacionEsNula()
        {
            var model = new ProductoViewModel
            {
                Nombre = "Laptop Dell",
                StockMinimo = 5,
                Ubicacion = null
            };

            var contexto = new ValidationContext(model);
            var errores = new List<ValidationResult>();
            bool esValido = DataValidator.TryValidateObject(model, contexto, errores, true);

            Assert.IsFalse(esValido, "La ubicación es obligatoria");
        }

        [TestMethod]
        public void ProductoViewModel_EsValido_ConDatosCorrectos()
        {
            var model = new ProductoViewModel
            {
                Nombre = "Laptop Dell Latitude 5420",
                Descripcion = "Laptop empresarial",
                StockMinimo = 5,
                Ubicacion = "Estante A1"
            };

            var contexto = new ValidationContext(model);
            var errores = new List<ValidationResult>();
            bool esValido = DataValidator.TryValidateObject(model, contexto, errores, true);

            Assert.IsTrue(esValido, "El modelo debe ser válido con datos correctos");
        }

        [TestMethod]
        public void ProductoViewModel_Descripcion_EsOpcional()
        {
            var model = new ProductoViewModel
            {
                Nombre = "Laptop Dell",
                Descripcion = null,
                StockMinimo = 5,
                Ubicacion = "A1"
            };

            var contexto = new ValidationContext(model);
            var errores = new List<ValidationResult>();
            bool esValido = DataValidator.TryValidateObject(model, contexto, errores, true);

            Assert.IsTrue(esValido, "La descripción debe ser opcional");
        }

        [TestMethod]
        public void Integracion_sp_Producto_Listar_SinFiltros_RetornaDatos()
        {
            var connStr = ConfigurationManager
                .ConnectionStrings["ConexionLogistica"].ConnectionString;
            var resultados = new List<object>();

            using (var conn = new SqlConnection(connStr))
            using (var cmd = new SqlCommand("sp_Producto_Listar", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@nombre", DBNull.Value);
                cmd.Parameters.AddWithValue("@estado", DBNull.Value);
                conn.Open();
                using (var reader = cmd.ExecuteReader())
                    while (reader.Read())
                        resultados.Add(new object());
            }

            Assert.IsTrue(resultados.Count > 0, "Debe retornar al menos un producto");
        }

        [TestMethod]
        public void Integracion_sp_Producto_Listar_TieneColumnasEsperadas()
        {
            var connStr = ConfigurationManager
                .ConnectionStrings["ConexionLogistica"].ConnectionString;

            using (var conn = new SqlConnection(connStr))
            using (var cmd = new SqlCommand("sp_Producto_Listar", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@nombre", DBNull.Value);
                cmd.Parameters.AddWithValue("@estado", DBNull.Value);
                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    var columnas = new List<string>();
                    for (int i = 0; i < reader.FieldCount; i++)
                        columnas.Add(reader.GetName(i));

                    Assert.IsTrue(columnas.Contains("idProducto"), "Debe contener idProducto");
                    Assert.IsTrue(columnas.Contains("nombre"), "Debe contener nombre");
                    Assert.IsTrue(columnas.Contains("descripcion"), "Debe contener descripcion");
                    Assert.IsTrue(columnas.Contains("stockActual"), "Debe contener stockActual");
                    Assert.IsTrue(columnas.Contains("stockMinimo"), "Debe contener stockMinimo");
                    Assert.IsTrue(columnas.Contains("ubicacion"), "Debe contener ubicacion");
                    Assert.IsTrue(columnas.Contains("estado"), "Debe contener estado");
                }
            }
        }

        [TestMethod]
        public void Integracion_sp_Producto_ObtenerPorId_RetornaDatos_ConIdValido()
        {
            var connStr = ConfigurationManager
                .ConnectionStrings["ConexionLogistica"].ConnectionString;
            bool retornoDatos = false;

            using (var conn = new SqlConnection(connStr))
            using (var cmd = new SqlCommand("sp_Producto_ObtenerPorId", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@idProducto", 1);
                conn.Open();
                using (var reader = cmd.ExecuteReader())
                    retornoDatos = reader.Read();
            }

            Assert.IsTrue(retornoDatos, "Debe retornar datos para un producto existente");
        }

        [TestMethod]
        public void Integracion_sp_Producto_ObtenerPorId_NoRetornaDatos_ConIdInvalido()
        {
            var connStr = ConfigurationManager
                .ConnectionStrings["ConexionLogistica"].ConnectionString;
            bool retornoDatos = false;

            using (var conn = new SqlConnection(connStr))
            using (var cmd = new SqlCommand("sp_Producto_ObtenerPorId", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@idProducto", 99999);
                conn.Open();
                using (var reader = cmd.ExecuteReader())
                    retornoDatos = reader.Read();
            }

            Assert.IsFalse(retornoDatos, "No debe retornar datos para un ID inexistente");
        }
    }
}