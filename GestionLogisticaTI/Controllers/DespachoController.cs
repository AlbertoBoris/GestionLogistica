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
using static GestionLogisticaTI.ViewModels.PedidoDespachoDetalleViewModel;

namespace GestionLogisticaTI.Controllers
{
    [AuthorizeRole("Almacen")]
    public class DespachoController : BaseController
    {
        public ActionResult Index()
        {
            List<PedidoDespachoViewModel> lista = new List<PedidoDespachoViewModel>();

            using (SqlConnection conn = ConexionBD.ObtenerConexion())
            {
                SqlCommand cmd = new SqlCommand("sp_Pedido_ListarAutorizados", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    lista.Add(new PedidoDespachoViewModel
                    {
                        IdPedido = Convert.ToInt32(reader["idPedido"]),
                        Fecha = Convert.ToDateTime(reader["fecha"]),
                        Cliente = reader["Cliente"].ToString(),
                        CantidadItems = Convert.ToInt32(reader["CantidadItems"])
                    });
                }
            }

            return View(lista);
        }
        [HttpPost]
        public ActionResult Confirmar(int idPedido, string numeroDocumento)
        {
            int idUsuario = Convert.ToInt32(Session["IdUsuario"]);

            using (SqlConnection conn = ConexionBD.ObtenerConexion())
            {
                SqlCommand cmd = new SqlCommand("sp_Pedido_ConfirmarDespacho", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@idPedido", idPedido);
                cmd.Parameters.AddWithValue("@idUsuario", idUsuario);
                cmd.Parameters.AddWithValue("@numeroDocumento", numeroDocumento);

                conn.Open();

                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    if (reader["Mensaje"] != DBNull.Value)
                        TempData["Mensaje"] = reader["Mensaje"].ToString();
                    else if (reader["ErrorMensaje"] != DBNull.Value)
                        TempData["Error"] = reader["ErrorMensaje"].ToString();
                }
            }

            return RedirectToAction("Index");
        }
        public ActionResult Detalle(int id)
        {
            PedidoDespachoDetalleViewModel model =
                new PedidoDespachoDetalleViewModel();

            model.Detalles = new List<DetalleItem>();

            using (SqlConnection conn = ConexionBD.ObtenerConexion())
            {
                SqlCommand cmd = new SqlCommand(
                    "sp_Pedido_ObtenerDetalleDespacho", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@idPedido", id);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    if (model.IdPedido == 0)
                    {
                        model.IdPedido = Convert.ToInt32(reader["idPedido"]);
                        model.Fecha = Convert.ToDateTime(reader["fecha"]);
                        model.Cliente = reader["Cliente"].ToString();

                        if (reader["fechaDespacho"] != DBNull.Value)
                            model.FechaDespacho = Convert.ToDateTime(reader["fechaDespacho"]);
                    }

                    model.Detalles.Add(new DetalleItem
                    {
                        Producto = reader["Producto"].ToString(),
                        Cantidad = Convert.ToInt32(reader["cantidad"])
                    });
                }
            }

            return View(model);
        }
    }
}
