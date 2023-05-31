using System.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace BookApi.Data
{
    public class SqlConnectionFactory
    {
        private readonly IConfiguration _config;

        public SqlConnectionFactory(IConfiguration config)
        {
            _config = config;
        }

        public IDbConnection GetConnection()
        {
            string connectionString = _config.GetConnectionString("DefaultConnection")!;
            return new SqlConnection(connectionString);
        }

    }
}