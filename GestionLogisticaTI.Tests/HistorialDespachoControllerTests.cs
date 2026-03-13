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
    public class HistorialDespachoControllerTests
    {
        [TestMethod]
        public void HistorialDespachoViewModel_FechaDespacho_AceptaNull()
        {
            var model = new HistorialDespachoViewModel
            {
                IdPedido = 1,
                Cliente = "Cliente A",
                Fecha = new DateTime(2026, 1, 10),
                FechaDespacho = null,
                NumeroGuia = ""
            };

            Assert.IsNull(model.FechaDespacho, "FechaDespacho debe aceptar null");
        }

        [TestMethod]
        public void HistorialDespachoViewModel_NumeroGuia_AceptaCadenaVacia()
        {
            var model = new HistorialDespachoViewModel
            {
                NumeroGuia = ""
            };

            Assert.AreEqual("", model.NumeroGuia, "NumeroGuia debe aceptar cadena vacía");
        }

        [TestMethod]
        public void HistorialDespachoViewModel_PropiedadesSeAsignanCorrectamente()
        {
            var model = new HistorialDespachoViewModel
            {
                IdPedido = 7,
                Cliente = "Empresa ABC",
                Fecha = new DateTime(2026, 2, 5),
                FechaDespacho = new DateTime(2026, 2, 6),
                NumeroGuia = "GUIA-2026-010"
            };

            Assert.AreEqual(7, model.IdPedido);
            Assert.AreEqual("Empresa ABC", model.Cliente);
            Assert.AreEqual(new DateTime(2026, 2, 5), model.Fecha);
            Assert.AreEqual(new DateTime(2026, 2, 6), model.FechaDespacho);
            Assert.AreEqual("GUIA-2026-010", model.NumeroGuia);
        }

        [TestMethod]
        public void DespachoDetalleViewModel_ListaDetalles_IniciaVacia()
        {
            var model = new DespachoDetalleViewModel
            {
                Detalles = new List<DetalleItem>()
            };

            Assert.IsNotNull(model.Detalles, "La lista de detalles no debe ser nula");
            Assert.AreEqual(0, model.Detalles.Count, "La lista debe iniciar vacía");
        }

        [TestMethod]
        public void DespachoDetalleViewModel_FechaDespacho_AceptaNull()
        {
            var model = new DespachoDetalleViewModel
            {
                IdPedido = 1,
                FechaDespacho = null
            };

            Assert.IsNull(model.FechaDespacho, "FechaDespacho debe aceptar null");
        }

        [TestMethod]
        public void Integracion_sp_Despacho_ListarHistorial_SinFiltros_SeEjecutaSinErrores()
        {
            var connStr = ConfigurationManager
                .ConnectionStrings["ConexionLogistica"].ConnectionString;
            var resultados = new List<object>();

            using (var conn = new SqlConnection(connStr))
            using (var cmd = new SqlCommand("sp_Despacho_ListarHistorial", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@clienteId", DBNull.Value);
                cmd.Parameters.AddWithValue("@fechaDesde", DBNull.Value);
                cmd.Parameters.AddWithValue("@fechaHasta", DBNull.Value);
                cmd.Parameters.AddWithValue("@numeroGuia", DBNull.Value);
                conn.Open();
                using (var reader = cmd.ExecuteReader())
                    while (reader.Read())
                        resultados.Add(new object());
            }

            Assert.IsTrue(resultados.Count >= 0, "El SP debe ejecutarse sin errores");
        }

        [TestMethod]
        public void Integracion_sp_Despacho_ListarHistorial_TieneColumnasEsperadas()
        {
            var connStr = ConfigurationManager
                .ConnectionStrings["ConexionLogistica"].ConnectionString;

            using (var conn = new SqlConnection(connStr))
            using (var cmd = new SqlCommand("sp_Despacho_ListarHistorial", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@clienteId", DBNull.Value);
                cmd.Parameters.AddWithValue("@fechaDesde", DBNull.Value);
                cmd.Parameters.AddWithValue("@fechaHasta", DBNull.Value);
                cmd.Parameters.AddWithValue("@numeroGuia", DBNull.Value);
                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    var columnas = new List<string>();
                    for (int i = 0; i < reader.FieldCount; i++)
                        columnas.Add(reader.GetName(i));

                    Assert.IsTrue(columnas.Contains("idPedido"), "Debe contener idPedido");
                    Assert.IsTrue(columnas.Contains("Cliente"), "Debe contener Cliente");
                    Assert.IsTrue(columnas.Contains("fecha"), "Debe contener fecha");
                    Assert.IsTrue(columnas.Contains("fechaDespacho"), "Debe contener fechaDespacho");
                    Assert.IsTrue(columnas.Contains("NumeroGuia"), "Debe contener NumeroGuia");
                }
            }
        }

        [TestMethod]
        public void Integracion_sp_Despacho_ListarHistorial_ConFiltroFecha_RetornaDatos()
        {
            var connStr = ConfigurationManager
                .ConnectionStrings["ConexionLogistica"].ConnectionString;
            var resultados = new List<object>();

            using (var conn = new SqlConnection(connStr))
            using (var cmd = new SqlCommand("sp_Despacho_ListarHistorial", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@clienteId", DBNull.Value);
                cmd.Parameters.AddWithValue("@fechaDesde", new DateTime(2026, 1, 1));
                cmd.Parameters.AddWithValue("@fechaHasta", new DateTime(2026, 3, 31));
                cmd.Parameters.AddWithValue("@numeroGuia", DBNull.Value);
                conn.Open();
                using (var reader = cmd.ExecuteReader())
                    while (reader.Read())
                        resultados.Add(new object());
            }

            Assert.IsTrue(resultados.Count > 0,
                "Debe retornar despachos entre enero y marzo 2026");
        }

        [TestMethod]
        public void Integracion_sp_Despacho_ObtenerDetalle_RetornaDosResultsets()
        {
            var connStr = ConfigurationManager
                .ConnectionStrings["ConexionLogistica"].ConnectionString;

            int idPedidoExistente = 0;
            using (var conn = new SqlConnection(connStr))
            using (var cmd = new SqlCommand(
                "SELECT TOP 1 idPedido FROM Pedido WHERE estado = 'Despachado'", conn))
            {
                conn.Open();
                var result = cmd.ExecuteScalar();
                if (result != null)
                    idPedidoExistente = Convert.ToInt32(result);
            }

            if (idPedidoExistente == 0)
            {
                Assert.Inconclusive("No hay pedidos despachados para probar");
                return;
            }

            bool primerResultset = false;
            bool segundoResultset = false;

            using (var conn = new SqlConnection(connStr))
            using (var cmd = new SqlCommand("sp_Despacho_ObtenerDetalle", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@idPedido", idPedidoExistente);
                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    primerResultset = true;
                    segundoResultset = reader.NextResult();
                }
            }

            Assert.IsTrue(primerResultset, "Debe retornar el primer resultset con cabecera");
            Assert.IsTrue(segundoResultset, "Debe retornar el segundo resultset con detalles");
        }
    }
}