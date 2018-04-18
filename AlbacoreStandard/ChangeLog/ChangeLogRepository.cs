using System;
using System.Linq;
using System.Collections.Generic;
using System.Data.SqlClient;
using Dapper;

namespace Albacore.ChangeLog
{
    public class ChangeLogRepository
    {
        private readonly string _connectionString;

        public ChangeLogRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IEnumerable<ChangeLog> GetChangelog()
        {
            string query = @"SELECT * FROM SchemaChangeLog";

            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.Query<ChangeLog>(query);
            }
        }

        public bool UpdateChangeLog(string scriptFullName)
        {
            string scriptName = scriptFullName.Split('\\')
                .Last();

            string scriptNumber = scriptName.Split('-').First();

            string sqlUpdatedChangelog = String.Format(@"INSERT INTO SchemaChangeLog (ScriptName)
                                        VALUES ('{0}')",
                                        scriptName);

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Execute(sqlUpdatedChangelog);
            }

            return true;
        }
    }
}
