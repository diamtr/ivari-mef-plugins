using NUnit.Framework;
using System;
using System.IO;
using System.Reflection;
using IvarI.Plugins.FileSystem;
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
    public void UpdateSomeDirAndEmpty()
    {
      var configuration = new Configuration();
      configuration.AddDirectory("C:\\");
      configuration.AddDirectory(string.Empty);

      this.CheckPaths(configuration, "C:\\");
    }

    [Test]
    public void UpdateDirsDirectlyWoSubdirs()
    {
      var basedir = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                    "Tools");
      var subdir1 = Path.Combine(basedir, "SubDir1");
      var subdir2 = Path.Combine(basedir, "SubDir2");
      this.directories.CreateIfNotExists(basedir);
      this.directories.CreateIfNotExists(subdir1);
      this.directories.CreateIfNotExists(subdir2);

      var configuration = new Configuration();
      configuration.AddDirectory(basedir);
      this.CheckPaths(configuration, basedir);
      configuration.AddDirectory("C:\\");
      this.CheckPaths(configuration, basedir, "C:\\");
      configuration.AddDirectory(subdir2);
      this.CheckPaths(configuration, basedir, "C:\\", subdir2);
    }

    [Test]
    [TestCase(0)]
    [TestCase(1)]
    [TestCase(2)]
    [TestCase(3)]
    public void UpdateSubdirectoriesFromEmpty(int level)
    {
      var configuration = new Configuration();
      configuration.AddSubDirectories(string.Empty, level);
      Assert.IsEmpty(configuration.GetPaths());
    }

    [Test]
    [TestCase(1)]
    [TestCase(2)]
    [TestCase(3)]
    public void UpdateSubdirectoriesSingleDirectory(int level)
    {
      var basedir = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                    "Plugins");
      this.directories.CreateIfNotExists(basedir);

      var configuration = new Configuration();
      configuration.AddSubDirectories(basedir, level);
      Assert.IsEmpty(configuration.GetPaths());
    }

    [Test]
    public void UpdateSubdirectoriesSingleDirectoryLevel0()
    {
      var basedir = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                    "Plugins");
      this.directories.CreateIfNotExists(basedir);

      var configuration = new Configuration();
      configuration.AddSubDirectories(basedir, 0);
      this.CheckPaths(configuration, basedir);
    }

    [Test]
    public void UpdateSubdirectoriesLevel1()
    {
      var basedir = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                    "Plugins");
      var subdir1 = Path.Combine(basedir, "SubDir1");
      var subdir2 = Path.Combine(basedir, "SubDir2");
      this.directories.CreateIfNotExists(basedir);
      this.directories.CreateIfNotExists(subdir1);
      this.directories.CreateIfNotExists(subdir2);

      var configuration = new Configuration();
      configuration.AddSubDirectories(basedir, 1);
      this.CheckPaths(configuration, subdir1, subdir2);
    }

    [Test]
    public void Update2LvlSubdirectoriesLevel1()
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
      configuration.AddSubDirectories(basedir, 1);
      this.CheckPaths(configuration, subdir1, subdir2);
    }

    [Test]
    public void Update2LvlSubdirectoriesLevel2()
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
      configuration.AddSubDirectories(basedir, 2);
      this.CheckPaths(configuration, subdir11, subdir12, subdir21, subdir22);
    }

    [Test]
    public void AddDirectoryNotExistingPath()
    {
      var path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                              "Plugins", "SubDir");
      var configuration = new Configuration();
      configuration.AddDirectory(path);
      this.CheckPaths(configuration, path);
    }

    [Test]
    public void AddSubDirectoriesNotExistingPathLevel0()
    {
      var path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                              "Plugins", "SubDir");
      var configuration = new Configuration();
      configuration.AddSubDirectories(path, 0);
      this.CheckPaths(configuration, path);
    }

    [Test]
    [TestCase(1)]
    [TestCase(2)]
    [TestCase(3)]
    public void AddSubDirectoriesNotExistingPathLevelGreaterThan0(int level)
    {
      var path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                              "Plugins", "SubDir");
      var configuration = new Configuration();
      configuration.AddSubDirectories(path, level);
      Assert.IsEmpty(configuration.GetPaths());
    }

    [Test]
    public void UpdateSubdirectoriesEmptyNegativeLevel()
    {
      var configuration = new Configuration();
      Assert.Catch<ArgumentException>(() => configuration.AddSubDirectories(string.Empty, -1));
    }

    [Test]
    public void UpdateFilePath()
    {
      var filePath = @"C:\\tmp\\tmp.txt";
      var configuration = new Configuration();
      configuration.AddDirectory(filePath);
      this.CheckPaths(configuration, "C:\\tmp");
    }

    private void CheckPaths(
      Configuration configuration,
      params string[] expectedPaths)
    {
      var paths = configuration.GetPaths();
      Assert.IsNotEmpty(paths);
      Assert.AreEqual(expectedPaths.Length, paths.Count);
      foreach (var path in expectedPaths)
        Assert.AreEqual(path, paths[Array.IndexOf(expectedPaths, path)]);
    }
  }
}
