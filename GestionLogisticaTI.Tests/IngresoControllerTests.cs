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
    public class IngresoControllerTests
    {
        [TestMethod]
        public void IngresoViewModel_EsInvalido_SiNumeroDocumentoEsNulo()
        {
            var model = new IngresoViewModel
            {
                NumeroDocumento = null,
                Motivo = "Compra proveedor"
            };

            var contexto = new ValidationContext(model);
            var errores = new List<ValidationResult>();
            bool esValido = DataValidator.TryValidateObject(model, contexto, errores, true);

            Assert.IsFalse(esValido, "El número de documento es requerido");
        }

        [TestMethod]
        public void IngresoViewModel_EsInvalido_SiNumeroDocumentoSuperaLongitudMaxima()
        {
            var model = new IngresoViewModel
            {
                NumeroDocumento = new string('A', 51),
                Motivo = "Compra proveedor"
            };

            var contexto = new ValidationContext(model);
            var errores = new List<ValidationResult>();
            bool esValido = DataValidator.TryValidateObject(model, contexto, errores, true);

            Assert.IsFalse(esValido, "NumeroDocumento no debe aceptar más de 50 caracteres");
        }

        [TestMethod]
        public void IngresoViewModel_EsValido_ConDatosCorrectos()
        {
            var model = new IngresoViewModel
            {
                NumeroDocumento = "FAC-2026-001",
                Motivo = "Compra de laptops"
            };

            var contexto = new ValidationContext(model);
            var errores = new List<ValidationResult>();
            bool esValido = DataValidator.TryValidateObject(model, contexto, errores, true);

            Assert.IsTrue(esValido, "El modelo debe ser válido con datos correctos");
        }

        [TestMethod]
        public void IngresoDetalleViewModel_EsInvalido_SiCantidadEsCero()
        {
            var model = new IngresoDetalleViewModel
            {
                IdProducto = 1,
                Cantidad = 0
            };

            var contexto = new ValidationContext(model);
            var errores = new List<ValidationResult>();
            bool esValido = DataValidator.TryValidateObject(model, contexto, errores, true);

            Assert.IsFalse(esValido, "Cantidad cero debe ser inválida");
        }

        [TestMethod]
        public void IngresoDetalleViewModel_EsInvalido_SiCantidadSuperaElMaximo()
        {
            var model = new IngresoDetalleViewModel
            {
                IdProducto = 1,
                Cantidad = 1001
            };

            var contexto = new ValidationContext(model);
            var errores = new List<ValidationResult>();
            bool esValido = DataValidator.TryValidateObject(model, contexto, errores, true);

            Assert.IsFalse(esValido, "Cantidad mayor a 1000 debe ser inválida");
        }

        [TestMethod]
        public void IngresoDetalleViewModel_EsValido_ConCantidadEnRango()
        {
            var model = new IngresoDetalleViewModel
            {
                IdProducto = 1,
                Cantidad = 50
            };

            var contexto = new ValidationContext(model);
            var errores = new List<ValidationResult>();
            bool esValido = DataValidator.TryValidateObject(model, contexto, errores, true);

            Assert.IsTrue(esValido, "Cantidad dentro del rango debe ser válida");
        }

        [TestMethod]
        public void IngresoViewModel_Motivo_EsOpcional()
        {
            var model = new IngresoViewModel
            {
                NumeroDocumento = "FAC-2026-001",
                Motivo = null
            };

            var contexto = new ValidationContext(model);
            var errores = new List<ValidationResult>();
            bool esValido = DataValidator.TryValidateObject(model, contexto, errores, true);

            Assert.IsTrue(esValido, "El motivo debe ser opcional");
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
        public void Integracion_sp_Producto_ListarActivos_TieneColumnasParaIngreso()
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

                    Assert.IsTrue(columnas.Contains("idProducto"), "Debe contener idProducto");
                    Assert.IsTrue(columnas.Contains("nombre"), "Debe contener nombre");
                    Assert.IsTrue(columnas.Contains("stockActual"), "Debe contener stockActual");
                    Assert.IsTrue(columnas.Contains("ubicacion"), "Debe contener ubicacion");
                }
            }
        }
    }
}