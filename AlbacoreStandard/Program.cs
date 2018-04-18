using Albacore.Properties;
using System;
using System.IO;

namespace Albacore
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 1)
            {
                Console.WriteLine(Strings.Usage);
                return;
            }

            string scriptDirectory = args[0];

            Console.WriteLine(Strings.Hello);

            var scriptRunner = new ScriptRunner(String.Empty);
            string script = scriptRunner.CompileSingleScript(scriptDirectory);
            string outputFile = Path.Combine(scriptDirectory, $"AlbacoreScript {DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss")}.sql");

            File.WriteAllText(outputFile, script);

            Console.WriteLine(Strings.Done);
        }
    }
}
