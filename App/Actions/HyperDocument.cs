using System;
using System.IO;
using System.IO.Compression;
using System.Net.Http;
using System.Net.Http.Headers;
using Nancy;
using HyperOffice.App.Toolkit;
using HyperOffice.App.Providers;

namespace HyperOffice.App.Actions
{
  class HyperDocument
  {
    public async void Snapshot(string fileName, string callBackUrl)
    {
      var wordApplication = new WordApplication();
      var wordDocument = wordApplication.OpenDocument(fileName);
      var buildPath = CreateTempDirectory();

      wordDocument.SnapshotPages(buildPath);
      wordDocument.Close();
      wordApplication.Quit();

      var zipFileName = Path.Combine(buildPath, "output.zip");
      var contentType = MimeTypes.GetMimeType(zipFileName);

      ZipFile.CreateFromDirectory(buildPath, zipFileName);

      var readStream = new ZipStream(zipFileName, FileMode.Open);
      var httpContent = new StreamContent(readStream);
      
      httpContent.Headers.ContentType = new MediaTypeHeaderValue(contentType);

      var client = new HttpClient();
      var res = await client.PostAsync(callBackUrl, httpContent);

      if (res.IsSuccessStatusCode)
      {
        Directory.Delete(buildPath, true);
        File.Delete(fileName);
      }
    }

    private static string CreateTempDirectory()
    {
      var tempPath = Path.GetTempPath();
      var guidPath = Guid.NewGuid().ToString();
      var dirName = Path.Combine(tempPath, guidPath);

      Directory.CreateDirectory(dirName);

      return dirName;
    }
  }
}
