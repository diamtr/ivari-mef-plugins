using NUnit.Framework;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using TestSharedInterfacesLib;
using ToolBox.Tests.Engine;

namespace ToolBox.Tests
{
  public class FileSystemLoaderTests
  {
    TestDirectories dirs;

    [SetUp]
    public void SetUp()
    {
      this.dirs = new TestDirectories();
    }

    [TearDown]
    public void TearDown()
    {
      this.dirs.TryClearAll();
      this.dirs = null;
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
      var libSourcesPath = Path.GetFullPath(Path.Combine(basePath, "..\\..\\..\\Engine\\lib"));
      var pluginDllName = "TestPlugin0Lib.dll";
      var pluginDirName = Path.Combine(basePath, "Plugins");
      dirs.CreateIfNotExists(pluginDirName);
      File.Copy(Path.Combine(libSourcesPath, pluginDllName), Path.Combine(pluginDirName, pluginDllName), true);
      var configuration = new FileSystemPlainConfiguration();
      configuration.AddSource(pluginDirName);
      var loader = new FileSystemLoader(configuration);
      var res = loader.Load<IPlugin>();
      Assert.IsNotNull(res);
      Assert.IsNotEmpty(res);
      Assert.AreEqual(1, res.Count);
      Assert.AreEqual("TestPlugin0Lib.Plugin", res[0].GetType().FullName);
    }
  }
}