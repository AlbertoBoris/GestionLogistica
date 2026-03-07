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
    [AuthorizeRole("Almacen")]
    public class AuditoriaController : BaseController
    {
        /* Listar */
        public ActionResult Index(string ubicacion)
        {
            List<AuditoriaViewModel> lista = new List<AuditoriaViewModel>();

            using (SqlConnection conn = ConexionBD.ObtenerConexion())
            {
                SqlCommand cmd = new SqlCommand("sp_Auditoria_ListarProductos", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                if (!string.IsNullOrEmpty(ubicacion))
                    cmd.Parameters.AddWithValue("@ubicacion", ubicacion);
                else
                    cmd.Parameters.Add("@ubicacion", SqlDbType.VarChar).Value = DBNull.Value;

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    lista.Add(new AuditoriaViewModel
                    {
                        IdProducto = Convert.ToInt32(reader["idProducto"]),
                        Nombre = reader["nombre"].ToString(),
                        StockActual = Convert.ToInt32(reader["stockActual"]),
                        StockMinimo = Convert.ToInt32(reader["stockMinimo"]),
                        Ubicacion = reader["ubicacion"].ToString(),
                        StockFisico = 0 // valor inicial
                    });
                }
            }

            ViewBag.Ubicacion = ubicacion;
            return View(lista);
        }
        /* Registrar Auditoria */
        [HttpPost]
        public ActionResult Aplicar(List<AuditoriaViewModel> model)
        {
            int idUsuario = Convert.ToInt32(Session["IdUsuario"]);

            DataTable dt = new DataTable();
            dt.Columns.Add("idProducto", typeof(int));
            dt.Columns.Add("stockFisico", typeof(int));

            if (model == null)
            {
                TempData["Error"] = "No se recibieron datos de auditoría.";
                return RedirectToAction("Index");
            }

            foreach (var item in model.Where(x => x != null))
            {
                if (item.StockFisico != null) // solo si ingresó valor
                {
                    dt.Rows.Add(item.IdProducto, item.StockFisico);
                }
            }

            using (SqlConnection conn = ConexionBD.ObtenerConexion())
            {
                SqlCommand cmd = new SqlCommand("sp_Auditoria_AplicarAjustes", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@idUsuario", idUsuario);

                SqlParameter param = cmd.Parameters.AddWithValue("@Detalles", dt);
                param.SqlDbType = SqlDbType.Structured;
                param.TypeName = "Tipo_AuditoriaDetalle";

                conn.Open();
                cmd.ExecuteNonQuery();
            }

            TempData["Mensaje"] = "Auditoría aplicada correctamente.";
            return RedirectToAction("Index");
        }
    }

}
