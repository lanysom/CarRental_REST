using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.DataAccessLayer
{
    static class Database
    {
        /// <summary>
        /// Creates and returns an open IDbConnection object
        /// </summary>
        public static IDbConnection Open()
        {
            var connStrBldr = new SqlConnectionStringBuilder
            {
                DataSource = @"(localdb)\MSSQLLocalDB",
                InitialCatalog = "CarRental_v3",
                IntegratedSecurity = true
            };
            var conn = new SqlConnection(connStrBldr.ConnectionString);
            conn.Open();
            return conn;
        }
    }
}
