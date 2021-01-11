using System;
using Nancy.Hosting.Self;
using System.Threading;
using CommandLine;
using HyperOffice.App.Providers;
using HyperOffice.App.Actions;

/**
 * https://github.com/commandlineparser/commandline
 * 
 * Show more options:
 *    HyperOffice.exe --help
 */

namespace HyperOffice
{
  class Program
  {
    [Verb("up",
      isDefault: true,
      HelpText = "Start the Http server"
    )]
    class UpOptions
    {
      [Option('d',
        Required = false,
        HelpText = "Detached mode. Run application in the background"
      )]
      public bool Detached { get; set; }

      [Option('q', "quiet",
        Required = false,
        HelpText = "Quiet mode"
      )]
      public bool Quiet { get; set; }

      [Option('p', "port",
        Required = false,
        Default = 8080,
        HelpText = "Port of listen server"
      )]
      public int Port { get; set; }

      [Option('t', "threads",
        Required = false,
        Default = 1,
        HelpText = "Numbers worker threads of Queue. Limited by the number of CPU cores"
      )]
      public int Threads { get; set; }
    }

    [Verb("snapshot",
      HelpText = "Make screenshot in Office document"
    )]
    class SnapshotOptions
    {
      [Option("input",
        HelpText = "Microsoft Word document file"
      )]
      public string Input { get; set; }

      [Option("host",
        Required = false,
        HelpText = "Endpoint for return result"
      )]
      public string Host { get; set; }
    }

    static void Main(string[] args)
    {
      Parser.Default.ParseArguments<UpOptions, SnapshotOptions>(args)
        .WithParsed<UpOptions>(opts => HttpServer(opts))
        .WithParsed<SnapshotOptions>(opts => Snapshot(opts));
    }

    static void HttpServer(UpOptions opts)
    {
      if (opts.Port < 1024 || opts.Port > 49151)
      {
        throw new Exception("Expected User Ports (1024-49151)");
      }

      State.Queue = new QueueProvider(opts.Threads);

      var origin = string.Format(@"http://localhost:{0}",
        opts.Port
      );

      var listen = new Uri(origin);
      var server = new NancyHost(listen);

      server.Start();

      if (opts.Detached)
      {
        Thread.Sleep(Timeout.Infinite);
      }
      else
      {
        Console.WriteLine(@"Server started on {0}",
          origin
        );

        Console.WriteLine("Press esc to exit the application");

        while (Console.ReadKey().Key != ConsoleKey.Escape) { }
      }

      server.Stop();
    }

    static void Snapshot(SnapshotOptions opts)
    {
      var hyperDocument = new HyperDocument();
      hyperDocument.Snapshot(opts.Input, opts.Host);
    }
  }
}
