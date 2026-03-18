using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GestionLogisticaTI.ViewModels
{
    public class AjusteInventarioViewModel
    {
        [Required]
        [StringLength(16, ErrorMessage = "Máximo 16 caracteres")]
        [RegularExpression("^[a-zA-Z0-9]*$", ErrorMessage = "Solo letras y números")]
        public string NumeroDocumento {  get; set; }

        [Required(ErrorMessage = "Debe ingresar motivo del ajuste")]
        [StringLength(200)]
        public string Motivo { get; set; }

        public List<ProductoViewModel> Productos { get; set; }

        public List<AjusteDetalleViewModel> Detalles { get; set; }
    }
}