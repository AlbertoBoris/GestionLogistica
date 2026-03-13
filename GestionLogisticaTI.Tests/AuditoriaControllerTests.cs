using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.Mvc;
using GestionLogisticaTI.Controllers;
using GestionLogisticaTI.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GestionLogisticaTI.Tests
{
    [TestClass]
    public class AuditoriaControllerTests
    {
        [TestMethod]
        public void AuditoriaViewModel_StockFisico_EsNullable()
        {
            var model = new AuditoriaViewModel
            {
                IdProducto = 1,
                Nombre = "Laptop Dell",
                StockActual = 10,
                StockMinimo = 5,
                Ubicacion = "A1",
                StockFisico = null
            };

            Assert.IsNull(model.StockFisico, "StockFisico debe aceptar null como valor inicial");
        }

        [TestMethod]
        public void AuditoriaViewModel_StockFisico_AceptaValorPositivo()
        {
            var model = new AuditoriaViewModel
            {
                IdProducto = 1,
                StockFisico = 15
            };

            Assert.AreEqual(15, model.StockFisico, "StockFisico debe aceptar valores positivos");
        }

        [TestMethod]
        public void AuditoriaViewModel_StockFisico_AceptaValorCero()
        {
            var model = new AuditoriaViewModel
            {
                IdProducto = 1,
                StockFisico = 0
            };

            Assert.AreEqual(0, model.StockFisico, "StockFisico debe aceptar cero");
        }

        [TestMethod]
        public void Integracion_sp_Auditoria_ListarProductos_SinFiltro_RetornaDatos()
        {
            var connStr = ConfigurationManager
                .ConnectionStrings["ConexionLogistica"].ConnectionString;
            var resultados = new List<object>();

            using (var conn = new SqlConnection(connStr))
            using (var cmd = new SqlCommand("sp_Auditoria_ListarProductos", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ubicacion", SqlDbType.VarChar).Value = DBNull.Value;
                conn.Open();
                using (var reader = cmd.ExecuteReader())
                    while (reader.Read())
                        resultados.Add(new object());
            }

            Assert.IsTrue(resultados.Count > 0,
                "Sin filtro debe retornar todos los productos");
        }

        [TestMethod]
        public void Integracion_sp_Auditoria_ListarProductos_TieneColumnasEsperadas()
        {
            var connStr = ConfigurationManager
                .ConnectionStrings["ConexionLogistica"].ConnectionString;

            using (var conn = new SqlConnection(connStr))
            using (var cmd = new SqlCommand("sp_Auditoria_ListarProductos", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ubicacion", SqlDbType.VarChar).Value = DBNull.Value;
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
                }
            }
        }

        [TestMethod]
        public void Integracion_sp_Auditoria_ListarProductos_ConFiltroUbicacion_RetornaDatos()
        {
            var connStr = ConfigurationManager
                .ConnectionStrings["ConexionLogistica"].ConnectionString;

            string ubicacionExistente = null;

            using (var conn = new SqlConnection(connStr))
            using (var cmd = new SqlCommand("sp_Auditoria_ListarProductos", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ubicacion", SqlDbType.VarChar).Value = DBNull.Value;
                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                        ubicacionExistente = reader["ubicacion"].ToString();
                }
            }

            if (string.IsNullOrEmpty(ubicacionExistente))
            {
                Assert.Inconclusive("No hay productos con ubicación para filtrar");
                return;
            }

            var resultados = new List<object>();
            using (var conn = new SqlConnection(connStr))
            using (var cmd = new SqlCommand("sp_Auditoria_ListarProductos", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ubicacion", ubicacionExistente);
                conn.Open();
                using (var reader = cmd.ExecuteReader())
                    while (reader.Read())
                        resultados.Add(new object());
            }

            Assert.IsTrue(resultados.Count > 0,
                "Debe retornar productos para la ubicación filtrada");
        }

        [TestMethod]
        public void Integracion_sp_Auditoria_ListarProductos_UbicacionInexistente_RetornaVacio()
        {
            var connStr = ConfigurationManager
                .ConnectionStrings["ConexionLogistica"].ConnectionString;
            var resultados = new List<object>();

            using (var conn = new SqlConnection(connStr))
            using (var cmd = new SqlCommand("sp_Auditoria_ListarProductos", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ubicacion", "UBICACION_QUE_NO_EXISTE_XYZ");
                conn.Open();
                using (var reader = cmd.ExecuteReader())
                    while (reader.Read())
                        resultados.Add(new object());
            }

            Assert.AreEqual(0, resultados.Count,
                "Una ubicación inexistente debe retornar lista vacía");
        }
    }
}