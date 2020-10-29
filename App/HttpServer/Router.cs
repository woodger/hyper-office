using System;
using System.Linq;
using Nancy;

namespace HyperOffice.App {
  public class Router : NancyModule {
    public Router() {
      this.Post("/api/v1/documents/convert", (args) => {
        HttpFile httpFile = this.Request.Files.FirstOrDefault();

        if (httpFile == null) {
          return 400;
        }

        string[] contentTypes = {
          "application/msword",
          "application/vnd.openxmlformats-officedocument.wordprocessingml.document"
        };

        bool checkType = Array.Exists(
          contentTypes,
          i => i == httpFile.ContentType
        );

        if (checkType == false) {
          return 400;
        }

        DocumentService service = new DocumentService();

        return service.ConvertWordToHtml(httpFile);
      });

      this.Post("/api/v1/documents/snapshot", (args) => {
        HttpFile httpFile = this.Request.Files.FirstOrDefault();

        if (httpFile == null) {
          return 400;
        }

        string[] contentTypes = {
          "application/msword",
          "application/vnd.openxmlformats-officedocument.wordprocessingml.document"
        };

        bool checkType = Array.Exists(
          contentTypes,
          i => i == httpFile.ContentType
        );

        if (checkType == false)
        {
          return 400;
        }

        DocumentService service = new DocumentService();
        
        return service.SnapshotWordDocument(httpFile);
      });
    }
  }
}
