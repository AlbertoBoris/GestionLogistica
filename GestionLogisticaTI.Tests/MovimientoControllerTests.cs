using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GestionLogisticaTI.ViewModels;

namespace GestionLogisticaTI.Tests
{
    [TestClass]
    public class MovimientoControllerTests
    {
        [TestMethod]
        public void MovimientoHistorialViewModel_PropiedadesSeAsignanCorrectamente()
        {
            var model = new MovimientoHistorialViewModel
            {
                IdMovimiento = 10,
                Fecha = new DateTime(2026, 2, 1),
                TipoMovimiento = "Ingreso",
                NumeroDocumento = "FAC-2026-001",
                Motivo = "Compra proveedor",
                Producto = "Laptop Dell",
                Cantidad = 5,
                Usuario = "Juan Perez"
            };

            Assert.AreEqual(10, model.IdMovimiento);
            Assert.AreEqual(new DateTime(2026, 2, 1), model.Fecha);
            Assert.AreEqual("Ingreso", model.TipoMovimiento);
            Assert.AreEqual("FAC-2026-001", model.NumeroDocumento);
            Assert.AreEqual("Compra proveedor", model.Motivo);
            Assert.AreEqual("Laptop Dell", model.Producto);
            Assert.AreEqual(5, model.Cantidad);
            Assert.AreEqual("Juan Perez", model.Usuario);
        }

        [TestMethod]
        public void MovimientoHistorialViewModel_TipoMovimiento_AceptaIngreso()
        {
            var model = new MovimientoHistorialViewModel { TipoMovimiento = "Ingreso" };
            Assert.AreEqual("Ingreso", model.TipoMovimiento);
        }

        [TestMethod]
        public void MovimientoHistorialViewModel_TipoMovimiento_AceptaSalida()
        {
            var model = new MovimientoHistorialViewModel { TipoMovimiento = "Salida" };
            Assert.AreEqual("Salida", model.TipoMovimiento);
        }

        [TestMethod]
        public void MovimientoHistorialViewModel_TipoMovimiento_AceptaAjuste()
        {
            var model = new MovimientoHistorialViewModel { TipoMovimiento = "Ajuste" };
            Assert.AreEqual("Ajuste", model.TipoMovimiento);
        }

        [TestMethod]
        public void Integracion_sp_Movimiento_ConsultarHistorial_SinFiltros_RetornaDatos()
        {
            var connStr = ConfigurationManager
                .ConnectionStrings["ConexionLogistica"].ConnectionString;
            var resultados = new List<object>();

            using (var conn = new SqlConnection(connStr))
            using (var cmd = new SqlCommand("sp_Movimiento_ConsultarHistorial", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@tipoMovimiento", DBNull.Value);
                cmd.Parameters.AddWithValue("@fechaInicio", DBNull.Value);
                cmd.Parameters.AddWithValue("@fechaFin", DBNull.Value);
                cmd.Parameters.AddWithValue("@producto", DBNull.Value);
                conn.Open();
                using (var reader = cmd.ExecuteReader())
                    while (reader.Read())
                        resultados.Add(new object());
            }

            Assert.IsTrue(resultados.Count > 0, "Debe retornar al menos un movimiento");
        }

        [TestMethod]
        public void Integracion_sp_Movimiento_ConsultarHistorial_TieneColumnasEsperadas()
        {
            var connStr = ConfigurationManager
                .ConnectionStrings["ConexionLogistica"].ConnectionString;

            using (var conn = new SqlConnection(connStr))
            using (var cmd = new SqlCommand("sp_Movimiento_ConsultarHistorial", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@tipoMovimiento", DBNull.Value);
                cmd.Parameters.AddWithValue("@fechaInicio", DBNull.Value);
                cmd.Parameters.AddWithValue("@fechaFin", DBNull.Value);
                cmd.Parameters.AddWithValue("@producto", DBNull.Value);
                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    var columnas = new List<string>();
                    for (int i = 0; i < reader.FieldCount; i++)
                        columnas.Add(reader.GetName(i));

                    Assert.IsTrue(columnas.Contains("idMovimiento"), "Debe contener idMovimiento");
                    Assert.IsTrue(columnas.Contains("fecha"), "Debe contener fecha");
                    Assert.IsTrue(columnas.Contains("tipoMovimiento"), "Debe contener tipoMovimiento");
                    Assert.IsTrue(columnas.Contains("numeroDocumento"), "Debe contener numeroDocumento");
                    Assert.IsTrue(columnas.Contains("Producto"), "Debe contener Producto");
                    Assert.IsTrue(columnas.Contains("cantidad"), "Debe contener cantidad");
                    Assert.IsTrue(columnas.Contains("Usuario"), "Debe contener Usuario");
                }
            }
        }

        [TestMethod]
        public void Integracion_sp_Movimiento_ConsultarHistorial_FiltradoPorTipo_Ingreso()
        {
            var connStr = ConfigurationManager
                .ConnectionStrings["ConexionLogistica"].ConnectionString;
            var resultados = new List<string>();

            using (var conn = new SqlConnection(connStr))
            using (var cmd = new SqlCommand("sp_Movimiento_ConsultarHistorial", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@tipoMovimiento", "Ingreso");
                cmd.Parameters.AddWithValue("@fechaInicio", DBNull.Value);
                cmd.Parameters.AddWithValue("@fechaFin", DBNull.Value);
                cmd.Parameters.AddWithValue("@producto", DBNull.Value);
                conn.Open();
                using (var reader = cmd.ExecuteReader())
                    while (reader.Read())
                        resultados.Add(reader["tipoMovimiento"].ToString());
            }

            Assert.IsTrue(resultados.Count > 0, "Debe retornar movimientos de tipo Ingreso");
            foreach (var tipo in resultados)
                Assert.AreEqual("Ingreso", tipo, "Todos los registros deben ser de tipo Ingreso");
        }

        [TestMethod]
        public void Integracion_sp_Movimiento_ConsultarHistorial_FiltradoPorFecha_RetornaDatos()
        {
            var connStr = ConfigurationManager
                .ConnectionStrings["ConexionLogistica"].ConnectionString;
            var resultados = new List<object>();

            using (var conn = new SqlConnection(connStr))
            using (var cmd = new SqlCommand("sp_Movimiento_ConsultarHistorial", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@tipoMovimiento", DBNull.Value);
                cmd.Parameters.AddWithValue("@fechaInicio", new DateTime(2026, 1, 1));
                cmd.Parameters.AddWithValue("@fechaFin", new DateTime(2026, 3, 31));
                cmd.Parameters.AddWithValue("@producto", DBNull.Value);
                conn.Open();
                using (var reader = cmd.ExecuteReader())
                    while (reader.Read())
                        resultados.Add(new object());
            }

            Assert.IsTrue(resultados.Count > 0,
                "Debe retornar movimientos entre enero y marzo 2026");
        }
    }
}