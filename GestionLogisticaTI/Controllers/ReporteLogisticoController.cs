using GestionLogisticaTI.Data;
using GestionLogisticaTI.Filters;
using GestionLogisticaTI.ViewModels;
using Rotativa;
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
    public class ReporteLogisticoController : BaseController
    {

        public ActionResult Index(string tipoReporte, DateTime? fechaInicio, DateTime? fechaFin)
        {
            ReporteLogisticoViewModel model = new ReporteLogisticoViewModel();

            model.TiposReporte = ObtenerTiposReporte();
            model.TipoReporte = tipoReporte;
            model.FechaInicio = fechaInicio;
            model.FechaFin = fechaFin;

            if (!string.IsNullOrEmpty(tipoReporte))
            {
                model.Resultados = ObtenerDatos(tipoReporte, fechaInicio, fechaFin);
            }

            return View(model);
        }

        private List<SelectListItem> ObtenerTiposReporte()
        {
            return new List<SelectListItem>
            {
                new SelectListItem { Value = "Inventario", Text = "Inventario Actual" },
                new SelectListItem { Value = "BajoStock", Text = "Productos Bajo Stock" },
                new SelectListItem { Value = "Movimientos", Text = "Movimientos por Periodo" },
                new SelectListItem { Value = "Ajustes", Text = "Ajustes por Periodo" },
                new SelectListItem { Value = "Despachos", Text = "Despachos por Periodo" },
                new SelectListItem { Value = "Consolidado", Text = "Consolidado Logístico" },
                new SelectListItem { Value = "Resumen", Text = "Resumen Inventario" }
            };
        }
        private List<dynamic> ObtenerDatos(string tipoReporte, DateTime? fechaInicio, DateTime? fechaFin)
        {
            List<dynamic> lista = new List<dynamic>();

            using (SqlConnection conn = ConexionBD.ObtenerConexion())
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandType = CommandType.StoredProcedure;

                switch (tipoReporte)
                {
                    case "Inventario":
                        cmd.CommandText = "sp_Reporte_InventarioActual";
                        break;

                    case "BajoStock":
                        cmd.CommandText = "sp_Reporte_ProductosBajoStock";
                        break;

                    case "Movimientos":
                        cmd.CommandText = "sp_Reporte_MovimientosPorFecha";
                        cmd.Parameters.AddWithValue("@fechaInicio", fechaInicio ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@fechaFin", fechaFin ?? (object)DBNull.Value);
                        break;

                    case "Ajustes":
                        cmd.CommandText = "sp_Reporte_Ajustes";
                        cmd.Parameters.AddWithValue("@fechaInicio", fechaInicio ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@fechaFin", fechaFin ?? (object)DBNull.Value);
                        break;

                    case "Despachos":
                        cmd.CommandText = "sp_Reporte_DespachosPorFecha";
                        cmd.Parameters.AddWithValue("@fechaInicio", fechaInicio ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@fechaFin", fechaFin ?? (object)DBNull.Value);
                        break;

                    case "Consolidado":
                        cmd.CommandText = "sp_Reporte_LogisticoGeneral";
                        cmd.Parameters.AddWithValue("@fechaInicio", fechaInicio ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@fechaFin", fechaFin ?? (object)DBNull.Value);
                        break;

                    case "Resumen":
                        cmd.CommandText = "sp_Reporte_ResumenInventario";
                        break;
                }

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    var row = new System.Dynamic.ExpandoObject() as IDictionary<string, object>;

                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        row.Add(reader.GetName(i), reader[i]);
                    }

                    lista.Add(row);
                }
            }

            return lista;
        }
        public ActionResult DescargarPDF(string tipoReporte, DateTime? fechaInicio, DateTime? fechaFin)
        {
            ReporteLogisticoViewModel model = new ReporteLogisticoViewModel();

            model.TipoReporte = tipoReporte;
            model.FechaInicio = fechaInicio;
            model.FechaFin = fechaFin;
            model.Resultados = ObtenerDatos(tipoReporte, fechaInicio, fechaFin);

            return new ViewAsPdf("ReportePDF", model)
            {
                FileName = "ReporteLogistico.pdf",
                PageSize = Rotativa.Options.Size.A4,
                PageOrientation = Rotativa.Options.Orientation.Landscape,
                PageMargins = new Rotativa.Options.Margins(15, 15, 20, 15),

                CustomSwitches = "--footer-right \"Página [page] de [toPage]\" --footer-font-size 8"
            };
        }
    }
}
