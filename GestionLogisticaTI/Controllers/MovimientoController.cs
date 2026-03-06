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
    [AuthorizeRole("Logistica", "JefeArea")]
    public class MovimientoController : BaseController
    {
        public ActionResult Historial(string tipoMovimiento, DateTime? fechaInicio, DateTime? fechaFin, string producto)
        {
            if (string.IsNullOrEmpty(tipoMovimiento))
                tipoMovimiento = null;

            if (string.IsNullOrEmpty(producto))
                producto = null;

            List<MovimientoHistorialViewModel> lista = new List<MovimientoHistorialViewModel>();

            using (SqlConnection conn = ConexionBD.ObtenerConexion())
            {
                SqlCommand cmd = new SqlCommand("sp_Movimiento_ConsultarHistorial", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@tipoMovimiento", (object)tipoMovimiento ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@fechaInicio", (object)fechaInicio ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@fechaFin", (object)fechaFin ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@producto", (object)producto ?? DBNull.Value);

                conn.Open();

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    lista.Add(new MovimientoHistorialViewModel
                    {
                        IdMovimiento = Convert.ToInt32(reader["idMovimiento"]),
                        Fecha = Convert.ToDateTime(reader["fecha"]),
                        TipoMovimiento = reader["tipoMovimiento"].ToString(),
                        NumeroDocumento = reader["numeroDocumento"].ToString(),
                        Motivo = reader["motivo"].ToString(),
                        Producto = reader["Producto"].ToString(),
                        Cantidad = Convert.ToInt32(reader["cantidad"]),
                        Usuario = reader["Usuario"].ToString()
                    });
                }
            }

            return View(lista);
        }
    }
}
