using GestionLogisticaTI.Controllers;
using GestionLogisticaTI.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.Mvc;
using System.Web.WebPages;
using DataValidator = System.ComponentModel.DataAnnotations.Validator;

namespace GestionLogisticaTI.Tests
{
    [TestClass]
    public class AjusteControllerTests
    {
        [TestMethod]
        public void AjusteDetalleViewModel_EsInvalido_SiCantidadEstaFueraDeRango()
        {
            var model = new AjusteDetalleViewModel
            {
                IdProducto = 1,
                CantidadAjuste = 9999
            };

            var contexto = new ValidationContext(model);
            var errores = new List<ValidationResult>();
            bool esValido = DataValidator.TryValidateObject(model, contexto, errores, true);

            Assert.IsFalse(esValido, "Cantidad fuera de rango debe ser inválida");
        }

        [TestMethod]
        public void AjusteDetalleViewModel_EsValido_ConCantidadNegativaEnRango()
        {
            var model = new AjusteDetalleViewModel
            {
                IdProducto = 1,
                CantidadAjuste = -5
            };

            var contexto = new ValidationContext(model);
            var errores = new List<ValidationResult>();
            bool esValido = DataValidator.TryValidateObject(model, contexto, errores, true);

            Assert.IsTrue(esValido, "Cantidad negativa dentro del rango debe ser válida");
        }

        [TestMethod]
        public void AjusteInventarioViewModel_EsInvalido_SiMotivoEsNulo()
        {
            var model = new AjusteInventarioViewModel
            {
                NumeroDocumento = "AJ-2026-001",
                Motivo = null
            };

            var contexto = new ValidationContext(model);
            var errores = new List<ValidationResult>();
            bool esValido = DataValidator.TryValidateObject(model, contexto, errores, true);

            Assert.IsFalse(esValido, "El motivo es requerido");
        }

        [TestMethod]
        public void AjusteInventarioViewModel_EsValido_ConDatosCorrectos()
        {
            var model = new AjusteInventarioViewModel
            {
                NumeroDocumento = "AJ-2026-001",
                Motivo = "Ajuste por conteo físico"
            };

            var contexto = new ValidationContext(model);
            var errores = new List<ValidationResult>();
            bool esValido = DataValidator.TryValidateObject(model, contexto, errores, true);

            Assert.IsTrue(esValido, "El modelo debe ser válido con datos correctos");
        }

        [TestMethod]
        public void Integracion_sp_Producto_ListarActivos_RetornaProductos()
        {
            var connStr = ConfigurationManager
                .ConnectionStrings["ConexionLogistica"].ConnectionString;
            var resultados = new List<object>();

            using (var conn = new SqlConnection(connStr))
            using (var cmd = new SqlCommand("sp_Producto_ListarActivos", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                conn.Open();
                using (var reader = cmd.ExecuteReader())
                    while (reader.Read())
                        resultados.Add(new object());
            }

            Assert.IsTrue(resultados.Count > 0,
                "Debe retornar al menos un producto activo");
        }

        [TestMethod]
        public void Integracion_sp_Producto_ObtenerStock_RetornaStockValido()
        {
            var connStr = ConfigurationManager
                .ConnectionStrings["ConexionLogistica"].ConnectionString;
            object resultado = null;

            using (var conn = new SqlConnection(connStr))
            using (var cmd = new SqlCommand("sp_Producto_ObtenerStock", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@idProducto", 1);
                conn.Open();
                resultado = cmd.ExecuteScalar();
            }

            Assert.IsNotNull(resultado, "Debe retornar un valor de stock");
            Assert.IsTrue(Convert.ToInt32(resultado) >= 0,
                "El stock debe ser un número mayor o igual a cero");
        }

        [TestMethod]
        public void Integracion_sp_Producto_ObtenerStock_ProductoInexistente_RetornaNull()
        {
            var connStr = ConfigurationManager
                .ConnectionStrings["ConexionLogistica"].ConnectionString;
            object resultado = null;

            using (var conn = new SqlConnection(connStr))
            using (var cmd = new SqlCommand("sp_Producto_ObtenerStock", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@idProducto", 99999);
                conn.Open();
                resultado = cmd.ExecuteScalar();
            }

            Assert.IsNull(resultado,
                "Un producto inexistente debe retornar null");
        }

        [TestMethod]
        public void Integracion_sp_Producto_ListarActivos_TieneColumnasEsperadas()
        {
            var connStr = ConfigurationManager
                .ConnectionStrings["ConexionLogistica"].ConnectionString;

            using (var conn = new SqlConnection(connStr))
            using (var cmd = new SqlCommand("sp_Producto_ListarActivos", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    var columnas = new List<string>();
                    for (int i = 0; i < reader.FieldCount; i++)
                        columnas.Add(reader.GetName(i));

                    Assert.IsTrue(columnas.Contains("idProducto"),
                        "Debe contener columna idProducto");
                    Assert.IsTrue(columnas.Contains("nombre"),
                        "Debe contener columna nombre");
                    Assert.IsTrue(columnas.Contains("stockActual"),
                        "Debe contener columna stockActual");
                    Assert.IsTrue(columnas.Contains("ubicacion"),
                        "Debe contener columna ubicacion");
                }
            }
        }
    }
}