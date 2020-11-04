using System;
using System.Configuration;
using Nancy.Hosting.Self;

namespace HyperOffice
{
  class Program
  {
    static void Main(string[] args)
    {
      string host = ConfigurationManager.AppSettings.Get("host");
      string port = ConfigurationManager.AppSettings.Get("port");

      string origin = string.Format(@"http://{0}:{1}",
        host,
        port
      );

      Uri uri = new Uri(origin);
      NancyHost server = new NancyHost(uri);
      server.Start();

      Console.WriteLine(
        $"Server started on {origin}\n" +
        "Press esc to exit the application"
      );

      while (Console.ReadKey().Key != ConsoleKey.Escape) { }
    }
  }
}
