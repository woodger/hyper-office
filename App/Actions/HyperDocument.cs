using System;
using System.IO;
using HyperOffice.App.Providers;
using RestSharp;

namespace HyperOffice.App.Actions
{
  class HyperDocument
  {
    public void Snapshot(string url, string fileName)
    {
      var wordApplication = new WordApplication();
      var wordDocument = wordApplication.OpenDocument(fileName);
      var buildPath = this.CreateTempDirectory();

      wordDocument.SnapshotPages(buildPath);
      wordDocument.Close();
      wordApplication.Quit();

      if (SendResponse(url, buildPath))
      {
        Directory.Delete(buildPath, true);
        File.Delete(fileName);
      }
    }

    private bool SendResponse(string url, string dirName)
    {
      var client = new RestClient(url);
      var req = new RestRequest();

      foreach (var item in Directory.GetFiles(dirName))
      {
        string fileName = Path.GetFileName(item);
        req.AddFile(fileName, item);
      }

      return client.Post(req).IsSuccessful;
    }

    private string CreateTempDirectory()
    {
      var tempPath = Path.GetTempPath();
      var guidPath = Guid.NewGuid().ToString();
      var dirName = Path.Combine(tempPath, guidPath);

      Directory.CreateDirectory(dirName);

      return dirName;
    }
  }
}
