using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GestionLogisticaTI.ViewModels
{
    public class PedidoAutorizacionDetalleViewModel
    {
        public int IdProducto { get; set; }

        public string Producto { get; set; }

        public int Cantidad { get; set; }

        public int StockActual { get; set; }
    }
}