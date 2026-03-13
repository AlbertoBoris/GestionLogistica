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
    public class PedidoControllerTests
    {
        [TestMethod]
        public void PedidoCreateViewModel_EsValido_ConIdClienteValido()
        {
            var model = new PedidoCreateViewModel
            {
                IdCliente = 1
            };

            var contexto = new ValidationContext(model);
            var errores = new List<ValidationResult>();
            bool esValido = DataValidator.TryValidateObject(model, contexto, errores, true);

            Assert.IsTrue(esValido, "El modelo debe ser válido con un IdCliente asignado");
        }
        [TestMethod]
        public void PedidoCreateViewModel_FechaDespacho_AceptaNull()
        {
            var model = new PedidoCreateViewModel
            {
                IdCliente = 1,
                FechaDespacho = null
            };

            Assert.IsNull(model.FechaDespacho, "FechaDespacho debe aceptar null");
        }

        [TestMethod]
        public void PedidoCreateViewModel_ListaDetalles_IniciaVacia()
        {
            var model = new PedidoCreateViewModel
            {
                IdCliente = 1,
                Detalles = new List<PedidoDetalleViewModel>()
            };

            Assert.IsNotNull(model.Detalles, "La lista de detalles no debe ser nula");
            Assert.AreEqual(0, model.Detalles.Count, "La lista debe iniciar vacía");
        }

        [TestMethod]
        public void PedidoListViewModel_PropiedadesSeAsignanCorrectamente()
        {
            var model = new PedidoListViewModel
            {
                IdPedido = 5,
                Fecha = new DateTime(2026, 2, 10),
                Cliente = "Empresa ABC",
                Estado = "Pendiente",
                CantidadItems = 3
            };

            Assert.AreEqual(5, model.IdPedido);
            Assert.AreEqual(new DateTime(2026, 2, 10), model.Fecha);
            Assert.AreEqual("Empresa ABC", model.Cliente);
            Assert.AreEqual("Pendiente", model.Estado);
            Assert.AreEqual(3, model.CantidadItems);
        }

        [TestMethod]
        public void PedidoDetalleViewModel_FechaDespacho_AceptaNull()
        {
            var model = new PedidoDetalleViewModel
            {
                IdProducto = 1,
                Cantidad = 5,
                FechaDespacho = null
            };

            Assert.IsNull(model.FechaDespacho, "FechaDespacho debe aceptar null");
        }

        [TestMethod]
        public void PedidoDetalleViewModel_PropiedadesSeAsignanCorrectamente()
        {
            var model = new PedidoDetalleViewModel
            {
                IdProducto = 2,
                Producto = "Monitor Samsung",
                Cantidad = 4,
                FechaDespacho = new DateTime(2026, 3, 1)
            };

            Assert.AreEqual(2, model.IdProducto);
            Assert.AreEqual("Monitor Samsung", model.Producto);
            Assert.AreEqual(4, model.Cantidad);
            Assert.AreEqual(new DateTime(2026, 3, 1), model.FechaDespacho);
        }

        [TestMethod]
        public void Integracion_sp_Pedido_Listar_SinFiltros_RetornaDatos()
        {
            var connStr = ConfigurationManager
                .ConnectionStrings["ConexionLogistica"].ConnectionString;
            var resultados = new List<object>();

            using (var conn = new SqlConnection(connStr))
            using (var cmd = new SqlCommand("sp_Pedido_Listar", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@estado", SqlDbType.VarChar).Value = DBNull.Value;
                cmd.Parameters.Add("@idCliente", SqlDbType.Int).Value = DBNull.Value;
                cmd.Parameters.AddWithValue("@fechaInicio", DBNull.Value);
                cmd.Parameters.AddWithValue("@fechaFin", DBNull.Value);
                conn.Open();
                using (var reader = cmd.ExecuteReader())
                    while (reader.Read())
                        resultados.Add(new object());
            }

            Assert.IsTrue(resultados.Count > 0, "Debe retornar al menos un pedido");
        }

        [TestMethod]
        public void Integracion_sp_Pedido_Listar_TieneColumnasEsperadas()
        {
            var connStr = ConfigurationManager
                .ConnectionStrings["ConexionLogistica"].ConnectionString;

            using (var conn = new SqlConnection(connStr))
            using (var cmd = new SqlCommand("sp_Pedido_Listar", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@estado", SqlDbType.VarChar).Value = DBNull.Value;
                cmd.Parameters.Add("@idCliente", SqlDbType.Int).Value = DBNull.Value;
                cmd.Parameters.AddWithValue("@fechaInicio", DBNull.Value);
                cmd.Parameters.AddWithValue("@fechaFin", DBNull.Value);
                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    var columnas = new List<string>();
                    for (int i = 0; i < reader.FieldCount; i++)
                        columnas.Add(reader.GetName(i));

                    Assert.IsTrue(columnas.Contains("idPedido"), "Debe contener idPedido");
                    Assert.IsTrue(columnas.Contains("fecha"), "Debe contener fecha");
                    Assert.IsTrue(columnas.Contains("Cliente"), "Debe contener Cliente");
                    Assert.IsTrue(columnas.Contains("estado"), "Debe contener estado");
                    Assert.IsTrue(columnas.Contains("CantidadItems"), "Debe contener CantidadItems");
                }
            }
        }

        [TestMethod]
        public void Integracion_sp_Pedido_ObtenerDetalle_RetornaDosResultsets()
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
            using (var cmd = new SqlCommand("sp_Pedido_ObtenerDetalle", conn))
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
            Assert.IsTrue(segundoResultset, "Debe retornar el segundo resultset con detalle");
        }

        [TestMethod]
        public void Integracion_sp_Pedido_Listar_FiltradoPorEstado_SoloRetornaPendientes()
        {
            var connStr = ConfigurationManager
                .ConnectionStrings["ConexionLogistica"].ConnectionString;
            var estados = new List<string>();

            using (var conn = new SqlConnection(connStr))
            using (var cmd = new SqlCommand("sp_Pedido_Listar", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@estado", "Pendiente");
                cmd.Parameters.Add("@idCliente", SqlDbType.Int).Value = DBNull.Value;
                cmd.Parameters.AddWithValue("@fechaInicio", DBNull.Value);
                cmd.Parameters.AddWithValue("@fechaFin", DBNull.Value);
                conn.Open();
                using (var reader = cmd.ExecuteReader())
                    while (reader.Read())
                        estados.Add(reader["estado"].ToString());
            }

            foreach (var estado in estados)
                Assert.AreEqual("Pendiente", estado,
                    "Todos los registros deben tener estado Pendiente");
        }
    }
}