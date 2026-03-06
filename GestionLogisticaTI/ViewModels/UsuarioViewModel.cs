using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GestionLogisticaTI.ViewModels
{
    public class UsuarioViewModel
    {
        public int IdUsuario { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(100)]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El correo es obligatorio")]
        [EmailAddress(ErrorMessage = "Correo inválido")]
        public string Correo { get; set; }
        
        public string Estado { get; set; }
        
        public int IdRol { get; set; }

        [Required(ErrorMessage = "Debe seleccionar un rol")]
        public string NombreRol { get; set; }

        [MinLength(6, ErrorMessage = "La contraseña debe tener mínimo 6 caracteres")]
        public string Password { get; set; }
        
        public List<SelectListItem> Roles { get; set; }
    }
}