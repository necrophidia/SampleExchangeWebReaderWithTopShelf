using System;
using System.Data.SqlClient;

namespace ExchangeReader.Repository
{
    public class SqlEngine
    {
        public class Server
        {
            public SqlConnection Connection { get; set; }

            public Server(string connectionString)
            {
                this.Connection = new SqlConnection(connectionString);

                try
                {
                    this.Connection.Open();
                }
                catch (Exception e)
                {
                    throw e;
                }
            }

            public void Dispose()
            {
                try
                {
                    this.Connection.Close();
                }
                catch (Exception e)
                {
                    throw e;
                }
            }

            public string ConvertToSqlDateTimeFormat(DateTime time)
            {
                return time.ToString("yyyy-MM-dd HH:mm:ss.fff");
            }
        }
    }
}
