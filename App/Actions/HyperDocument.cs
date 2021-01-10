using Nancy;
using System;
using System.IO;

namespace HyperOffice.App
{
  class HyperDocument
  {
    public void Snapshot(string fileName, string callBack)
    {
      var wordApplication = new WordApplication();
      var wordDocument = wordApplication.OpenDocument(fileName);
      var tempDir = CreateTempDirectory();

      wordDocument.SnapshotPages(tempDir);
      wordDocument.Close();
      wordApplication.Quit();
    }

    private static string CreateTempDirectory()
    {
      string tempPath = Path.GetTempPath();
      string guidPath = Guid.NewGuid().ToString();
      string dirName = Path.Combine(tempPath, guidPath);

      Directory.CreateDirectory(dirName);

      return dirName;
    }
  }
}
