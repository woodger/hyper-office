using Nancy;
using Nancy.Responses;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HyperOffice.App.HttpServer
{
  class Toolkit
  {
    public static bool ValidateRequestType(string[] fileTypes, HttpFile httpFile)
    {
      return Array.Exists(fileTypes, i =>
        i == httpFile.ContentType
      );
    }

    public static StreamResponse PipeDyrectory(string dirName)
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
