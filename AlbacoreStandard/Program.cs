using Albacore.Properties;
using System;
using System.IO;

namespace Albacore
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(Strings.Hello);
            Console.WriteLine("Please inform script directory: ");
            string scriptDirectory = Console.ReadLine();

            if (!Directory.Exists(scriptDirectory))
            {
                Console.WriteLine("Couldn't read directory. Exiting...");
                return;
            }

            var scriptRunner = new ScriptCompiler(String.Empty);
            string script = scriptRunner.CompileSingleScript(scriptDirectory);
            string outputFile = Path.Combine(scriptDirectory, $"AlbacoreScript {DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss")}.sql");

            File.WriteAllText(outputFile, script);

            Console.WriteLine(Strings.Done);
        }
    }
}
