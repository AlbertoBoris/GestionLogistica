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
    public class ProductoController : BaseController
    {
        public ActionResult Index(string nombre, string estado)
        {
            if (string.IsNullOrEmpty(nombre))
                nombre = null;

            if (string.IsNullOrEmpty(estado))
                estado = null;

            List<ProductoViewModel> lista = new List<ProductoViewModel>();

            using (SqlConnection conn = ConexionBD.ObtenerConexion())
            {
                SqlCommand cmd = new SqlCommand("sp_Producto_Listar", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@nombre", (object)nombre ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@estado", (object)estado ?? DBNull.Value);

                conn.Open();

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    lista.Add(new ProductoViewModel
                    {
                        IdProducto = Convert.ToInt32(reader["idProducto"]),
                        Nombre = reader["nombre"].ToString(),
                        Descripcion = reader["descripcion"].ToString(),
                        StockActual = Convert.ToInt32(reader["stockActual"]),
                        StockMinimo = Convert.ToInt32(reader["stockMinimo"]),
                        Ubicacion = reader["ubicacion"].ToString(),
                        Estado = reader["estado"].ToString()
                    });
                }
            }

            return View(lista);
        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(ProductoViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            using (SqlConnection conn = ConexionBD.ObtenerConexion())
            {
                SqlCommand cmd = new SqlCommand("sp_Producto_Insertar", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@nombre", model.Nombre);
                cmd.Parameters.AddWithValue("@descripcion", model.Descripcion);
                cmd.Parameters.AddWithValue("@stockMinimo", model.StockMinimo);
                cmd.Parameters.AddWithValue("@ubicacion", model.Ubicacion);

                conn.Open();
                cmd.ExecuteNonQuery();
            }

            return RedirectToAction("Index");
        }
        public ActionResult Edit(int id)
        {
            ProductoViewModel model = new ProductoViewModel();

            using (SqlConnection conn = ConexionBD.ObtenerConexion())
            {
                SqlCommand cmd = new SqlCommand("sp_Producto_ObtenerPorId", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@idProducto", id);

                conn.Open();

                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    model.IdProducto = Convert.ToInt32(reader["idProducto"]);
                    model.Nombre = reader["nombre"].ToString();
                    model.Descripcion = reader["descripcion"].ToString();
                    model.StockMinimo = Convert.ToInt32(reader["stockMinimo"]);
                    model.Ubicacion = reader["ubicacion"].ToString();
                }
            }

            return View(model);
        }
        [HttpPost]
        public ActionResult Edit(ProductoViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            using (SqlConnection conn = ConexionBD.ObtenerConexion())
            {
                SqlCommand cmd = new SqlCommand("sp_Producto_Actualizar", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@idProducto", model.IdProducto);
                cmd.Parameters.AddWithValue("@nombre", model.Nombre);
                cmd.Parameters.AddWithValue("@descripcion", model.Descripcion);
                cmd.Parameters.AddWithValue("@stockMinimo", model.StockMinimo);
                cmd.Parameters.AddWithValue("@ubicacion", model.Ubicacion);

                conn.Open();
                cmd.ExecuteNonQuery();
            }

            return RedirectToAction("Index");
        }
        public ActionResult CambiarEstado(int id)
        {
            using (SqlConnection conn = ConexionBD.ObtenerConexion())
            {
                SqlCommand cmd = new SqlCommand("sp_Producto_CambiarEstado", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@idProducto", id);

                conn.Open();
                cmd.ExecuteNonQuery();
            }

            return RedirectToAction("Index");
        }

    }
}
