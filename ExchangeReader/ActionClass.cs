using Microsoft.Exchange.WebServices.Data;
using NLog;
using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace ExchangeReader
{
    public static class ActionClass
    {
        public static void RetrieveEmails(bool firstRun = false)
        {
            var logger = LogManager.GetCurrentClassLogger();
            var now = DateTime.Now;
            var lastFetchedEmailTime = Repository.Option.LastEmailReceived();

            try
            {
                var exchangeSvc = Engine.ExchangeWeb.Connect();
                if (exchangeSvc != null)
                {
                    var times = new TimeSpan(0, 2, 0, 0);
                    if (firstRun == true)
                    {
                        times = new TimeSpan(-100, 0, 0, 0);
                    }
                    else
                    {
                        times = lastFetchedEmailTime - now;
                    }

                    var results = Engine.ExchangeWeb.FindResults(exchangeSvc, times);
                    int index = 1;
                    foreach (Item item in results)
                    {
                        var message = EmailMessage.Bind(exchangeSvc, item.Id);
                        var receivedTime = message.DateTimeReceived;

                        if (lastFetchedEmailTime < receivedTime)
                        {
                            string subject = message.Subject;
                            string senderName = message.From.Name;
                            string senderEmail = message.From.Address;
                            string messageContent = message.Body;
                            bool isRead = message.IsRead;

                            bool emailExists = Repository.Email.Exists(subject, senderEmail, receivedTime);
                            if(emailExists == false)
                            {
                                bool saved = Repository.Email.Save(subject, senderEmail, senderName, messageContent, receivedTime, isRead);
                            }
                        }

                        if (index == results.Count())
                        {
                            Repository.Option.UpdateLastEmailReceived(receivedTime);
                        }

                        index = index + 1;
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, "error " + ex.Message + " happened with stack trace: " + ex.StackTrace);
            }
        }
    }
}
