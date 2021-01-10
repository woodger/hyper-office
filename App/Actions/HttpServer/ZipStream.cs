using System.IO;

namespace HyperOffice.App.HttpServer
{
  internal class ZipStream : FileStream
  {
    public ZipStream(string fileName, FileMode fileMode) : base(fileName, fileMode) { }

    public override void Close()
    {
      this.Dispose(true);
      File.Delete(this.Name);
    }
  }
}
