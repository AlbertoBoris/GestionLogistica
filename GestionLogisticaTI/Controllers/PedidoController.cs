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
    [AuthorizeRole("Ventas")]
    public class PedidoController : BaseController
    {
        /* Listar */
        public ActionResult Index(int? idCliente, string estado)
        {
            List<PedidoListViewModel> lista = new List<PedidoListViewModel>();

            using (SqlConnection conn = ConexionBD.ObtenerConexion())
            {
                SqlCommand cmd = new SqlCommand("sp_Pedido_Listar", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                if (!string.IsNullOrEmpty(estado))
                    cmd.Parameters.AddWithValue("@estado", estado);
                else
                    cmd.Parameters.Add("@estado", SqlDbType.VarChar).Value = DBNull.Value;
                if (idCliente.HasValue)
                    cmd.Parameters.AddWithValue("@idCliente", idCliente.Value);
                else
                    cmd.Parameters.Add("@idCliente", SqlDbType.Int).Value = DBNull.Value;
                cmd.Parameters.AddWithValue("@fechaInicio", DBNull.Value);
                cmd.Parameters.AddWithValue("@fechaFin", DBNull.Value);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    lista.Add(new PedidoListViewModel
                    {
                        IdPedido = Convert.ToInt32(reader["idPedido"]),
                        Fecha = Convert.ToDateTime(reader["fecha"]),
                        Cliente = reader["Cliente"].ToString(),
                        Estado = reader["estado"].ToString(),
                        CantidadItems = Convert.ToInt32(reader["CantidadItems"])
                    });
                }
            }

            ViewBag.Clientes = ObtenerClientes();
            ViewBag.EstadoSeleccionado = estado;
            ViewBag.ClienteSeleccionado = idCliente;

            return View(lista);
        }
        public ActionResult Create()    
        {
            PedidoCreateViewModel model = new PedidoCreateViewModel();

            model.Clientes = ObtenerClientes();
            model.Productos = ObtenerProductos();
            model.Detalles = new List<PedidoDetalleViewModel>();

            return View(model);
        }



        private List<SelectListItem> ObtenerProductos()
        {
            List<SelectListItem> lista = new List<SelectListItem>();

            using (SqlConnection conn = ConexionBD.ObtenerConexion())
            {
                SqlCommand cmd = new SqlCommand("sp_Producto_Listar", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    lista.Add(new SelectListItem
                    {
                        Value = reader["idProducto"].ToString(),
                        Text = reader["nombre"].ToString()
                    });
                }
            }

            return lista;
        }
        private List<SelectListItem> ObtenerClientes()
        {
            List<SelectListItem> lista = new List<SelectListItem>();

            using (SqlConnection conn = ConexionBD.ObtenerConexion())
            {
                SqlCommand cmd = new SqlCommand("sp_Cliente_Listar", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    lista.Add(new SelectListItem
                    {
                        Value = reader["idCliente"].ToString(),
                        Text = reader["nombre"].ToString()
                    });
                }
            }

            return lista;
        }

        /* Registrar Pedido */
        [HttpPost]
        public ActionResult Create(PedidoCreateViewModel model)
        {
            using (SqlConnection conn = ConexionBD.ObtenerConexion())
            {
                SqlCommand cmd = new SqlCommand("sp_Pedido_Insertar", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@idCliente", model.IdCliente);

                // Crear DataTable para TVP
                DataTable dt = new DataTable();
                dt.Columns.Add("idProducto", typeof(int));
                dt.Columns.Add("cantidad", typeof(int));

                if (model.Detalles != null)
                {
                    foreach (var item in model.Detalles)
                    {
                        dt.Rows.Add(item.IdProducto, item.Cantidad);
                    }
                }

                SqlParameter tvp = cmd.Parameters.AddWithValue("@Detalles", dt);
                tvp.SqlDbType = SqlDbType.Structured;
                tvp.TypeName = "Tipo_PedidoDetalle";

                conn.Open();
                cmd.ExecuteNonQuery();
            }

            return RedirectToAction("Index");
        }
        public ActionResult Edit(int id)
        {
            PedidoCreateViewModel model = new PedidoCreateViewModel();
            model.Detalles = new List<PedidoDetalleViewModel>();

            using (SqlConnection conn = ConexionBD.ObtenerConexion())
            {
                SqlCommand cmd = new SqlCommand("sp_Pedido_ObtenerDetalle", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@idPedido", id);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                // PRIMER RESULTSET → Cabecera
                if (reader.Read())
                {
                    model.IdPedido = Convert.ToInt32(reader["idPedido"]);
                    model.IdCliente = Convert.ToInt32(reader["idCliente"]);
                }

                // SEGUNDO RESULTSET → Detalle
                reader.NextResult();

                while (reader.Read())
                {
                    model.Detalles.Add(new PedidoDetalleViewModel
                    {
                        IdProducto = Convert.ToInt32(reader["idProducto"]),
                        Producto = reader["Producto"].ToString(),
                        Cantidad = Convert.ToInt32(reader["cantidad"])
                    });
                }
            }

            model.Clientes = ObtenerClientes();
            model.Productos = ObtenerProductos();

            return View(model);
        }

        /* Editar Pedido */
        [HttpPost]
        public ActionResult Edit(PedidoCreateViewModel model)
        {
            try
            {
                using (SqlConnection conn = ConexionBD.ObtenerConexion())
                {
                    SqlCommand cmd = new SqlCommand("sp_Pedido_Actualizar", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@idPedido", model.IdPedido);
                    cmd.Parameters.AddWithValue("@idCliente", model.IdCliente);

                    // Crear DataTable para TVP
                    DataTable dt = new DataTable();
                    dt.Columns.Add("idProducto", typeof(int));
                    dt.Columns.Add("cantidad", typeof(int));

                    if (model.Detalles != null)
                    {
                        foreach (var item in model.Detalles)
                        {
                            dt.Rows.Add(item.IdProducto, item.Cantidad);
                        }
                    }

                    SqlParameter tvp = cmd.Parameters.AddWithValue("@Detalles", dt);
                    tvp.SqlDbType = SqlDbType.Structured;
                    tvp.TypeName = "Tipo_PedidoDetalle";

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);

                model.Clientes = ObtenerClientes();
                model.Productos = ObtenerProductos();

                return View(model);
            }
        }

        /* Cancelar Pedido */
        public ActionResult Cancelar(int id)
        {
            using (SqlConnection conn = ConexionBD.ObtenerConexion())
            {
                SqlCommand cmd = new SqlCommand("sp_Pedido_Cancelar", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@idPedido", id);

                conn.Open();
                cmd.ExecuteNonQuery();
            }

            return RedirectToAction("Index");
        }

        /* Detalle */
        public ActionResult Detalle(int id)
        {
            PedidoCreateViewModel model = new PedidoCreateViewModel();
            model.Detalles = new List<PedidoDetalleViewModel>();

            using (SqlConnection conn = ConexionBD.ObtenerConexion())
            {
                SqlCommand cmd = new SqlCommand("sp_Pedido_ObtenerDetalle", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@idPedido", id);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                // CABECERA
                if (reader.Read())
                {
                    model.IdPedido = Convert.ToInt32(reader["idPedido"]);
                    model.IdCliente = Convert.ToInt32(reader["idCliente"]);

                    ViewBag.Fecha = Convert.ToDateTime(reader["fecha"]);
                    ViewBag.Estado = reader["estado"].ToString();
                    ViewBag.Cliente = reader["Cliente"].ToString();

                    if (reader["fechaDespacho"] != DBNull.Value)
                        model.FechaDespacho = Convert.ToDateTime(reader["fechaDespacho"]);
                    else
                        model.FechaDespacho = null;
                }

                // DETALLE
                reader.NextResult();

                while (reader.Read())
                {
                    model.Detalles.Add(new PedidoDetalleViewModel
                    {
                        IdProducto = Convert.ToInt32(reader["idProducto"]),
                        Producto = reader["Producto"].ToString(),
                        Cantidad = Convert.ToInt32(reader["cantidad"])
                    });
                }
            }

            return View(model);
        }
    }
}
