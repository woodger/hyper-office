namespace HyperOffice {
  using Nancy.Hosting.Self;
  using System;
  using System.Configuration;

  class Program {
    static void Main(string[] args) {
      string address = ConfigurationManager.AppSettings.Get("address");
      string port = ConfigurationManager.AppSettings.Get("port");

      Uri uri = new Uri("http://" + address + ":" + port);

      NancyHost server = new NancyHost(uri);
      server.Start();

      Console.WriteLine(
        "Server started on http://" + address + ":" + port + "\n" +
        "Press esc to exit the application"
      );

      while (Console.ReadKey().Key != ConsoleKey.Escape) { }
    }
  }
}
