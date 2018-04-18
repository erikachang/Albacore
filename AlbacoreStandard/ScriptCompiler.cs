using Albacore.Properties;
using System;
using System.IO;
using System.Linq;
using System.Text;

namespace Albacore
{
    internal class ScriptCompiler
    {
        private static readonly string[] ScriptSeparators = new string[] { "GO" };
        private static readonly char[] TrimCharacters = new char[] { ' ', '\r', '\n' };

        private static readonly string _updateChangeLogScript = "INSERT INTO [SchemaChangeLog] ([ScriptName]) VALUES ('{0}')";
        private static readonly string _ifScriptNotYetAdded = "IF (SELECT COUNT(1) FROM [SchemaChangeLog] WHERE [ScriptName] = '{0}') = 0";
        private static readonly string _begin = "BEGIN";
        private static readonly string _end = "END";

        internal ScriptCompiler() { }

        internal string CompileSingleScript(string scriptDirectory)
        {
            var scriptBuilder = new StringBuilder();

            var sqlFiles = Directory.EnumerateFiles(scriptDirectory)
                .ToList();
            sqlFiles.Sort();

            foreach (string file in sqlFiles)
            {
                string fileName = file.Split('\\').Last();

                var scriptFile = new FileInfo(file);
                Console.WriteLine(String.Format(Strings.RunningScript, scriptFile.Name));

                string scriptText = scriptFile.OpenText().ReadToEnd();

                var scriptBatches = scriptText.Split(ScriptSeparators, StringSplitOptions.RemoveEmptyEntries)
                    .Select(x => x.Trim(TrimCharacters));

                foreach (string script in scriptBatches)
                {
                    scriptBuilder.AppendLine(ScriptSeparators.First());
                    if (!String.IsNullOrEmpty(script))
                    {
                        scriptBuilder.AppendLine(String.Format(_ifScriptNotYetAdded, fileName));
                        scriptBuilder.AppendLine(_begin);
                        scriptBuilder.AppendLine(script);
                        scriptBuilder.AppendLine(_end);
                    }
                }

                scriptBuilder.AppendLine(String.Format(_ifScriptNotYetAdded, fileName));
                scriptBuilder.AppendLine(String.Format(_updateChangeLogScript, fileName));

                Console.WriteLine(Strings.ScriptDone);
            }

            return scriptBuilder.ToString();
        }
    }
}
