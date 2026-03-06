using GestionLogisticaTI.Data;
using GestionLogisticaTI.Filters;
using GestionLogisticaTI.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GestionLogisticaTI.Controllers
{
    [AuthorizeRole("Logistica")]
    public class AutorizarPedidoController : BaseController
    {
        public ActionResult Index()
        {
            List<PedidoAutorizacionViewModel> lista = new List<PedidoAutorizacionViewModel>();

            using (SqlConnection conn = ConexionBD.ObtenerConexion())
            {
                SqlCommand cmd = new SqlCommand("sp_Pedido_ListarPendientes", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                conn.Open();

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    lista.Add(new PedidoAutorizacionViewModel
                    {
                        IdPedido = Convert.ToInt32(reader["idPedido"]),
                        Fecha = Convert.ToDateTime(reader["fecha"]),
                        Cliente = reader["Cliente"].ToString(),
                        CantidadItems = Convert.ToInt32(reader["cantidadItems"])
                    });
                }
            }

            return View(lista);
        }
        public ActionResult Autorizar(int id)
        {
            try
            {
                using (SqlConnection conn = ConexionBD.ObtenerConexion())
                {
                    SqlCommand cmd = new SqlCommand("sp_Pedido_AutorizarSalida", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@idPedido", id);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }

                TempData["Mensaje"] = "Pedido autorizado correctamente.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }

            return RedirectToAction("Index");
        }

        public ActionResult Detalle(int id)
        {
            PedidoAutorizacionViewModel model = new PedidoAutorizacionViewModel();
            List<PedidoAutorizacionDetalleViewModel> detalles = new List<PedidoAutorizacionDetalleViewModel>();

            using (SqlConnection conn = ConexionBD.ObtenerConexion())
            {
                SqlCommand cmd = new SqlCommand("sp_Pedido_DetalleAutorizacion", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@idPedido", id);

                conn.Open();

                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    model.IdPedido = Convert.ToInt32(reader["idPedido"]);
                    model.Fecha = Convert.ToDateTime(reader["fecha"]);
                    model.Cliente = reader["Cliente"].ToString();
                }

                reader.NextResult();

                while (reader.Read())
                {
                    detalles.Add(new PedidoAutorizacionDetalleViewModel
                    {
                        IdProducto = Convert.ToInt32(reader["idProducto"]),
                        Producto = reader["Producto"].ToString(),
                        Cantidad = Convert.ToInt32(reader["cantidad"]),
                        StockActual = Convert.ToInt32(reader["stockActual"])
                    });
                }
            }

            ViewBag.Detalles = detalles;

            return View(model);
        }

    }
}
