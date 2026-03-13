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
    public class StockControllerTests
    {
        [TestMethod]
        public void StockViewModel_EsCritico_RetornaTrue_CuandoEstadoEsCritico()
        {
            var model = new StockViewModel { EstadoStock = "Crítico" };

            Assert.IsTrue(model.EsCritico, "EsCritico debe ser true cuando EstadoStock es 'Crítico'");
        }

        [TestMethod]
        public void StockViewModel_EsCritico_RetornaFalse_CuandoEstadoEsNormal()
        {
            var model = new StockViewModel { EstadoStock = "Normal" };

            Assert.IsFalse(model.EsCritico, "EsCritico debe ser false cuando EstadoStock es 'Normal'");
        }

        [TestMethod]
        public void StockViewModel_EsCritico_RetornaFalse_CuandoEstadoEsBajoStock()
        {
            var model = new StockViewModel { EstadoStock = "Bajo Stock" };

            Assert.IsFalse(model.EsCritico, "EsCritico debe ser false cuando EstadoStock es 'Bajo Stock'");
        }

        [TestMethod]
        public void StockViewModel_PropiedadesSeAsignanCorrectamente()
        {
            var model = new StockViewModel
            {
                IdProducto = 1,
                Producto = "Laptop Dell",
                StockActual = 10,
                StockMinimo = 5,
                Ubicacion = "Estante A1",
                EstadoStock = "Normal"
            };

            Assert.AreEqual(1, model.IdProducto);
            Assert.AreEqual("Laptop Dell", model.Producto);
            Assert.AreEqual(10, model.StockActual);
            Assert.AreEqual(5, model.StockMinimo);
            Assert.AreEqual("Estante A1", model.Ubicacion);
            Assert.AreEqual("Normal", model.EstadoStock);
        }

        [TestMethod]
        public void StockViewModel_StockActual_MenorQueStockMinimo_IndicaStockBajo()
        {
            var model = new StockViewModel
            {
                StockActual = 2,
                StockMinimo = 5
            };

            bool stockBajo = model.StockActual < model.StockMinimo;

            Assert.IsTrue(stockBajo, "Debe detectar que el stock actual está por debajo del mínimo");
        }

        [TestMethod]
        public void StockViewModel_StockActual_MayorQueStockMinimo_IndicaStockSuficiente()
        {
            var model = new StockViewModel
            {
                StockActual = 10,
                StockMinimo = 5
            };

            bool stockSuficiente = model.StockActual >= model.StockMinimo;

            Assert.IsTrue(stockSuficiente, "Debe detectar que el stock actual es suficiente");
        }

        [TestMethod]
        public void Integracion_sp_Producto_ConsultarStock_SinFiltro_RetornaDatos()
        {
            var connStr = ConfigurationManager
                .ConnectionStrings["ConexionLogistica"].ConnectionString;
            var resultados = new List<object>();

            using (var conn = new SqlConnection(connStr))
            using (var cmd = new SqlCommand("sp_Producto_ConsultarStock", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@nombreProducto", SqlDbType.VarChar).Value = DBNull.Value;
                conn.Open();
                using (var reader = cmd.ExecuteReader())
                    while (reader.Read())
                        resultados.Add(new object());
            }

            Assert.IsTrue(resultados.Count > 0, "Debe retornar al menos un producto");
        }

        [TestMethod]
        public void Integracion_sp_Producto_ConsultarStock_TieneColumnasEsperadas()
        {
            var connStr = ConfigurationManager
                .ConnectionStrings["ConexionLogistica"].ConnectionString;

            using (var conn = new SqlConnection(connStr))
            using (var cmd = new SqlCommand("sp_Producto_ConsultarStock", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@nombreProducto", SqlDbType.VarChar).Value = DBNull.Value;
                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    var columnas = new List<string>();
                    for (int i = 0; i < reader.FieldCount; i++)
                        columnas.Add(reader.GetName(i));

                    Assert.IsTrue(columnas.Contains("idProducto"), "Debe contener idProducto");
                    Assert.IsTrue(columnas.Contains("nombre"), "Debe contener nombre");
                    Assert.IsTrue(columnas.Contains("stockActual"), "Debe contener stockActual");
                    Assert.IsTrue(columnas.Contains("stockMinimo"), "Debe contener stockMinimo");
                    Assert.IsTrue(columnas.Contains("ubicacion"), "Debe contener ubicacion");
                    Assert.IsTrue(columnas.Contains("estadoStock"), "Debe contener estadoStock");
                }
            }
        }

        [TestMethod]
        public void Integracion_sp_Producto_ConsultarStock_ConFiltroNombre_RetornaDatos()
        {
            var connStr = ConfigurationManager
                .ConnectionStrings["ConexionLogistica"].ConnectionString;

            string nombreExistente = null;
            using (var conn = new SqlConnection(connStr))
            using (var cmd = new SqlCommand("sp_Producto_ConsultarStock", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@nombreProducto", SqlDbType.VarChar).Value = DBNull.Value;
                conn.Open();
                using (var reader = cmd.ExecuteReader())
                    if (reader.Read())
                        nombreExistente = reader["nombre"].ToString().Substring(0, 3);
            }

            if (string.IsNullOrEmpty(nombreExistente))
            {
                Assert.Inconclusive("No hay productos para filtrar");
                return;
            }

            var resultados = new List<object>();
            using (var conn = new SqlConnection(connStr))
            using (var cmd = new SqlCommand("sp_Producto_ConsultarStock", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@nombreProducto", nombreExistente);
                conn.Open();
                using (var reader = cmd.ExecuteReader())
                    while (reader.Read())
                        resultados.Add(new object());
            }

            Assert.IsTrue(resultados.Count > 0,
                "Debe retornar productos que coincidan con el filtro de nombre");
        }
    }
}