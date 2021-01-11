﻿using System;
using System.IO;
using System.IO.Compression;
using Nancy;
using Nancy.Responses;

namespace HyperOffice.App.Toolkit
{
  class HttpRequest
  {
    public static bool ValidateRequestType(HttpFile httpFile, string[] fileTypes)
    {
      return Array.Exists(fileTypes, (item) =>
        item == httpFile.ContentType
      );
    }

    public static StreamResponse PackFolderIntoZipStream(string dirName)
    {
      var tempPath = Path.GetTempPath();
      var guidPath = Guid.NewGuid().ToString();

      var baseName = string.Format(@"{0}.zip",
        guidPath
      );

      var zipFileName = Path.Combine(tempPath, baseName);

      ZipFile.CreateFromDirectory(dirName, zipFileName);
      Directory.Delete(dirName, true);

      var stream = new ZipStream(zipFileName, FileMode.Open);
      var contentType = MimeTypes.GetMimeType(zipFileName);

      return new StreamResponse(
        () => stream,
        contentType
      );
    }
  }
}
