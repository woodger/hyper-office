using System.Linq;
using HyperOffice.App.HttpServer;
using HyperOffice.App.Providers;
using Nancy;

namespace HyperOffice.App
{
  public class Router : NancyModule
  {
    public Router()
    {
      Post("/api/v1/documents/snapshot", (argv) =>
      {
        HttpFile httpFile = this.Request.Files.FirstOrDefault();

        if (httpFile == null)
        {
          return 400;
        }

        string[] fileTypes = {
          "application/msword",
          "application/vnd.openxmlformats-officedocument.wordprocessingml.document"
        };

        if (Toolkit.ValidateRequestType(fileTypes, httpFile) == false)
        {
          return 415;
        }

        /*
        bool res = Queue.Publish();

        if (res == false) {
          return 500;
        }
        */

        return 202;

        /*        DocumentService service = new DocumentService();

                string dirName = service.SnapshotWordDocument(httpFile);

                if (dirName == null)
                {
                  return 406;
                }

                return Toolkit.PipeDyrectory(dirName);*/
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
