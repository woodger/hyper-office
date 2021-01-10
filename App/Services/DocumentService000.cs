using System;
using System.Configuration;
using System.IO;
using Nancy;

namespace HyperOffice.App
{
  class DocumentService000
  {
    public string SnapshotWordDocument(HttpFile httpFile)
    {
      string documentFileName = TempUpload(httpFile);
      string dirName = CreateGuidDirectory();

      WordApplication wordApplication = new WordApplication();
      WordDocument wordDocument = wordApplication.OpenDocument(documentFileName);

      wordDocument.SnapshotPages(dirName);
      wordDocument.Close();
      wordApplication.Quit();

      return dirName;
    }

    public string ConvertWordToHtml(HttpFile httpFile)
    {
      string pageBaseName = ConfigurationManager.AppSettings.Get("page");

      string documentFileName = TempUpload(httpFile);
      string dirName = CreateGuidDirectory();
      string pageFileName = Path.Combine(dirName, pageBaseName);

      WordApplication wordApplication = new WordApplication();
      WordDocument wordDocument = wordApplication.OpenDocument(documentFileName);

      wordDocument.FixPageBreaks();
      wordDocument.SaveAsHtml(pageFileName);
      wordDocument.Close();
      wordApplication.Quit();

      return dirName;
    }

    public string DocumentInfo(HttpFile httpFile)
    {
      return "";
    }

    protected static string CreateGuidDirectory()
    {
      string tempPath = Path.GetTempPath();
      string guidPath = Guid.NewGuid().ToString();
      string dirName = Path.Combine(tempPath, guidPath);

      Directory.CreateDirectory(dirName);

      return dirName;
    }

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
