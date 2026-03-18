using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GestionLogisticaTI.ViewModels
{
    public class IngresoViewModel
    {
        [Required]
        [StringLength(16, ErrorMessage = "Máximo 16 caracteres")]
        [RegularExpression("^[a-zA-Z0-9]*$", ErrorMessage = "Solo letras y números")]
        public string NumeroDocumento { get; set; }

        [StringLength(100)]
        public string Motivo { get; set; }

        public int IdProducto { get; set; }

        public int Cantidad { get; set; }

        public List<ProductoViewModel> Productos { get; set; }

        public List<IngresoDetalleViewModel> Detalles { get; set; }
    }
}