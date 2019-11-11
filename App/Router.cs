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

        Service service = new Service();

        if (service.CheckIncludesType(httpFile) == false) {
          return 400;
        }

        return service.ConvertWordToHtml(httpFile);
      });

      this.Post("/api/v1/documents/snapshot", (args) => {
        HttpFile httpFile = this.Request.Files.FirstOrDefault();

        if (httpFile == null) {
          return 400;
        }

        Service service = new Service();

        if (service.CheckIncludesType(httpFile) == false) {
          return 400;
        }

        return service.SnapshotWordDocument(httpFile);
      });
    }
  }
}
