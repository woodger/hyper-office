using System;
using System.Configuration;
using System.IO;
using System.IO.Compression;
using Nancy;

namespace HyperOffice.App {
  class Service {
    protected string[] ContentTypes = {
      "application/msword",
      "application/vnd.openxmlformats-officedocument.wordprocessingml.document"
    };

    public bool CheckIncludesType(HttpFile httpFile) {
      return Array.Exists(
        this.ContentTypes,
        i => i == httpFile.ContentType
      );
    }

    public string SnapshotWordDocument(HttpFile httpFile) {
      string env = ConfigurationManager.AppSettings.Get("env");
      bool wordVisible = (env == "development");

      string documentFileName = TempUpload(httpFile);
      string sandboxPath = CreateGuidDirectory();
      
      WordApplication wordApplication = new WordApplication(wordVisible);
      WordDocument wordDocument = wordApplication.OpenDocument(documentFileName);

      wordDocument.SnapshotPages(sandboxPath);
      wordDocument.Close();
      wordApplication.Quit();

      string contents = TransformToZip(sandboxPath);

      File.Delete(documentFileName);
      Directory.Delete(sandboxPath, true);

      return contents;
    }

    /**
     * Трансфорать Word-документ в HTML
     */

    public string ConvertWordToHtml(HttpFile httpFile) {
      string env = ConfigurationManager.AppSettings.Get("env");
      string pageBaseName = ConfigurationManager.AppSettings.Get("page");
      
      string documentFileName = TempUpload(httpFile);
      string sandboxPath = CreateGuidDirectory();
      string pageFileName = Path.Combine(sandboxPath, pageBaseName);
      bool wordVisible = (env == "development");

      WordApplication wordApplication = new WordApplication(wordVisible);
      WordDocument wordDocument = wordApplication.OpenDocument(documentFileName);

      wordDocument.FixPageBreaks();
      wordDocument.SaveAsHtml(pageFileName);
      wordDocument.Close();
      wordApplication.Quit();

      string contents = TransformToZip(sandboxPath);

      File.Delete(documentFileName);
      Directory.Delete(sandboxPath, true);
      
      return contents;
    }

    protected static string TransformToZip(string dirName) {
      string zipFileName = CreateZipFileFromDirectory(dirName);
      string contents = File.ReadAllText(@zipFileName);

      //using (Stream stream = File.OpenWrite(zipFileName)) {
      //  return stream;
      //}

      File.Delete(zipFileName);

      return contents;
    }

    protected static string CreateGuidDirectory() {
      string tempPath = Path.GetTempPath();
      string guidPath = Guid.NewGuid().ToString();
      string dirName = Path.Combine(tempPath, guidPath);

      Directory.CreateDirectory(dirName);

      return dirName;
    }

    /**
     * Сохранить файл в Temp-директорию
     */

    protected static string TempUpload(HttpFile httpFile) {
      string tempPath = Path.GetTempPath();
      string guidPath = Guid.NewGuid().ToString();
      string extension = Path.GetExtension(httpFile.Name);

      string baseName = string.Format(@"{0}{1}",
        guidPath,
        extension
      );

      string documentFileName = Path.Combine(tempPath, baseName);

      using (var fileStream = File.OpenWrite(documentFileName)) {
        httpFile.Value.CopyTo(fileStream);
      }

      return documentFileName;
    }

    protected static string CreateZipFileFromDirectory(string dirName) {
      string tempPath = Path.GetTempPath();
      string guidPath = Guid.NewGuid().ToString();

      string baseName = string.Format(@"{0}.zip",
        guidPath
      );

      string zipFileName = Path.Combine(tempPath, baseName);
      ZipFile.CreateFromDirectory(dirName, zipFileName);

      return zipFileName;
    }
  }
}
