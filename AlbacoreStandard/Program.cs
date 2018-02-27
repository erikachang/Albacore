using System;

namespace Albacore
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 2)
            {
                Console.WriteLine("Usage: AlbacoreDBDeployer <script_directory> (<connection_string> | -local <instance_name> -db <database_name>)");
                return;
            }

            var scriptDirectory = args[0];
            string connectionString = String.Empty;

            if (args[1].Equals("-local"))
            {
                var instanceName = args[2];
                var databaseName = args[4];

                connectionString = $@"Server=(localdb)\{instanceName};Initial Catalog={databaseName};Integrated Security=true";
            }
            else
            {
                connectionString = args[1];
            }

            var scriptRunner = new ScriptRunner(connectionString);
            scriptRunner.RunScripts(scriptDirectory);
        }
    }
}
