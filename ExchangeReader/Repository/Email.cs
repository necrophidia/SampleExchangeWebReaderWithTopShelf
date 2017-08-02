using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeReader.Repository
{
    public class Email
    {
        public static bool Exists(string subject, string senderEmail, DateTime receivedTime)
        {
            var result = false;

            var sqlEngine = new SqlEngine.Server(Constants.ConnectionString);
            using (var command = sqlEngine.Connection.CreateCommand())
            {
                string queryString = @"SELECT TOP 1 id FROM emails";
                queryString += @" WHERE sender_mail='" + senderEmail + "'";
                queryString += @" AND subject='" + subject + "'";
                queryString += @" AND received_at='" + sqlEngine.ConvertToSqlDateTimeFormat(receivedTime) + "'";

                command.CommandType = CommandType.Text;
                command.CommandText = queryString;

                try
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var value = reader.GetValue(0);
                            if (value != DBNull.Value)
                            {
                                result = true;
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

        public static bool Save(string subject, string senderEmail, string senderName, string content, DateTime receivedTime, bool isRead)
        {
            bool result = false;
            var sqlEngine = new SqlEngine.Server(Constants.ConnectionString);

            using (var command = sqlEngine.Connection.CreateCommand())
            {
                int read = 0;
                if(isRead == true)
                {
                    read = 1;
                }

                string queryString = @"INSERT INTO emails";
                queryString += @" (subject, sender_mail, sender_name, message, received_at, is_read, created_at)";
                queryString += @" VALUES('" + subject + @"','" + senderEmail + @"','" + senderName + @"','" +
                                    content + @"','" + sqlEngine.ConvertToSqlDateTimeFormat(receivedTime) + @"','" +
                                    read + @"',GETDATE())";

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
