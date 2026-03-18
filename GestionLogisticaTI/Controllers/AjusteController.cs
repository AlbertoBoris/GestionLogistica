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
    public class AjusteController : BaseController
    {
        private List<ProductoViewModel> ObtenerProductosActivos()
        {
            List<ProductoViewModel> lista = new List<ProductoViewModel>();

            using (SqlConnection conn = ConexionBD.ObtenerConexion())
            {
                SqlCommand cmd = new SqlCommand("sp_Producto_ListarActivos", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                conn.Open();

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    lista.Add(new ProductoViewModel
                    {
                        IdProducto = Convert.ToInt32(reader["idProducto"]),
                        Nombre = reader["nombre"].ToString(),
                        StockActual = Convert.ToInt32(reader["stockActual"]),
                        Ubicacion = reader["ubicacion"].ToString()
                    });
                }
            }

            return lista;
        }
        public ActionResult Registrar()
        {
            AjusteInventarioViewModel model = new AjusteInventarioViewModel();

            model.Productos = ObtenerProductosActivos();

            model.Detalles = new List<AjusteDetalleViewModel>();

            return View(model);
        }
        [HttpPost]
        public ActionResult Registrar(AjusteInventarioViewModel model)
        {

            using (SqlConnection conn = ConexionBD.ObtenerConexion())
            {
                SqlCommand cmd = new SqlCommand("sp_Movimiento_InsertarAjuste", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@numeroDocumento", model.NumeroDocumento);
                cmd.Parameters.AddWithValue("@motivo", model.Motivo);
                cmd.Parameters.AddWithValue("@idUsuario", Session["IdUsuario"]);

                DataTable tabla = new DataTable();
                tabla.Columns.Add("idProducto", typeof(int));
                tabla.Columns.Add("cantidad", typeof(int));

                foreach (var item in model.Detalles)
                {
                    var stockActual = ObtenerStockDesdeBD(item.IdProducto);

                    if (stockActual + item.CantidadAjuste < 0)
                    {
                        ModelState.AddModelError("", $"El producto no puede quedar con stock negativo.");
                        return View(model);
                    }
                    tabla.Rows.Add(item.IdProducto, item.CantidadAjuste);
                }

                SqlParameter param = cmd.Parameters.AddWithValue("@detalles", tabla);
                param.SqlDbType = SqlDbType.Structured;
                param.TypeName = "Tipo_AjusteDetalle";

                conn.Open();
                cmd.ExecuteNonQuery();
            }

            TempData["Mensaje"] = "Ajuste de inventario registrado correctamente.";

            return RedirectToAction("Registrar");
        }
        public JsonResult ObtenerStock(int idProducto)
        {
            int stock = 0;

            using (SqlConnection conn = ConexionBD.ObtenerConexion())
            {
                SqlCommand cmd = new SqlCommand("sp_Producto_ObtenerStock", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@idProducto", idProducto);

                conn.Open();

                var result = cmd.ExecuteScalar();

                if (result != null)
                    stock = Convert.ToInt32(result);
            }

            return Json(stock, JsonRequestBehavior.AllowGet);
        }

        private int ObtenerStockDesdeBD(int idProducto)
        {
            int stock = 0;

            using (SqlConnection conn = ConexionBD.ObtenerConexion())
            {
                SqlCommand cmd = new SqlCommand("sp_Producto_ObtenerStock", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@idProducto", idProducto);

                conn.Open();
                var result = cmd.ExecuteScalar();

                if (result != null)
                {
                    stock = Convert.ToInt32(result);
                }
            }

            return stock;
        }
    }
}
