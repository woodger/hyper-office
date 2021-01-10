using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace HyperOffice.App.Providers
{
  class SqliteProvider
  {
    SqliteConnection connection;

    public SqliteProvider(string databaseName)
    {
      var param = string.Format(@"Data Source={0}.db",
        databaseName
      );

      this.connection = new SqliteConnection(param);
      this.connection.Open();
    }

    public SqliteDataReader Execute(string sql, object values = null)
    {
      var cmd = this.connection.CreateCommand();

      cmd.CommandText = string.Format(@"{0}",
        sql
      );

      if (values != null)
      {
        foreach (var item in values.GetType().GetFields())
        {
          var key = string.Format(@"${0}",
            item.Name
          );
          var value = item.GetValue(values);

          cmd.Parameters.AddWithValue(key, value);
        }
      }

      return cmd.ExecuteReader();
  
      /*
      using (var exec = cmd.ExecuteReader())
      {
        while (exec.Read())
        {
          var value = exec.GetString(0);
          Console.WriteLine(value);
        }
      }
      */
    }
  }
}
