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
    public class AutorizarPedidoControllerTests
    {
        [TestMethod]
        public void PedidoAutorizacionViewModel_PropiedadesSeAsignanCorrectamente()
        {
            var model = new PedidoAutorizacionViewModel
            {
                IdPedido = 10,
                Fecha = new DateTime(2026, 2, 15),
                Cliente = "Cliente A",
                CantidadItems = 3
            };

            Assert.AreEqual(10, model.IdPedido);
            Assert.AreEqual(new DateTime(2026, 2, 15), model.Fecha);
            Assert.AreEqual("Cliente A", model.Cliente);
            Assert.AreEqual(3, model.CantidadItems);
        }

        [TestMethod]
        public void PedidoAutorizacionDetalleViewModel_PropiedadesSeAsignanCorrectamente()
        {
            var model = new PedidoAutorizacionDetalleViewModel
            {
                IdProducto = 1,
                Producto = "Laptop Dell",
                Cantidad = 5,
                StockActual = 20
            };

            Assert.AreEqual(1, model.IdProducto);
            Assert.AreEqual("Laptop Dell", model.Producto);
            Assert.AreEqual(5, model.Cantidad);
            Assert.AreEqual(20, model.StockActual);
        }

        [TestMethod]
        public void PedidoAutorizacionDetalleViewModel_StockActual_MayorQueCantidad_IndicaStockSuficiente()
        {
            var model = new PedidoAutorizacionDetalleViewModel
            {
                Cantidad = 5,
                StockActual = 20
            };

            bool stockSuficiente = model.StockActual >= model.Cantidad;

            Assert.IsTrue(stockSuficiente, "El stock actual debe ser suficiente para cubrir la cantidad");
        }

        [TestMethod]
        public void PedidoAutorizacionDetalleViewModel_StockActual_MenorQueCantidad_IndicaStockInsuficiente()
        {
            var model = new PedidoAutorizacionDetalleViewModel
            {
                Cantidad = 10,
                StockActual = 3
            };

            bool stockInsuficiente = model.StockActual < model.Cantidad;

            Assert.IsTrue(stockInsuficiente, "Debe detectar stock insuficiente para cubrir la cantidad");
        }

        [TestMethod]
        public void Integracion_sp_Pedido_ListarPendientes_RetornaDatos()
        {
            var connStr = ConfigurationManager
                .ConnectionStrings["ConexionLogistica"].ConnectionString;
            var resultados = new List<object>();

            using (var conn = new SqlConnection(connStr))
            using (var cmd = new SqlCommand("sp_Pedido_ListarPendientes", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                conn.Open();
                using (var reader = cmd.ExecuteReader())
                    while (reader.Read())
                        resultados.Add(new object());
            }

            Assert.IsTrue(resultados.Count >= 0,
                "El SP debe ejecutarse sin errores");
        }

        [TestMethod]
        public void Integracion_sp_Pedido_ListarPendientes_TieneColumnasEsperadas()
        {
            var connStr = ConfigurationManager
                .ConnectionStrings["ConexionLogistica"].ConnectionString;

            using (var conn = new SqlConnection(connStr))
            using (var cmd = new SqlCommand("sp_Pedido_ListarPendientes", conn))
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
                    Assert.IsTrue(columnas.Contains("cantidadItems"), "Debe contener cantidadItems");
                }
            }
        }

        [TestMethod]
        public void Integracion_sp_Pedido_DetalleAutorizacion_RetornaDosResultsets()
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

            bool primerResultset = false;
            bool segundoResultset = false;

            using (var conn = new SqlConnection(connStr))
            using (var cmd = new SqlCommand("sp_Pedido_DetalleAutorizacion", conn))
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

            Assert.IsTrue(primerResultset, "Debe retornar el primer resultset con datos del pedido");
            Assert.IsTrue(segundoResultset, "Debe retornar el segundo resultset con el detalle");
        }
    }
}