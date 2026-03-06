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
    [AuthorizeRole("Ventas", "Logistica", "Almacen", "JefeArea")]
    public class StockController : BaseController
    {
        public ActionResult Index(string nombreProducto)
        {
            List<StockViewModel> lista = new List<StockViewModel>();

            using (SqlConnection conn = ConexionBD.ObtenerConexion())
            {
                SqlCommand cmd = new SqlCommand("sp_Producto_ConsultarStock", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                if (!string.IsNullOrEmpty(nombreProducto))
                    cmd.Parameters.AddWithValue("@nombreProducto", nombreProducto);
                else
                    cmd.Parameters.Add("@nombreProducto", SqlDbType.VarChar).Value = DBNull.Value;

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    lista.Add(new StockViewModel
                    {
                        IdProducto = Convert.ToInt32(reader["idProducto"]),
                        Producto = reader["nombre"].ToString(),
                        StockActual = Convert.ToInt32(reader["stockActual"]),
                        StockMinimo = Convert.ToInt32(reader["stockMinimo"]),
                        Ubicacion = reader["ubicacion"].ToString(),
                        EstadoStock = reader["estadoStock"].ToString()
                    });
                }
            }

            ViewBag.FiltroNombre = nombreProducto;

            return View(lista);
        }
    }
}
