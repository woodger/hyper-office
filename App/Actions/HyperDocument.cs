using System;
using System.IO;
using System.Net;
using HyperOffice.App.Providers;
using RestSharp;

namespace HyperOffice.App.Actions
{
  class HyperDocument
  {
    public void Snapshot(Uri uri, string fileName)
    {
      var wordApplication = new WordApplication();
      var wordDocument = wordApplication.OpenDocument(fileName);
      var buildPath = this.CreateTempDirectory();

      wordDocument.SnapshotPages(buildPath);
      wordDocument.Close();
      wordApplication.Quit();

      if (this.SendResponse(uri, buildPath))
      {
        Directory.Delete(buildPath, true);
        File.Delete(fileName);
      }
    }

    private bool SendResponse(Uri uri, string dirName)
    {
      var client = new RestClient(uri);
      
      client.RemoteCertificateValidationCallback =
        (sender, cert, chain, err) => true;

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
