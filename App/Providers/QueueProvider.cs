using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace HyperOffice.App.Providers
{
  struct Task
  {
    public string programm;
    public string arguments;
  }

  class QueueProvider
  {
    private SqliteProvider Sqlite = new SqliteProvider("Sqlite");
    private List<Task> TokentRing;
    private int Threads;

    public QueueProvider(int threads = 1)
    {
      var cores = Environment.ProcessorCount;

      if (threads < 1 || threads > cores)
      {
        var errorMessage = string.Format(@"Number of worker threads exceeded (1, {0})",
          cores
        );

        throw new Exception(errorMessage);
      }

      this.TokentRing = new List<Task>(threads);
      this.Threads = threads;

      this.Sqlite.Execute(
        @"CREATE TABLE IF NOT EXISTS `queue` (" +
          "`programm` text," +
          "`arguments` text" +
        ")"
      );
    }

    public void Publish(string programm, string arguments = "")
    {
      var task = new Task
      {
        programm = programm,
        arguments = arguments
      };

      this.Sqlite.Execute(
        @"INSERT INTO `queue` (`programm`, `arguments`) VALUES ($programm, $arguments)",
        task
      );

      this.Notificate();
    }

    public void Notificate()
    {
      var cursor = this.Sqlite.Execute(
        @"SELECT * FROM `queue`"
      );

      while (cursor.Read())
      {
        var task = new Task
        {
          programm = cursor.GetString(0),
          arguments = cursor.GetString(1)
        };

        if (this.TokentRing.Contains(task) == false && this.TokentRing.Count < this.Threads) {
          if (this.Execute(task))
          {
            this.TokentRing.Add(task);
          }
        }
      }
    }

    private bool Execute(Task task)
    {
      var proc = new Process();

      proc.StartInfo.FileName = task.programm;
      proc.StartInfo.Arguments = task.arguments;
      proc.StartInfo.UseShellExecute = false;
      proc.StartInfo.CreateNoWindow = true;
      proc.EnableRaisingEvents = true;

      proc.Exited += (object sender, System.EventArgs e) => {
        this.ProcExited(task);
      };

      return proc.Start();
    }

    private void ProcExited(Task task)
    {
      this.Sqlite.Execute(
        @"DELETE FROM `queue` WHERE `programm` = $programm AND `arguments` = $arguments",
        task
      );

      this.TokentRing.Remove(task);
      this.Notificate();
    }
  }
}
