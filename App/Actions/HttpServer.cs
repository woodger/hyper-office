using System;
using System.Linq;
using Nancy;
using HyperOffice.App.Providers;
using HyperOffice.App.Services;
using HyperOffice.App.Toolkit;

namespace HyperOffice.App.Actions
{
  struct State
  {
    public static QueueProvider Queue;
  }

  public class HttpServer : NancyModule
  {
    public HttpServer()
    {
      Post("/api/v1/documents/snapshot", (argv) =>
      {
        var httpFile = this.Request.Files.FirstOrDefault();
        var callBack = this.Request.Form["callback"];

        if (httpFile == null || callBack == null)
        {
          return 400;
        }

        string[] fileTypes = {
          "application/msword",
          "application/vnd.openxmlformats-officedocument.wordprocessingml.document"
        };

        if (HttpRequest.ValidateRequestType(httpFile, fileTypes) == false)
        {
          return 415;
        }

        var documentService = new DocumentService(State.Queue);
        documentService.Snapshot(httpFile, callBack);

        return 202;
      });
    }
  }
}
