using GestionLogisticaTI.Data;
using GestionLogisticaTI.Filters;
using GestionLogisticaTI.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GestionLogisticaTI.Controllers
{
    [AuthorizeRole("Ventas")]
    public class ClienteController : BaseController
    {
        /* Listar Cliente */
        public ActionResult Index(string nombre, string estado)
        {
            List<ClienteViewModel> lista = new List<ClienteViewModel>();

            using (SqlConnection conn = ConexionBD.ObtenerConexion())
            {
                SqlCommand cmd = new SqlCommand("sp_Cliente_Listar", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                if (!string.IsNullOrEmpty(nombre))
                    cmd.Parameters.AddWithValue("@nombre", nombre);
                else
                    cmd.Parameters.Add("@nombre", SqlDbType.VarChar).Value = DBNull.Value;

                if (!string.IsNullOrEmpty(estado))
                    cmd.Parameters.AddWithValue("@estado", estado);
                else
                    cmd.Parameters.Add("@estado", SqlDbType.VarChar).Value = DBNull.Value;

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    lista.Add(new ClienteViewModel
                    {
                        IdCliente = Convert.ToInt32(reader["idCliente"]),
                        Nombre = reader["nombre"].ToString(),
                        RucDni = reader["rucDni"].ToString(),
                        Telefono = reader["telefono"].ToString(),
                        Direccion = reader["direccion"].ToString(),
                        Estado = reader["estado"].ToString()
                    });
                }
            }

            ViewBag.FiltroNombre = nombre;
            ViewBag.FiltroEstado = estado;

            return View(lista);
        }
        public ActionResult CambiarEstado(int id)
        {
            using (SqlConnection conn = ConexionBD.ObtenerConexion())
            {
                SqlCommand cmd = new SqlCommand("sp_Cliente_CambiarEstado", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@idCliente", id);

                conn.Open();
                cmd.ExecuteNonQuery();
            }

            return RedirectToAction("Index");
        }
        /* Registrar Cliente */

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(ClienteViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            int nuevoId = 0;

            using (SqlConnection conn = ConexionBD.ObtenerConexion())
            {
                SqlCommand cmd = new SqlCommand("sp_Cliente_Insertar", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@nombre", model.Nombre);
                cmd.Parameters.AddWithValue("@rucDni", model.RucDni);
                cmd.Parameters.AddWithValue("@telefono", model.Telefono);
                cmd.Parameters.AddWithValue("@direccion", model.Direccion);

                conn.Open();
                nuevoId = Convert.ToInt32(cmd.ExecuteScalar());
            }

            if (!string.IsNullOrEmpty(returnUrl))
                return Redirect(returnUrl + "?clienteId=" + nuevoId);

            return RedirectToAction("Index");
        }

        /* Actualizar */
        public ActionResult Edit(int id)
        {
            ClienteViewModel model = new ClienteViewModel();

            using (SqlConnection conn = ConexionBD.ObtenerConexion())
            {
                SqlCommand cmd = new SqlCommand("sp_Cliente_ObtenerPorId", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@idCliente", id);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    model.IdCliente = Convert.ToInt32(reader["idCliente"]);
                    model.Nombre = reader["nombre"].ToString();
                    model.RucDni = reader["rucDni"].ToString();
                    model.Telefono = reader["telefono"].ToString();
                    model.Direccion = reader["direccion"].ToString();
                }
            }

            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(ClienteViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            using (SqlConnection conn = ConexionBD.ObtenerConexion())
            {
                SqlCommand cmd = new SqlCommand("sp_Cliente_Actualizar", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@idCliente", model.IdCliente);
                cmd.Parameters.AddWithValue("@nombre", model.Nombre);
                cmd.Parameters.AddWithValue("@rucDni", model.RucDni);
                cmd.Parameters.AddWithValue("@telefono", model.Telefono);
                cmd.Parameters.AddWithValue("@direccion", model.Direccion);

                conn.Open();
                cmd.ExecuteNonQuery();
            }

            return RedirectToAction("Index");
        }
    }
}
