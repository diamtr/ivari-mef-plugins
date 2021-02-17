using NUnit.Framework;
using System;
using System.IO;
using System.Reflection;
using ToolBox.Tests.Engine;

namespace ToolBox.Tests
{
  [TestFixture]
  public class FileSystemPlainConfigurationTests
  {
    [Test]
    public void Create()
    {
      var configuration = new FileSystemPlainConfiguration();
      Assert.IsEmpty(configuration.GetPaths());
    }

    [Test]
    public void UpdateSomeAndEmpty()
    {
      var configuration = new FileSystemPlainConfiguration();
      configuration.AddSource("C:\\");
      configuration.AddSource(string.Empty);

      this.CheckPaths(configuration, 1, new string[] { "C:\\" });
    }

    [Test]
    public void UpdateOnlyByDesignNoRecursive()
    {
      var dirs = new TestDirectories();
      var basedir = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                    "Tools");
      var subdir1 = Path.Combine(basedir, "SubDir1");
      var subdir2 = Path.Combine(basedir, "SubDir2");
      dirs.CreateIfNotExists(basedir);
      dirs.CreateIfNotExists(subdir1);
      dirs.CreateIfNotExists(subdir2);

      var configuration = new FileSystemPlainConfiguration();
      configuration.AddSource(basedir);
      this.CheckPaths(configuration, 1, new string[] { basedir });
      configuration.AddSource("C:\\");
      this.CheckPaths(configuration, 2, new string[] { basedir, "C:\\" });
      configuration.AddSource(subdir2);
      this.CheckPaths(configuration, 3, new string[] { basedir, "C:\\", subdir2 });

      dirs.DeleteIfCreated(subdir2);
      dirs.DeleteIfCreated(subdir1);
      dirs.DeleteIfCreated(basedir);
    }

    private void CheckPaths(FileSystemPlainConfiguration configuration,
      int expectedCount,
      string[] expectedPaths)
    {
      var paths = configuration.GetPaths();
      Assert.IsNotEmpty(paths);
      Assert.AreEqual(expectedCount, paths.Count);
      foreach (var path in expectedPaths)
        Assert.AreEqual(path, paths[Array.IndexOf(expectedPaths, path)]);
    }
  }
}
