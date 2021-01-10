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
    private int Threds;

    public QueueProvider(int threds = 1)
    {
      this.TokentRing = new List<Task>(threds);
      this.Threds = threds;

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

        if (this.TokentRing.Contains(task) == false && this.TokentRing.Count < this.Threds) {
          this.TokentRing.Add(task);
          this.Execute(task);
        }
      }
    }

    private bool Execute(Task task)
    {
      var proc = new Process();

      proc.Exited += (object sender, System.EventArgs e) => {
        this.ProcExited(task);
      };

      proc.StartInfo.FileName = task.programm;
      proc.StartInfo.Arguments = task.arguments;
      proc.StartInfo.UseShellExecute = false;
      proc.StartInfo.CreateNoWindow = true;
      proc.EnableRaisingEvents = true;

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
