using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GestionLogisticaTI.ViewModels
{
    public class IngresoViewModel
    {
        [Required(ErrorMessage = "Debe ingresar número de documento")]
        [StringLength(50)]
        public string NumeroDocumento { get; set; }

        [StringLength(100)]
        public string Motivo { get; set; }

        public int IdProducto { get; set; }

        public int Cantidad { get; set; }

        public List<ProductoViewModel> Productos { get; set; }

        public List<IngresoDetalleViewModel> Detalles { get; set; }
    }
}