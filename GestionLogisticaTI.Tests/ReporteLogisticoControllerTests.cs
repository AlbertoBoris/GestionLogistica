using System;
using System.Collections.Generic;
using System.Web.Mvc;
using GestionLogisticaTI.Controllers;
using GestionLogisticaTI.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace GestionLogisticaTI.Tests
{
    [TestClass]
    public class ReporteLogisticoControllerTests
    {
        [TestMethod]
        public void Index_SinTipoReporte_RetornaVistaConResultadosNulos()
        {
            var controller = new ReporteLogisticoController();

            var resultado = controller.Index(null, null, null) as ViewResult;

            Assert.IsNotNull(resultado, "Debe retornar una ViewResult");
            var model = resultado.Model as ReporteLogisticoViewModel;
            Assert.IsNotNull(model, "El modelo no debe ser nulo");
            Assert.IsNull(model.Resultados, "Sin tipo de reporte no debe haber resultados");
        }

        [TestMethod]
        public void Index_SinTipoReporte_ModeloContieneListaDeTipos()
        {
            var controller = new ReporteLogisticoController();

            var resultado = controller.Index(null, null, null) as ViewResult;
            var model = resultado.Model as ReporteLogisticoViewModel;

            Assert.IsNotNull(model.TiposReporte, "La lista de tipos no debe ser nula");
            Assert.AreEqual(7, model.TiposReporte.Count, "Debe haber exactamente 7 tipos de reporte");
        }

        [TestMethod]
        public void Index_SinTipoReporte_FechasQuedaronNulas()
        {
            var controller = new ReporteLogisticoController();

            var resultado = controller.Index(null, null, null) as ViewResult;
            var model = resultado.Model as ReporteLogisticoViewModel;

            Assert.IsNull(model.FechaInicio, "FechaInicio debe ser nula");
            Assert.IsNull(model.FechaFin, "FechaFin debe ser nula");
        }

        [TestMethod]
        public void Index_ConTipoReporte_ModeloConservaTipoSeleccionado()
        {
            var controller = new ReporteLogisticoController();

            var resultado = controller.Index("Inventario", null, null) as ViewResult;
            var model = resultado.Model as ReporteLogisticoViewModel;

            Assert.AreEqual("Inventario", model.TipoReporte,
                "El modelo debe conservar el tipo de reporte seleccionado");
        }

        [TestMethod]
        public void Integracion_sp_Reporte_InventarioActual_RetornaDatos()
        {
            var connStr = ConfigurationManager
                .ConnectionStrings["ConexionLogistica"].ConnectionString;
            var resultados = new List<object>();

            using (var conn = new SqlConnection(connStr))
            using (var cmd = new SqlCommand("sp_Reporte_InventarioActual", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                conn.Open();
                using (var reader = cmd.ExecuteReader())
                    while (reader.Read())
                        resultados.Add(new object());
            }

            Assert.IsTrue(resultados.Count > 0,
                "El SP de inventario debe retornar al menos un producto");
        }

        [TestMethod]
        public void Integracion_sp_Reporte_DespachosPorFecha_RetornaDatosEnRango()
        {
            var connStr = ConfigurationManager
                .ConnectionStrings["ConexionLogistica"].ConnectionString;
            var resultados = new List<object>();

            using (var conn = new SqlConnection(connStr))
            using (var cmd = new SqlCommand("sp_Reporte_DespachosPorFecha", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@fechaInicio", new DateTime(2026, 1, 1));
                cmd.Parameters.AddWithValue("@fechaFin", new DateTime(2026, 3, 31));
                conn.Open();
                using (var reader = cmd.ExecuteReader())
                    while (reader.Read())
                        resultados.Add(new object());
            }

            Assert.IsTrue(resultados.Count > 0,
                "Debe retornar despachos entre enero y marzo 2026");
        }

        [TestMethod]
        public void Integracion_sp_Reporte_DespachosPorFecha_RetornaVacioFueraDeRango()
        {
            var connStr = ConfigurationManager
                .ConnectionStrings["ConexionLogistica"].ConnectionString;
            var resultados = new List<object>();

            using (var conn = new SqlConnection(connStr))
            using (var cmd = new SqlCommand("sp_Reporte_DespachosPorFecha", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@fechaInicio", new DateTime(2020, 1, 1));
                cmd.Parameters.AddWithValue("@fechaFin", new DateTime(2020, 12, 31));
                conn.Open();
                using (var reader = cmd.ExecuteReader())
                    while (reader.Read())
                        resultados.Add(new object());
            }

            Assert.AreEqual(0, resultados.Count,
                "No debe retornar datos para fechas sin registros");
        }

        [TestMethod]
        public void Integracion_Index_ConDespachos_RetornaResultados()
        {
            var controller = new ReporteLogisticoController();
            var inicio = new DateTime(2026, 1, 1);
            var fin = new DateTime(2026, 3, 31);

            var resultado = controller.Index("Despachos", inicio, fin) as ViewResult;
            var model = resultado.Model as ReporteLogisticoViewModel;

            Assert.IsNotNull(model.Resultados, "Debe haber resultados");
            Assert.IsTrue(model.Resultados.Count > 0,
                "Debe retornar al menos un despacho en el rango");
        }
    }
}