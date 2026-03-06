using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GestionLogisticaTI.ViewModels
{
    public class PedidoCreateViewModel
    {
        public int IdPedido { get; set; }

        [Required(ErrorMessage = "Debe seleccionar un cliente")]
        public int IdCliente { get; set; }

        public List<SelectListItem> Clientes { get; set; }
        public List<SelectListItem> Productos { get; set; }
        public DateTime? FechaDespacho { get; set; }

        public List<PedidoDetalleViewModel> Detalles { get; set; }
    }
}