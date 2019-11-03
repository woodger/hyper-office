using Nancy.Hosting.Self;
using System;
using System.Configuration;

namespace HyperOffice {
  class Program {
    static void Main(string[] args) {
      string address = ConfigurationManager.AppSettings.Get("address");
      string port = ConfigurationManager.AppSettings.Get("port");

      Uri uri = new Uri("http://" + address + ":" + port);

      NancyHost server = new NancyHost(uri);
      server.Start();

      Console.WriteLine("Server started on http://" + address + ":" + port);
      Console.WriteLine("Press enter to exit the application");
      Console.ReadLine();
    }
  }
}
