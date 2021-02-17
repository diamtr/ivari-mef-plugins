using NUnit.Framework;
using System;
using System.IO;
using System.Reflection;
using ToolBox.Tests.Engine;

namespace ToolBox.Tests
{
  [TestFixture]
  public class FileSystemRecursiveConfigurationTests
  {
    private TestDirectories directories;

    [SetUp]
    public void SetUp()
    {
      this.directories = new TestDirectories();
    }

    [TearDown]
    public void TearDown()
    {
      this.directories.TryClearAll();
      this.directories = null;
    }

    [Test]
    public void Create()
    {
      var configuration = new FileSystemRecursiveConfiguration();
      Assert.AreEqual(1, configuration.Depth, "Depth");
      Assert.IsEmpty(configuration.GetPaths());
    }

    [Test]
    public void UpdateEmpty()
    {
      var configuration = new FileSystemRecursiveConfiguration();
      configuration.AddSource(string.Empty);
      Assert.IsEmpty(configuration.GetPaths());
    }

    [Test]
    public void UpdateWithoutSubdirectories()
    {
      var basedir = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                    "Tools");
      this.directories.CreateIfNotExists(basedir);

      var configuration = new FileSystemRecursiveConfiguration();
      configuration.AddSource(basedir);
      Assert.IsEmpty(configuration.GetPaths());
    }

    [Test]
    public void UpdateWithoutSubdirectoriesDepth0()
    {
      var basedir = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                    "Tools");
      this.directories.CreateIfNotExists(basedir);

      var configuration = new FileSystemRecursiveConfiguration();
      configuration.Depth = 0;
      configuration.AddSource(basedir);
      this.CheckPaths(configuration, 1, new string[] { basedir });
    }

    [Test]
    public void UpdateWith1LvlSubdirectories()
    {
      var basedir = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                    "Tools");
      var subdir1 = Path.Combine(basedir, "SubDir1");
      var subdir2 = Path.Combine(basedir, "SubDir2");
      this.directories.CreateIfNotExists(basedir);
      this.directories.CreateIfNotExists(subdir1);
      this.directories.CreateIfNotExists(subdir2);

      var configuration = new FileSystemRecursiveConfiguration();
      configuration.AddSource(basedir);
      this.CheckPaths(configuration, 2, new string[] { subdir1, subdir2 });
    }

    [Test]
    public void UpdateWith2LvlSubdirectories()
    {
      var basedir = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                    "Tools");
      var subdir1 = Path.Combine(basedir, "SubDir1");
      var subdir11 = Path.Combine(subdir1, "SubDir11");
      var subdir12 = Path.Combine(subdir1, "SubDir12");
      var subdir2 = Path.Combine(basedir, "SubDir2");
      var subdir21 = Path.Combine(subdir2, "SubDir21");
      var subdir22 = Path.Combine(subdir2, "SubDir22");
      this.directories.CreateIfNotExists(basedir);
      this.directories.CreateIfNotExists(subdir1);
      this.directories.CreateIfNotExists(subdir11);
      this.directories.CreateIfNotExists(subdir12);
      this.directories.CreateIfNotExists(subdir2);
      this.directories.CreateIfNotExists(subdir21);
      this.directories.CreateIfNotExists(subdir22);

      var configuration = new FileSystemRecursiveConfiguration();
      configuration.AddSource(basedir);
      this.CheckPaths(configuration, 2, new string[] { subdir1, subdir2 });
    }

    [Test]
    public void UpdateWith2LvlSubdirectoriesDepth2()
    {
      var basedir = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                    "Tools");
      var subdir1 = Path.Combine(basedir, "SubDir1");
      var subdir11 = Path.Combine(subdir1, "SubDir11");
      var subdir12 = Path.Combine(subdir1, "SubDir12");
      var subdir2 = Path.Combine(basedir, "SubDir2");
      var subdir21 = Path.Combine(subdir2, "SubDir21");
      var subdir22 = Path.Combine(subdir2, "SubDir22");
      this.directories.CreateIfNotExists(basedir);
      this.directories.CreateIfNotExists(subdir1);
      this.directories.CreateIfNotExists(subdir11);
      this.directories.CreateIfNotExists(subdir12);
      this.directories.CreateIfNotExists(subdir2);
      this.directories.CreateIfNotExists(subdir21);
      this.directories.CreateIfNotExists(subdir22);

      var configuration = new FileSystemRecursiveConfiguration();
      configuration.Depth = 2;
      configuration.AddSource(basedir);
      this.CheckPaths(configuration, 4, new string[] { subdir11, subdir12, subdir21, subdir22 });
    }

    private void CheckPaths(FileSystemRecursiveConfiguration configuration,
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
