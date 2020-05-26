using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Uplift.DataAccess.Data.Repository.Interface;

namespace Uplift.DataAccess.Data.Repository
{
    public class SPCall : ISPCall
    {

        private readonly ApplicationDbContext dbContext;
        private static string connectionString = "";

        public SPCall(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
            connectionString = dbContext.Database.GetDbConnection().ConnectionString;
        }
        public void Dispose()
        {
            dbContext.Dispose();
        }

        public T ExecuteReturnScaler<T>(string procedureName, DynamicParameters param = null)
        {
            using var sqlConnection = new SqlConnection(connectionString);
            sqlConnection.Open();
            return (T)Convert.ChangeType(sqlConnection.ExecuteScalar<T>(procedureName, param, commandType: CommandType.StoredProcedure), typeof(T));
        }

        public void ExecuteWithoutReturn(string procedureName, DynamicParameters param = null)
        {
            using var sqlConnection = new SqlConnection(connectionString);
            sqlConnection.Open();
            sqlConnection.Execute(procedureName, param, commandType: CommandType.StoredProcedure);
        }

        public IEnumerable<T> ReturnList<T>(string procedureName, DynamicParameters param = null)
        {
            using var sqlConnection = new SqlConnection(connectionString);
            sqlConnection.Open();
            return sqlConnection.Query<T>(procedureName, param, commandType: CommandType.StoredProcedure);
        }
    }
}
