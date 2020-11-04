using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using HyperOffice.App.HttpServer;
using Nancy;
using Nancy.Responses;

namespace HyperOffice.App
{
  public class Router : NancyModule
  {
    public Router()
    {
      this.Post("/api/v1/word/convert", (argv) => {
        HttpFile httpFile = this.Request.Files.FirstOrDefault();

        Console.WriteLine("Post: convert");

        if (httpFile == null)
        {
          return 400;
        }

        string[] fileTypes = {
          "application/msword",
          "application/vnd.openxmlformats-officedocument.wordprocessingml.document"
        };

        if (this.ValidateRequestType(fileTypes, httpFile) == false)
        {
          return 415;
        }

        DocumentService service = new DocumentService();
        string dirName = service.ConvertWordToHtml(httpFile);

        if (dirName == null)
        {
          return 406;
        }

        return this.PipeDyrectory(dirName);
      });

      this.Post("/api/v1/word/snapshot", (argv) => {
        HttpFile httpFile = this.Request.Files.FirstOrDefault();

        if (httpFile == null)
        {
          return 400;
        }

        string[] fileTypes = {
          "application/msword",
          "application/vnd.openxmlformats-officedocument.wordprocessingml.document"
        };

        if (this.ValidateRequestType(fileTypes, httpFile) == false)
        {
          return 415;
        }

        DocumentService service = new DocumentService();

        string dirName = service.SnapshotWordDocument(httpFile);

        if (dirName == null)
        {
          return 406;
        }

        return this.PipeDyrectory(dirName);
      });
    }

    private bool ValidateRequestType(string[] fileTypes, HttpFile httpFile)
    {
      return Array.Exists(fileTypes, i =>
        i == httpFile.ContentType
      );
    }

    private StreamResponse PipeDyrectory(string dirName)
    {
      string tempPath = Path.GetTempPath();
      string guidPath = Guid.NewGuid().ToString();

      string baseName = string.Format(@"{0}.zip", guidPath);
      string zipFileName = Path.Combine(tempPath, baseName);

      ZipFile.CreateFromDirectory(dirName, zipFileName);
      Directory.Delete(dirName, true);

      ZipStream stream = new ZipStream(zipFileName, FileMode.Open);
      string contentType = MimeTypes.GetMimeType(zipFileName);

      return new StreamResponse(
        () => stream,
        contentType
      );
    }
  }
}
