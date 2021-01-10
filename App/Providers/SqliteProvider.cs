using Microsoft.Data.Sqlite;

namespace HyperOffice.App.Providers
{
  class SqliteProvider
  {
    private SqliteConnection Connection;

    public SqliteProvider(string databaseName)
    {
      var options = string.Format(@"Data Source={0}.db",
        databaseName
      );

      this.Connection = new SqliteConnection(options);
      this.Connection.Open();
    }

    public SqliteDataReader Execute(string expression, object values = null)
    {
      var cmd = this.Connection.CreateCommand();

      cmd.CommandText = string.Format(@"{0}",
        expression
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
    }
  }
}
