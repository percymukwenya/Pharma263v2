using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Pharma263.Persistence.Contexts
{
    public class DapperContext
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        public DapperContext(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("Pharma263Connection");
            //_connectionString = _configuration.GetConnectionString("LocalConnection");
        }

        public IDbConnection CreateConnection() => new SqlConnection(_connectionString);
    }
}
