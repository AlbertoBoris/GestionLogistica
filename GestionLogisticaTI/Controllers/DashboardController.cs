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
    [AuthorizeRole("JefeArea")]
    public class DashboardController : BaseController
    {
        public ActionResult Index()
        {
            var model = new DashboardViewModel();

            using (SqlConnection cn = ConexionBD.ObtenerConexion())
            {
                cn.Open();

                // KPIs
                using (SqlCommand cmd = new SqlCommand("sp_Dashboard_KPIs", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    var dr = cmd.ExecuteReader();

                    if (dr.Read())
                    {
                        model.KPIs = new KPIViewModel
                        {
                            TotalProductos = Convert.ToInt32(dr["TotalProductos"]),
                            TotalStock = Convert.ToInt32(dr["TotalStock"]),
                            ProductosCriticos = Convert.ToInt32(dr["ProductosCriticos"]),
                            PedidosPendientes = Convert.ToInt32(dr["PedidosPendientes"]),
                            PedidosDespachados = Convert.ToInt32(dr["PedidosDespachados"])
                        };
                    }
                    dr.Close();
                }

                // Movimientos por tipo
                model.MovimientosPorTipo = new List<MovimientoTipoVM>();
                using (SqlCommand cmd = new SqlCommand("sp_Dashboard_MovimientosPorTipo", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    var dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        model.MovimientosPorTipo.Add(new MovimientoTipoVM
                        {
                            Tipo = dr["tipoMovimiento"].ToString(),
                            Total = Convert.ToInt32(dr["Total"])
                        });
                    }
                    dr.Close();
                }

                // Movimientos por fecha
                model.MovimientosPorFecha = new List<MovimientoFechaVM>();
                using (SqlCommand cmd = new SqlCommand("sp_Dashboard_MovimientosPorFecha", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    var dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        model.MovimientosPorFecha.Add(new MovimientoFechaVM
                        {
                            Fecha = Convert.ToDateTime(dr["Fecha"]),
                            Total = Convert.ToInt32(dr["TotalMovimientos"])
                        });
                    }
                    dr.Close();
                }

                // Productos críticos
                model.ProductosCriticos = new List<ProductoCriticoVM>();
                using (SqlCommand cmd = new SqlCommand("sp_Dashboard_ProductosCriticos", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    var dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        model.ProductosCriticos.Add(new ProductoCriticoVM
                        {
                            Nombre = dr["nombre"].ToString(),
                            StockActual = Convert.ToInt32(dr["stockActual"]),
                            StockMinimo = Convert.ToInt32(dr["stockMinimo"])
                        });
                    }
                    dr.Close();
                }

                // Top productos más movidos
                model.TopProductos = new List<TopProductoVM>();
                using (SqlCommand cmd = new SqlCommand("sp_Dashboard_TopProductosMovidos", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    var dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        model.TopProductos.Add(new TopProductoVM
                        {
                            Nombre = dr["nombre"].ToString(),
                            TotalMovido = Convert.ToInt32(dr["TotalMovido"])
                        });
                    }
                    dr.Close();
                }

                // Pedidos por estado
                model.PedidosPorEstado = new List<PedidoEstadoVM>();
                using (SqlCommand cmd = new SqlCommand("sp_Dashboard_PedidosPorEstado", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    var dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        model.PedidosPorEstado.Add(new PedidoEstadoVM
                        {
                            Estado = dr["estado"].ToString(),
                            Total = Convert.ToInt32(dr["Total"])
                        });
                    }
                    dr.Close();
                }

                // Despachos por fecha
                model.DespachosPorFecha = new List<DespachoFechaVM>();
                using (SqlCommand cmd = new SqlCommand("sp_Dashboard_DespachosPorFecha", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    var dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        model.DespachosPorFecha.Add(new DespachoFechaVM
                        {
                            Fecha = Convert.ToDateTime(dr["Fecha"]),
                            Total = Convert.ToInt32(dr["TotalDespachos"])
                        });
                    }
                    dr.Close();
                }

            }

            return View(model);
        }
        public ActionResult PowerBI()
        {
            return View();
        }
    }
}
