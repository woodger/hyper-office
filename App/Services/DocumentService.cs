using HyperOffice.App.Providers;
using Nancy;
using System;
using System.IO;

namespace HyperOffice.App.Services
{
  class DocumentService
  {
    QueueProvider Queue;

    public DocumentService(QueueProvider queue)
    {
      this.Queue = queue;
    }

    public void Snapshot(HttpFile httpFile, string callBack)
    {
      var storePath = this.GetStoragePath("documents");

      Directory.CreateDirectory(storePath);

      var fileName = this.FileUpload(storePath, httpFile);
      var programm = System.AppDomain.CurrentDomain.FriendlyName;

      var arguments = string.Format(@"snapshot --input={0} --host={1}",
        fileName,
        callBack
      );

      this.Queue.Publish(programm, arguments);
    }

    public string GetStoragePath(string ctxPath = "")
    {
      var localData = Environment.SpecialFolder.LocalApplicationData;
      var localPath = Environment.GetFolderPath(localData);
      var progName = System.AppDomain.CurrentDomain.FriendlyName;
      var dotIndex = progName.LastIndexOf('.');
      
      var appPath = dotIndex > -1 ?
        progName.Substring(0, dotIndex) : progName;

      return Path.Combine(localPath, appPath, ctxPath);
    }

    protected string FileUpload(string destPath, HttpFile httpFile)
    {
      var guidPath = Guid.NewGuid().ToString();
      var extension = Path.GetExtension(httpFile.Name);

      var baseName = string.Format(@"{0}{1}",
        guidPath,
        extension
      );
      
      var fileName = Path.Combine(destPath, baseName);

      using (var fileStream = File.OpenWrite(fileName))
      {
        httpFile.Value.CopyTo(fileStream);
      }

      return fileName;
    }
  }
}
