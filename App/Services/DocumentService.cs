using System;
using System.Configuration;
using System.IO;
using Nancy;

namespace HyperOffice.App
{
  class DocumentService
  {
    public string SnapshotWordDocument(HttpFile httpFile)
    {
      string env = ConfigurationManager.AppSettings.Get("env");
      bool wordVisible = (env == "development");

      string documentFileName = TempUpload(httpFile);
      string dirName = CreateGuidDirectory();

      WordApplication wordApplication = new WordApplication(wordVisible);
      WordDocument wordDocument = wordApplication.OpenDocument(documentFileName);

      wordDocument.SnapshotPages(dirName);
      wordDocument.Close();
      wordApplication.Quit();

      return dirName;
    }

    /**
     * Трансфорать Word-документ в HTML
     */

    public string ConvertWordToHtml(HttpFile httpFile)
    {
      string env = ConfigurationManager.AppSettings.Get("env");
      string pageBaseName = ConfigurationManager.AppSettings.Get("page");

      string documentFileName = TempUpload(httpFile);
      string dirName = CreateGuidDirectory();
      string pageFileName = Path.Combine(dirName, pageBaseName);
      bool wordVisible = (env == "development");

      WordApplication wordApplication = new WordApplication(wordVisible);
      WordDocument wordDocument = wordApplication.OpenDocument(documentFileName);

      wordDocument.FixPageBreaks();
      wordDocument.SaveAsHtml(pageFileName);
      wordDocument.Close();
      wordApplication.Quit();

      return dirName;
    }

    protected static string CreateGuidDirectory()
    {
      string tempPath = Path.GetTempPath();
      string guidPath = Guid.NewGuid().ToString();
      string dirName = Path.Combine(tempPath, guidPath);

      Directory.CreateDirectory(dirName);

      return dirName;
    }

    /**
     * Сохранить файл в Temp-директорию
     */

    protected static string TempUpload(HttpFile httpFile)
    {
      string tempPath = Path.GetTempPath();
      string guidPath = Guid.NewGuid().ToString();
      string extension = Path.GetExtension(httpFile.Name);

      string baseName = string.Format(@"{0}{1}",
        guidPath,
        extension
      );

      string documentFileName = Path.Combine(tempPath, baseName);

      using (var fileStream = File.OpenWrite(documentFileName))
      {
        httpFile.Value.CopyTo(fileStream);
      }

      return documentFileName;
    }
  }
}
