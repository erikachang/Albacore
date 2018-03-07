using System;
using System.Linq;
using System.Collections.Generic;
using System.Data.SqlClient;
using Dapper;

namespace Albacore.ChangeLog
{
    public class ChangeLogRepository
    {
        private readonly String _connectionString;

        public ChangeLogRepository(String connectionString)
        {
            _connectionString = connectionString;
        }

        public IEnumerable<ChangeLog> GetChangelog()
        {
            var query = @"SELECT * FROM SchemaChangeLog";

            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.Query<ChangeLog>(query);
            }
        }

        public bool UpdateChangeLog(String scriptFullName)
        {
            var scriptName = scriptFullName.Split('\\')
                .Last();

            var scriptNumber = scriptName.Split('-').First();

            var sqlUpdatedChangelog = String.Format(@"INSERT INTO SchemaChangeLog (ScriptName)
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
