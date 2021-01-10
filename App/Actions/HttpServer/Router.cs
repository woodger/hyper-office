using System;
using System.Linq;
using HyperOffice.App.HttpServer;
using HyperOffice.App.Providers;
using HyperOffice.App.Services;
using Nancy;

namespace HyperOffice.App
{
  struct State
  {
    public static QueueProvider Queue = new QueueProvider(2);
  }

  public class Router : NancyModule
  {
    public static void UpContext() {
      State.Queue.Notificate();
    }

    public Router()
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

        if (Toolkit.ValidateRequestType(httpFile, fileTypes) == false)
        {
          return 415;
        }

        var documentService = new DocumentService(State.Queue);
        documentService.Snapshot(httpFile, callBack);

        return 202;
      });

      /*
      Post("/api/v1/documents/convert", (argv) => {
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
        string dirName = service.ConvertWordToHtml(httpFile);

        if (dirName == null)
        {
          return 406;
        }

        return this.PipeDyrectory(dirName);
      });

      Post("/api/v1/documents/info", (argv) => {
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

        string dirName = service.DocumentInfo(httpFile);

        if (dirName == null)
        {
          return 406;
        }

        return this.PipeDyrectory(dirName);
      });
      */
    }
  }
}
