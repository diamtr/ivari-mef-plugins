using NUnit.Framework;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using ToolBox.Tests.Engine;

namespace ToolBox.Tests
{
  public class FileSystemLoaderTests
  {
    TestDirectories dirs;
    TestAssemblies asmbls;

    [SetUp]
    public void SetUp()
    {
      this.dirs = new TestDirectories();
      this.asmbls = new TestAssemblies();
    }

    [TearDown]
    public void TearDown()
    {
      this.dirs.TryClearAll();
      this.dirs = null;
      this.asmbls = null;
    }

    [Test]
    public void InitPlainFileSystemLoader()
    {
      var conf = new FileSystemPlainConfiguration();
      var loader = new FileSystemLoader(conf);
      var res = loader.Load<object>();
      Assert.IsNotNull(res);
      Assert.IsEmpty(res);
    }

    [Test]
    public void InitRecursiveFileSystemLoader()
    {
      var conf = new FileSystemRecursiveConfiguration();
      var loader = new FileSystemLoader(conf);
      var res = loader.Load<object>();
      Assert.IsNotNull(res);
      Assert.IsEmpty(res);
    }

    [Test]
    public void FileSystemPlainLoadByInterface()
    {
      
      var basePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
      var libPath = Path.GetFullPath(Path.Combine(basePath, "..\\..\\..\\Engine\\lib"));
      var systemLibsPath = TestAssemblies.GetSystemLibsPath();
      var privateCoreLibPath = Path.Combine(libPath, "System.Private.CoreLib.dll");
      var compositionLibPath = Path.Combine(libPath, "System.ComponentModel.Composition.dll");

      // Shared interface dll
      var interfaceAsmblPath = Path.Combine(basePath, "Shared");
      this.dirs.CreateIfNotExists(interfaceAsmblPath);
      var references = systemLibsPath;
      references.Add(privateCoreLibPath);
      var interfaceAsmbl = this.asmbls.BuildAssembly(
        "Interfaces.dll",
        interfaceAsmblPath,
        SourceCodes.GetSharedInterfaceCode(),
        references);
      var interfaceType = interfaceAsmbl.GetTypes().FirstOrDefault(x => x.FullName == "Interfaces.ITestable");

      // Plugin
      var pluginPath = Path.Combine(basePath, "pl1");
      this.dirs.CreateIfNotExists(pluginPath);
      var references_pl1 = systemLibsPath;
      references_pl1.Add(compositionLibPath);
      references.Add(privateCoreLibPath);
      references_pl1.Add(Path.Combine(interfaceAsmblPath, "Interfaces.dll"));
      var pl1_Asmbl = this.asmbls.BuildAssembly(
        "pl1.dll",
        pluginPath,
        SourceCodes.GetPluginCode(),
        references_pl1);

      // Loader
      var configuration = new FileSystemPlainConfiguration();
      configuration.AddSource(pluginPath);
      var loader = new FileSystemLoader(configuration);
      var res = typeof(FileSystemLoader).GetMethod("Load").MakeGenericMethod(interfaceType).Invoke(loader, null);
    }
  }
}