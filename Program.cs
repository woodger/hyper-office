using System;
using System.Configuration;
using System.Collections.Generic;
using Nancy.Hosting.Self;
using System.Threading;
using CommandLine;

namespace HyperOffice
{
  class Program
  {
    public class Options
    {
      [Option('d', "detached", Required = false, HelpText = "Detached mode: Run application in the background")]
      public bool Detached { get; set; }
    }

    static void Main(string[] args)
    {
      string host = ConfigurationManager.AppSettings.Get("host");
      string port = ConfigurationManager.AppSettings.Get("port");

      string origin = string.Format(@"http://{0}:{1}",
        host,
        port
      );

      Uri addr = new Uri(origin);
      NancyHost server = new NancyHost(addr);

      server.Start();

      // Under mono if you daemonize a process a Console.ReadLine will cause an EOF
      // so we need to block another way

      Parser.Default.ParseArguments<Options>(args).WithParsed<Options>(o =>
      {
        if (o.Detached)
        {
          Thread.Sleep(Timeout.Infinite);
        }
        else {
          Console.WriteLine($"Server started on {origin}");
          Console.WriteLine("Press esc to exit the application");

          while (Console.ReadKey().Key != ConsoleKey.Escape) { }
        }
      });

      server.Stop();
    }
  }
}
