using GestionLogisticaTI.Data;
using GestionLogisticaTI.Filters;
using GestionLogisticaTI.ViewModels;
using GestionLogisticaTI.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GestionLogisticaTI.Controllers
{
    [AuthorizeRole("Administrador")]
    public class UsuarioController : BaseController
    {
        public ActionResult Index()
        {
            List<UsuarioViewModel> lista = new List<UsuarioViewModel>();

            using (SqlConnection conn = ConexionBD.ObtenerConexion())
            {
                SqlCommand cmd = new SqlCommand("sp_Usuario_Listar", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    lista.Add(new UsuarioViewModel
                    {
                        IdUsuario = Convert.ToInt32(reader["idUsuario"]),
                        Nombre = reader["nombre"].ToString(),
                        Correo = reader["correo"].ToString(),
                        Estado = reader["estado"].ToString(),
                        IdRol = Convert.ToInt32(reader["idRol"]),
                        NombreRol = reader["nombreRol"].ToString()
                    });
                }
            }

            return View(lista);
        }
        private List<SelectListItem> ObtenerRoles()
        {
            List<SelectListItem> lista = new List<SelectListItem>();

            using (SqlConnection conn = ConexionBD.ObtenerConexion())
            {
                SqlCommand cmd = new SqlCommand("SELECT idRol, nombreRol FROM Rol", conn);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    lista.Add(new SelectListItem
                    {
                        Value = reader["idRol"].ToString(),
                        Text = reader["nombreRol"].ToString()
                    });
                }
            }

            return lista;
        }
        public ActionResult Create()
        {
            UsuarioViewModel model = new UsuarioViewModel();
            model.Roles = ObtenerRoles();
            return View(model);
        }
        [HttpPost]
        public ActionResult Create(UsuarioViewModel model)
        {
            string passwordHash = PasswordHelper.HashPassword(model.Password);

            using (SqlConnection conn = ConexionBD.ObtenerConexion())
            {
                SqlCommand cmd = new SqlCommand("sp_Usuario_Insertar", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@nombre", model.Nombre);
                cmd.Parameters.AddWithValue("@correo", model.Correo);
                cmd.Parameters.AddWithValue("@passwordHash", passwordHash);
                cmd.Parameters.AddWithValue("@idRol", model.IdRol);

                conn.Open();
                cmd.ExecuteNonQuery();
            }

            return RedirectToAction("Index");
        }
        public ActionResult Edit(int id)
        {
            UsuarioViewModel model = new UsuarioViewModel();
            model.Roles = ObtenerRoles();

            using (SqlConnection conn = ConexionBD.ObtenerConexion())
            {
                SqlCommand cmd = new SqlCommand("sp_Usuario_ObtenerPorId", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@idUsuario", id);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    model.IdUsuario = Convert.ToInt32(reader["idUsuario"]);
                    model.Nombre = reader["nombre"].ToString();
                    model.Correo = reader["correo"].ToString();
                    model.IdRol = Convert.ToInt32(reader["idRol"]);
                }
            }

            return View(model);
        }
        [HttpPost]
        public ActionResult Edit(UsuarioViewModel model)
        {
            using (SqlConnection conn = ConexionBD.ObtenerConexion())
            {
                conn.Open();

                // 1️⃣ Actualizar datos básicos
                SqlCommand cmd = new SqlCommand("sp_Usuario_Actualizar", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@idUsuario", model.IdUsuario);
                cmd.Parameters.AddWithValue("@nombre", model.Nombre);
                cmd.Parameters.AddWithValue("@correo", model.Correo);
                cmd.Parameters.AddWithValue("@idRol", model.IdRol);

                cmd.ExecuteNonQuery();

                // 2️⃣ Si ingresó nueva contraseña, actualizar
                if (!string.IsNullOrEmpty(model.Password))
                {
                    string hash = HashHelper.GenerarHash(model.Password);

                    SqlCommand cmdPass = new SqlCommand("sp_Usuario_ActualizarPassword", conn);
                    cmdPass.CommandType = CommandType.StoredProcedure;

                    cmdPass.Parameters.AddWithValue("@idUsuario", model.IdUsuario);
                    cmdPass.Parameters.AddWithValue("@passwordHash", hash);

                    cmdPass.ExecuteNonQuery();
                }
            }

            return RedirectToAction("Index");
        }
        public ActionResult CambiarEstado(int id)
        {
            int idSesion = Convert.ToInt32(Session["IdUsuario"]);

            // No permitir que el usuario se desactive a sí mismo
            if (id == idSesion)
            {
                TempData["Error"] = "No puede desactivar su propio usuario.";
                return RedirectToAction("Index");
            }

            using (SqlConnection conn = ConexionBD.ObtenerConexion())
            {
                SqlCommand cmd = new SqlCommand("sp_Usuario_CambiarEstado", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@idUsuario", id);

                conn.Open();
                cmd.ExecuteNonQuery();
            }

            return RedirectToAction("Index");
        }
    }
}
