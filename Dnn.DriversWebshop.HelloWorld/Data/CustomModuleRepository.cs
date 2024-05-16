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

        public IEnumerable<HardwareBenchmark> GetCpuBenchmarks()
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                string query = "SELECT name AS CpuName, percentage AS CpuPercentage FROM module_cpu_benchmarks";
                return db.Query<HardwareBenchmark>(query);
            }
        }

        public IEnumerable<HardwareBenchmark> GetGpuBenchmarks()
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                string query = "SELECT name AS GpuName, percentage AS GpuPercentage FROM module_gpu_benchmarks";
                return db.Query<HardwareBenchmark>(query);
            }
        }

        public IEnumerable<HardwareBenchmark> GetRamBenchmarks()
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                string query = "SELECT name AS RamName, percentage AS RamPercentage FROM module_ram_benchmarks";
                return db.Query<HardwareBenchmark>(query);
            }
        }
    }
}