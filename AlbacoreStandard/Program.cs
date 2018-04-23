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

            var scriptRunner = new ScriptCompiler();
            string script = scriptRunner.CompileSingleScript(scriptDirectory);

            Console.WriteLine("How would you like to name your script? ");
            string outputName = Console.ReadLine();

            string outputFile = Path.Combine(scriptDirectory, outputName);

            File.WriteAllText(outputFile, script);

            Console.WriteLine(Strings.Done);
            Console.ReadKey();
        }
    }
}
