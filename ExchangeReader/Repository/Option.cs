using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeReader.Repository
{
    public class Option
    {
        public static DateTime LastEmailReceived()
        {
            var result = DateTime.MinValue;

            var sqlEngine = new SqlEngine.Server(Constants.ConnectionString);
            using (var command = sqlEngine.Connection.CreateCommand())
            {
                string queryString = @"SELECT TOP 1 option_value FROM options WHERE option_key='email_last_received'";

                command.CommandType = CommandType.Text;
                command.CommandText = queryString;

                try
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var value = reader.GetValue(0);
                            if(value != DBNull.Value)
                            {
                                result = DateTime.Parse(value.ToString());
                            }
                        }
                    }
                }
                catch (SqlException ex)
                {
                    throw ex;
                }
            }

            sqlEngine.Dispose();

            return result;
        }

        public static bool UpdateLastEmailReceived(DateTime newTime)
        {
            bool result = false;
            var sqlEngine = new SqlEngine.Server(Constants.ConnectionString);

            using (var command = sqlEngine.Connection.CreateCommand())
            {
                string queryString = @"UPDATE options SET option_value='" + newTime.ToString() + 
                    "' WHERE option_key='email_last_received'";

                command.CommandType = CommandType.Text;
                command.CommandText = queryString;

                using (var transaction = sqlEngine.Connection.BeginTransaction(IsolationLevel.ReadCommitted))
                {
                    try
                    {
                        command.Transaction = transaction;
                        int rowAffected = command.ExecuteNonQuery();
                        transaction.Commit();
                        result = rowAffected > 0;
                    }
                    catch (SqlException ex)
                    {
                        transaction.Rollback();
                        throw ex;
                    }
                }
            }

            return result;
        }
    }
}
