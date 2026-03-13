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
    public class IngresoController : BaseController
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
        public ActionResult RegistrarIngreso()
        {
            IngresoViewModel model = new IngresoViewModel();

            model.Productos = ObtenerProductosActivos();

            model.Detalles = new List<IngresoDetalleViewModel>();

            return View(model);
        }
        [HttpPost]
        public ActionResult RegistrarIngreso(IngresoViewModel model)
        {
            if (string.IsNullOrEmpty(model.NumeroDocumento))
            {
                TempData["Error"] = "El número de documento es requerido.";
                return View(model);
            }
            using (SqlConnection conn = ConexionBD.ObtenerConexion())
            {
                SqlCommand cmd = new SqlCommand("sp_Movimiento_InsertarIngreso", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@numeroDocumento", model.NumeroDocumento);
                cmd.Parameters.AddWithValue("@motivo", (object)model.Motivo ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@idUsuario", Session["IdUsuario"]);

                DataTable tabla = new DataTable();
                tabla.Columns.Add("idProducto", typeof(int));
                tabla.Columns.Add("cantidad", typeof(int));

                foreach (var item in model.Detalles)
                {
                    if (item.Cantidad > 0)
                        tabla.Rows.Add(item.IdProducto, item.Cantidad);
                }

                SqlParameter param = cmd.Parameters.AddWithValue("@detalles", tabla);
                param.SqlDbType = SqlDbType.Structured;
                param.TypeName = "Tipo_MovimientoDetalle";

                conn.Open();
                cmd.ExecuteNonQuery();
            }

            TempData["Mensaje"] = "Ingreso de mercadería registrado correctamente.";

            return RedirectToAction("RegistrarIngreso");
        }
    }
}
