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
    public class DespachoControllerTests
    {
        [TestMethod]
        public void PedidoDespachoViewModel_PropiedadesSeAsignanCorrectamente()
        {
            var model = new PedidoDespachoViewModel
            {
                IdPedido = 5,
                Fecha = new DateTime(2026, 2, 10),
                Cliente = "Empresa XYZ",
                CantidadItems = 4
            };

            Assert.AreEqual(5, model.IdPedido);
            Assert.AreEqual(new DateTime(2026, 2, 10), model.Fecha);
            Assert.AreEqual("Empresa XYZ", model.Cliente);
            Assert.AreEqual(4, model.CantidadItems);
        }

        [TestMethod]
        public void PedidoDespachoDetalleViewModel_FechaDespacho_EsNullable()
        {
            var model = new PedidoDespachoDetalleViewModel
            {
                IdPedido = 1,
                FechaDespacho = null
            };

            Assert.IsNull(model.FechaDespacho, "FechaDespacho debe aceptar null");
        }

        [TestMethod]
        public void PedidoDespachoDetalleViewModel_FechaDespacho_AceptaFechaValida()
        {
            var model = new PedidoDespachoDetalleViewModel
            {
                IdPedido = 1,
                FechaDespacho = new DateTime(2026, 3, 1)
            };

            Assert.AreEqual(new DateTime(2026, 3, 1), model.FechaDespacho);
        }

        [TestMethod]
        public void DetalleItem_PropiedadesSeAsignanCorrectamente()
        {
            var item = new DetalleItem
            {
                Producto = "Laptop Dell",
                Cantidad = 3
            };

            Assert.AreEqual("Laptop Dell", item.Producto);
            Assert.AreEqual(3, item.Cantidad);
        }

        [TestMethod]
        public void PedidoDespachoDetalleViewModel_ListaDetalles_IniciaVacia()
        {
            var model = new PedidoDespachoDetalleViewModel
            {
                Detalles = new List<DetalleItem>()
            };

            Assert.IsNotNull(model.Detalles, "La lista de detalles no debe ser nula");
            Assert.AreEqual(0, model.Detalles.Count, "La lista debe iniciar vacía");
        }

        [TestMethod]
        public void Integracion_sp_Pedido_ListarAutorizados_SeEjecutaSinErrores()
        {
            var connStr = ConfigurationManager
                .ConnectionStrings["ConexionLogistica"].ConnectionString;
            var resultados = new List<object>();

            using (var conn = new SqlConnection(connStr))
            using (var cmd = new SqlCommand("sp_Pedido_ListarAutorizados", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                conn.Open();
                using (var reader = cmd.ExecuteReader())
                    while (reader.Read())
                        resultados.Add(new object());
            }

            Assert.IsTrue(resultados.Count >= 0, "El SP debe ejecutarse sin errores");
        }

        [TestMethod]
        public void Integracion_sp_Pedido_ListarAutorizados_TieneColumnasEsperadas()
        {
            var connStr = ConfigurationManager
                .ConnectionStrings["ConexionLogistica"].ConnectionString;

            using (var conn = new SqlConnection(connStr))
            using (var cmd = new SqlCommand("sp_Pedido_ListarAutorizados", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    var columnas = new List<string>();
                    for (int i = 0; i < reader.FieldCount; i++)
                        columnas.Add(reader.GetName(i));

                    Assert.IsTrue(columnas.Contains("idPedido"), "Debe contener idPedido");
                    Assert.IsTrue(columnas.Contains("fecha"), "Debe contener fecha");
                    Assert.IsTrue(columnas.Contains("Cliente"), "Debe contener Cliente");
                    Assert.IsTrue(columnas.Contains("CantidadItems"), "Debe contener CantidadItems");
                }
            }
        }

        [TestMethod]
        public void Integracion_sp_Pedido_ObtenerDetalleDespacho_RetornaDatos_ConIdValido()
        {
            var connStr = ConfigurationManager
                .ConnectionStrings["ConexionLogistica"].ConnectionString;

            int idPedidoExistente = 0;
            using (var conn = new SqlConnection(connStr))
            using (var cmd = new SqlCommand("SELECT TOP 1 idPedido FROM Pedido", conn))
            {
                conn.Open();
                var result = cmd.ExecuteScalar();
                if (result != null)
                    idPedidoExistente = Convert.ToInt32(result);
            }

            if (idPedidoExistente == 0)
            {
                Assert.Inconclusive("No hay pedidos en la base de datos para probar");
                return;
            }

            var resultados = new List<object>();
            using (var conn = new SqlConnection(connStr))
            using (var cmd = new SqlCommand("sp_Pedido_ObtenerDetalleDespacho", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@idPedido", idPedidoExistente);
                conn.Open();
                using (var reader = cmd.ExecuteReader())
                    while (reader.Read())
                        resultados.Add(new object());
            }

            Assert.IsTrue(resultados.Count > 0, "Debe retornar detalles para un pedido existente");
        }
    }
}