#region
using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Reflection;
using System.Text;

#endregion

namespace VeeSharpTemplate
{
    public static class Parser
    {
        public static string ParseSymbols(string mString)
        {
            var result = "";
            var reader = new StringReader(mString);
            string line;

            while ((line = reader.ReadLine()) != null)
            {
                var trimmedLine = line.TrimStart('\t').TrimStart('\b').TrimStart('\r').TrimStart(' ');

                if (trimmedLine.StartsWith(Utils.Symbol))
                {
                    // This is a C# code line, just add it to the result

                    var spacing = line.Substring(0, line.IndexOf(trimmedLine));
                    var afterSymbolIndex = trimmedLine.IndexOf(Utils.Symbol) + Utils.Symbol.Length;
                    var afterSymbolCode = trimmedLine.Substring(afterSymbolIndex);
                    result += spacing + afterSymbolCode + Environment.NewLine;
                }
                else
                {
                    // This is a template code line
                    // It must be converted to a TemplateWriteLine method call
                    // Things that are between $$( )$$ must be converted to C#

                    var spacing = line.Substring(0, line.IndexOf(trimmedLine));
                    while (trimmedLine.IndexOf(Utils.SymbolOpen) != -1)
                    {
                        var indexStart = trimmedLine.IndexOf(Utils.SymbolOpen);
                        var indexEnd = trimmedLine.IndexOf(Utils.SymbolClose);
                        var symbolPart = trimmedLine.Substring(indexStart, indexEnd - indexStart + Utils.SymbolOpen.Length);
                        var content = symbolPart.Substring(Utils.SymbolOpen.Length, symbolPart.Length - Utils.SymbolOpen.Length - Utils.SymbolClose.Length);

                        trimmedLine = trimmedLine.Replace(symbolPart, string.Format("\" + {0} + \"", content));
                    }

                    var toProcess = "\"" + trimmedLine + "\"";
                    var processed = string.Format("{0}(\"{1}\"); {2}({3});", Utils.MethodWrite, spacing, Utils.MethodWriteLine, toProcess);

                    result += processed + Environment.NewLine;
                }
            }

            return result;
        }
        public static string Process(string mString)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append(Utils.ScriptPrefix);
            stringBuilder.Append(mString);
            stringBuilder.Append(Utils.ScriptSuffix);
            var result = stringBuilder.ToString();

            var assembly = CompileSource(result);
            if (assembly == null) return null;

            var type = assembly.GetType(string.Format("{0}.{1}", Utils.AssemblyName, Utils.ScriptName));
            var script = Activator.CreateInstance(type) as Script;
            if (script == null) throw new Exception("Error during dynamic compilation");
            script.Run();
            return script.GetResult();
        }
        private static Assembly CompileSource(string mSource)
        {
            var provider = CodeDomProvider.CreateProvider("CSharp");
            var parameters = new CompilerParameters
                             {
                                 GenerateExecutable = false,
                                 GenerateInMemory = true
                             };

            var executingAssembly = Assembly.GetExecutingAssembly();
            parameters.ReferencedAssemblies.Add("System.Collections.dll");
            parameters.ReferencedAssemblies.Add("System.IO.dll");
            parameters.ReferencedAssemblies.Add("System.Reflection.dll");
            parameters.ReferencedAssemblies.Add("System.Linq.dll");
            parameters.ReferencedAssemblies.Add("System.Linq.Expressions.dll");
            parameters.ReferencedAssemblies.Add("System.Text.RegularExpressions.dll");
            parameters.ReferencedAssemblies.Add("System.Threading.Tasks.dll");
            parameters.ReferencedAssemblies.Add("Microsoft.CSharp.dll");
            parameters.ReferencedAssemblies.Add(executingAssembly.Location);

            foreach (var assemblyName in executingAssembly.GetReferencedAssemblies())
                parameters.ReferencedAssemblies.Add(Assembly.Load(assemblyName).Location);

            var result = provider.CompileAssemblyFromSource(parameters, mSource);

            if (result.Errors.Count > 0)
            {
                var builder = new StringBuilder();
                foreach (CompilerError ce in result.Errors)
                {
                    builder.Append(ce);
                    builder.Append(Environment.NewLine);
                }
                throw new Exception(builder.ToString());
            }

            return result.CompiledAssembly;
        }
    }
}