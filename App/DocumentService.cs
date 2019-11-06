using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nancy;

namespace HyperOffice.App {
  class DocumentService {
    protected string[] ContentTypes = {
      "application/msword",
      "application/vnd.openxmlformats-officedocument.wordprocessingml.document"
    };

    public bool IncludesType(HttpFile file) {
      return Array.Exists(
        this.ContentTypes,
        i => i == file.ContentType
      );
    }

    public void ConvertToHtml() { }

    public string TempUpload(HttpFile file) {
      int index = Array.IndexOf(this.ContentTypes, file.ContentType);

      if (index == -1) {
        throw new ArgumentOutOfRangeException(
          "Expected Mime-types of {0}",
          this.ContentTypes.ToString()
        );
      }

      string[] extensions = { ".doc", ".docx" };
      
      string fileName = string.Format(
        @"{0}{1}{2}",
        Path.GetTempPath(),
        Guid.NewGuid(),
        extensions[index]
      );

      using (FileStream fileStream = File.Create(fileName)) {
        file.Value.CopyTo(fileStream);
      }

      return fileName;
    }
  }
}
