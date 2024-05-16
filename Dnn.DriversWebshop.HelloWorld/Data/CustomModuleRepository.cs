using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using DriversWebshop.Dnn.Dnn.DriversWebshop.HelloWorld.Models;
using Dapper;

namespace DriversWebshop.Dnn.Dnn.DriversWebshop.HelloWorld.Data
{
    public class CustomModuleRepository
    {
        private readonly string _connectionString;

        public CustomModuleRepository()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["SiteSqlServer"].ConnectionString;
        }

        public IEnumerable<CustomModuleItem> GetAllItems()
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                string query = @"
                    SELECT 
                        cpu.percentage AS CpuPercentage,
                        gpu.percentage AS GpuPercentage,
                        ram.percentage AS RamPercentage
                    FROM 
                        module_cpu_benchmarks cpu
                    JOIN 
                        module_gpu_benchmarks gpu ON cpu.id = gpu.cpu_id
                    JOIN 
                        module_ram_benchmarks ram ON cpu.id = ram.cpu_id";

                return db.Query<CustomModuleItem>(query);
            }
        }
    }
}