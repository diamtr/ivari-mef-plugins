using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Text;
using NUnit.Framework;

namespace ToolBox.Tests.Engine
{
  public class TestAssemblies
  {
    public Assembly BuildAssembly(string name, string destination, string code, IEnumerable<string> references)
    {
      /* Look at that
       * https://laurentkempe.com/2019/02/18/dynamically-compile-and-run-code-using-dotNET-Core-3.0/
       */
      byte[] assemblyRaw;
      var codeString = SourceText.From(code);
      var options = CSharpParseOptions.Default.WithLanguageVersion(LanguageVersion.CSharp7_3);
      var parsedSyntaxTree = SyntaxFactory.ParseSyntaxTree(codeString, options);

      var referencesMetadata = new List<MetadataReference>();
      foreach (var reference in references)
        referencesMetadata.Add(MetadataReference.CreateFromFile(reference));

      var c_options = new CSharpCompilationOptions(
        OutputKind.DynamicallyLinkedLibrary,
        platform: Platform.AnyCpu,
        optimizationLevel: OptimizationLevel.Debug,
        assemblyIdentityComparer: DesktopAssemblyIdentityComparer.Default
        );

      var compilation = CSharpCompilation.Create(name,
          new[] { parsedSyntaxTree },
          references: referencesMetadata,
          options: c_options);

      using (var peStream = new MemoryStream())
      {
        var result = compilation.Emit(peStream);
        if (!result.Success)
        {
          var failures = result.Diagnostics.Where(diagnostic => diagnostic.IsWarningAsError ||
                                                                diagnostic.Severity == DiagnosticSeverity.Error);
          foreach (var diagnostic in failures)
            TestContext.Error.WriteLine("{0}: {1}", diagnostic.Id, diagnostic.GetMessage());

          Assert.Fail("Compilation done with error.");
        }

        assemblyRaw = peStream.ToArray();
      }

      var target = Path.Combine(destination, name);
      File.WriteAllBytes(target, assemblyRaw);

      return Assembly.Load(assemblyRaw);
    }

    public static List<string> GetSystemLibsPath()
    {
      /* See there
       * https://stackoverflow.com/questions/23907305/roslyn-has-no-reference-to-system-runtime
       * Adding some necessary .NET assemblies
       * These assemblies couldn't be loaded correctly via the same construction as above,
       * in specific the System.Runtime.
       */
      var returnList = new List<string>();
      //The location of the .NET assemblies
      var assemblyPath = Path.GetDirectoryName(typeof(object).Assembly.Location);
      returnList.Add(Path.Combine(assemblyPath, "mscorlib.dll"));
      returnList.Add(Path.Combine(assemblyPath, "System.dll"));
      returnList.Add(Path.Combine(assemblyPath, "System.Core.dll"));
      returnList.Add(Path.Combine(assemblyPath, "System.Runtime.dll"));
      return returnList;
    }
  }
}
