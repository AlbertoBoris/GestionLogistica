using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GestionLogisticaTI.ViewModels
{
    public class PedidoDespachoDetalleViewModel
    {
        public int IdPedido { get; set; }
        public DateTime Fecha { get; set; }
        public string Cliente { get; set; }

        public List<DetalleItem> Detalles { get; set; }

        public string NumeroDocumento { get; set; }

        public DateTime? FechaDespacho { get; set; }
    }
    public class DetalleItem
    {
        public string Producto { get; set; }
        public int Cantidad { get; set; }
    }
}