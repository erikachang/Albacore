using System;

namespace Albacore
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 2)
            {
                Console.WriteLine(Properties.Strings.Usage);
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

            Console.WriteLine(Properties.Strings.Hello);

            var scriptRunner = new ScriptRunner(connectionString);
            int scriptsRun = scriptRunner.RunScripts(scriptDirectory);
            
            Console.WriteLine(scriptsRun > 0 ? Properties.Strings.Done : Properties.Strings.None);
        }
    }
}
