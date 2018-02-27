using Albacore.ChangeLog;
using Dapper;
using System;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Transactions;

namespace Albacore
{
    internal class ScriptRunner
    {
        private readonly string _connectionString;
        private static string[] ScriptSeparators = new string[] { "GO" };
        private static char[] TrimCharacters = new char[] { ' ', '\r', '\n' };

        internal ScriptRunner(string connectionString)
        {
            _connectionString = connectionString;
        }

        internal void RunScripts(string scriptDirectory)
        {
            var changeLogRepository = new ChangeLogRepository(_connectionString);

            var sqlFiles = Directory.EnumerateFiles(scriptDirectory).ToArray();
            var changeLog = changeLogRepository.GetChangelog()
                .Select(x => x.ScriptName).ToArray();
            
            using (var deployment = new TransactionScope())
            {
                foreach (var file in sqlFiles)
                {
                    if (!changeLog.Any(x => file.EndsWith(x)))
                    {
                        var scriptFile = new FileInfo(file);
                        var scriptText = scriptFile.OpenText().ReadToEnd();

                        var scriptBatches = scriptText.Split(ScriptSeparators, StringSplitOptions.RemoveEmptyEntries)
                            .Select(x => x.Trim(TrimCharacters));

                        using (var sqlDatabase = new SqlConnection(_connectionString))
                        {
                            foreach (var script in scriptBatches)
                            {
                                sqlDatabase.Execute(script);
                            }
                        }
                        changeLogRepository.UpdateChangeLog(file);
                    }
                }
                deployment.Complete();
            }
        }
    }
}
