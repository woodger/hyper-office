using System;
using System.IO;
using System.Linq;
using System.Configuration;
using System.Collections;
using Nancy;

namespace HyperOffice.App {
  public class Router : NancyModule {
    public Router() {
      this.Get("/", async (args) => {
        return "Hello World, it's Nancy on .NET Core\n";
      });

      this.Post("/api/v1/documents/convert", async (args) => {
        HttpFile file = this.Request.Files.FirstOrDefault();

        if (file == null) {
          return 400;
        }

        DocumentService documentService = new DocumentService();

        if (documentService.IncludesType(file) == false) {
          return 400;
        }

        string fileName = documentService.TempUpload(file);
        File.Delete(fileName);
      });
    }
  }
}
