using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GestionLogisticaTI.ViewModels
{
    public class ClienteViewModel
    {
        public int IdCliente { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(100)]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "Debe ingresar RUC o DNI")]
        [StringLength(11)]
        public string RucDni { get; set; }

        [Required(ErrorMessage = "Debe ingresar teléfono")]
        [Phone(ErrorMessage = "Teléfono inválido")]
        public string Telefono { get; set; }

        [Required(ErrorMessage = "Debe ingresar la Dirección")]
        [StringLength(60)]
        public string Direccion { get; set; }

        public string Estado { get; set; }

        public bool EsActivo
        {
            get { return Estado == "Activo"; }
        }
    }
}