using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GestionLogisticaTI.ViewModels
{
    public class DashboardViewModel
    {
        public KPIViewModel KPIs { get; set; }

        public List<MovimientoTipoVM> MovimientosPorTipo { get; set; }
        public List<MovimientoFechaVM> MovimientosPorFecha { get; set; }

        public List<ProductoCriticoVM> ProductosCriticos { get; set; }
        public List<TopProductoVM> TopProductos { get; set; }

        public List<PedidoEstadoVM> PedidosPorEstado { get; set; }
        public List<DespachoFechaVM> DespachosPorFecha { get; set; }
    }
    public class KPIViewModel
    {
        public int TotalProductos { get; set; }
        public int TotalStock { get; set; }
        public int ProductosCriticos { get; set; }
        public int PedidosPendientes { get; set; }
        public int PedidosDespachados { get; set; }
    }

    public class MovimientoTipoVM
    {
        public string Tipo { get; set; }
        public int Total { get; set; }
    }

    public class MovimientoFechaVM
    {
        public DateTime Fecha { get; set; }
        public int Total { get; set; }
    }

    public class ProductoCriticoVM
    {
        public string Nombre { get; set; }
        public int StockActual { get; set; }
        public int StockMinimo { get; set; }
    }

    public class TopProductoVM
    {
        public string Nombre { get; set; }
        public int TotalMovido { get; set; }
    }

    public class PedidoEstadoVM
    {
        public string Estado { get; set; }
        public int Total { get; set; }
    }

    public class DespachoFechaVM
    {
        public DateTime Fecha { get; set; }
        public int Total { get; set; }
    }
}