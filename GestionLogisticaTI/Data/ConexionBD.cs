using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace GestionLogisticaTI.Data
{
    public class ConexionBD
    {
        public static SqlConnection ObtenerConexion()
        {
            return new SqlConnection(
                ConfigurationManager.ConnectionStrings["ConexionLogistica"].ConnectionString);
        }
    }
}