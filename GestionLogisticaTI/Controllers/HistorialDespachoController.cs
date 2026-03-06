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
    [AuthorizeRole("Logistica", "Almacen")]
    public class HistorialDespachoController : BaseController
    {
        public ActionResult Index(int? clienteId, DateTime? fechaDesde, DateTime? fechaHasta, string numeroGuia)
        {
            List<HistorialDespachoViewModel> lista = new List<HistorialDespachoViewModel>();

            using (SqlConnection conn = ConexionBD.ObtenerConexion())
            {
                SqlCommand cmd = new SqlCommand("sp_Despacho_ListarHistorial", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@clienteId", (object)clienteId ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@fechaDesde", (object)fechaDesde ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@fechaHasta", (object)fechaHasta ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@numeroGuia", string.IsNullOrEmpty(numeroGuia) ? (object)DBNull.Value : numeroGuia);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    lista.Add(new HistorialDespachoViewModel
                    {
                        IdPedido = Convert.ToInt32(reader["idPedido"]),
                        Cliente = reader["Cliente"].ToString(),
                        Fecha = Convert.ToDateTime(reader["fecha"]),
                        FechaDespacho = reader["fechaDespacho"] != DBNull.Value
                            ? Convert.ToDateTime(reader["fechaDespacho"])
                            : (DateTime?)null,
                        NumeroGuia = reader["NumeroGuia"] != DBNull.Value
                            ? reader["NumeroGuia"].ToString()
                            : ""
                    });
                }
            }

            ViewBag.ClienteId = clienteId;
            ViewBag.FechaDesde = fechaDesde;
            ViewBag.FechaHasta = fechaHasta;
            ViewBag.NumeroGuia = numeroGuia;

            return View(lista);
        }

        public ActionResult Detalle(int id)
        {
            DespachoDetalleViewModel model = new DespachoDetalleViewModel();
            model.Detalles = new List<DetalleItem>();

            using (SqlConnection conn = ConexionBD.ObtenerConexion())
            {
                SqlCommand cmd = new SqlCommand("sp_Despacho_ObtenerDetalle", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@idPedido", id);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    model.IdPedido = Convert.ToInt32(reader["idPedido"]);
                    model.Fecha = Convert.ToDateTime(reader["fecha"]);
                    model.Cliente = reader["Cliente"].ToString();
                    model.FechaDespacho = reader["fechaDespacho"] != DBNull.Value
                        ? Convert.ToDateTime(reader["fechaDespacho"])
                        : (DateTime?)null;
                    model.NumeroGuia = reader["NumeroGuia"].ToString();
                }

                reader.NextResult();

                while (reader.Read())
                {
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
