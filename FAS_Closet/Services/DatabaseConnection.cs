using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace FASCloset.Services
{
    public static class DatabaseConnection
    {
        private static readonly string ConnectionString = ConfigurationManager.ConnectionStrings["myconnectionstring"].ConnectionString;

        public static IDbConnection GetConnection()
        {
            return new SqlConnection(ConnectionString);
        }
    }
}
