using NUnit.Framework;
using System;
using System.IO;
using System.Reflection;
using Plugins.FileSystem;
using Plugins.Tests.Engine;

namespace Plugins.Tests.FileSystem
{
  [TestFixture]
  public class ConfigurationTests
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
      var configuration = new Configuration();
      Assert.IsEmpty(configuration.GetPaths());
    }

    [Test]
    public void UpdateSomeAndEmpty()
    {
      var configuration = new Configuration();
      configuration.AddPath("C:\\");
      configuration.AddPath(string.Empty);

      this.CheckPaths(configuration, 1, new string[] { "C:\\" });
    }

    [Test]
    public void UpdateOnlyByDesignNoRecursive()
    {
      var basedir = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                    "Tools");
      var subdir1 = Path.Combine(basedir, "SubDir1");
      var subdir2 = Path.Combine(basedir, "SubDir2");
      this.directories.CreateIfNotExists(basedir);
      this.directories.CreateIfNotExists(subdir1);
      this.directories.CreateIfNotExists(subdir2);

      var configuration = new Configuration();
      configuration.AddPath(basedir);
      this.CheckPaths(configuration, 1, new string[] { basedir });
      configuration.AddPath("C:\\");
      this.CheckPaths(configuration, 2, new string[] { basedir, "C:\\" });
      configuration.AddPath(subdir2);
      this.CheckPaths(configuration, 3, new string[] { basedir, "C:\\", subdir2 });
    }

    [Test]
    public void UpdateEmptyRecursively()
    {
      var configuration = new Configuration();
      configuration.AddPathRecursively(string.Empty, 0);
      Assert.IsEmpty(configuration.GetPaths());
    }

    [Test]
    public void UpdateRecursivelyWithoutSubdirectoriesLevel1()
    {
      var basedir = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                    "Plugins");
      this.directories.CreateIfNotExists(basedir);

      var configuration = new Configuration();
      configuration.AddPathRecursively(basedir, 1);
      Assert.IsEmpty(configuration.GetPaths());
    }

    [Test]
    public void UpdateRecursivelyWithoutSubdirectoriesLevel0()
    {
      var basedir = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                    "Plugins");
      this.directories.CreateIfNotExists(basedir);

      var configuration = new Configuration();
      configuration.AddPathRecursively(basedir, 0);
      this.CheckPaths(configuration, 1, new string[] { basedir });
    }

    [Test]
    public void UpdateRecursivelyWithSubdirectoriesLevel1()
    {
      var basedir = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                    "Plugins");
      var subdir1 = Path.Combine(basedir, "SubDir1");
      var subdir2 = Path.Combine(basedir, "SubDir2");
      this.directories.CreateIfNotExists(basedir);
      this.directories.CreateIfNotExists(subdir1);
      this.directories.CreateIfNotExists(subdir2);

      var configuration = new Configuration();
      configuration.AddPathRecursively(basedir, 1);
      this.CheckPaths(configuration, 2, new string[] { subdir1, subdir2 });
    }

    [Test]
    public void UpdateRecursivelyWith2LvlSubdirectoriesLevel1()
    {
      var basedir = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                    "Plugins");
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

      var configuration = new Configuration();
      configuration.AddPathRecursively(basedir, 1);
      this.CheckPaths(configuration, 2, new string[] { subdir1, subdir2 });
    }

    [Test]
    public void UpdateRecursivelyWith2LvlSubdirectoriesLevel2()
    {
      var basedir = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                    "Plugins");
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

      var configuration = new Configuration();
      configuration.AddPathRecursively(basedir, 2);
      this.CheckPaths(configuration, 4, new string[] { subdir11, subdir12, subdir21, subdir22 });
    }

    [Test]
    public void UpdateEmptyRecursivelyNegativeLevel()
    {
      var configuration = new Configuration();
      Assert.Catch<ArgumentException>(() => configuration.AddPathRecursively(string.Empty, -1));
    }

    private void CheckPaths(Configuration configuration,
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
