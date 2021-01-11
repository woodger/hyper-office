using System;
using Nancy;

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
  }
}
