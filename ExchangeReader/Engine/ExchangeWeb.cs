using Microsoft.Exchange.WebServices.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeReader.Engine
{
    public class ExchangeWeb
    {
        public static ExchangeService Connect()
        {
            var exchangeSvc = new ExchangeService(ExchangeVersion.Exchange2013_SP1);

            exchangeSvc.Credentials = new WebCredentials(Constants.MailCredentialUsername, 
                Constants.MailCredentialPassword, Constants.MailCredentialDomain);

            exchangeSvc.Url = new Uri(Constants.MailCredentialUrl);

            ServicePointManager.ServerCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;

            return exchangeSvc;
        }

        public static FindItemsResults<Item> FindResults(ExchangeService exchange, TimeSpan times)
        {
            var date = DateTime.Now.Add(times);

            var filter = new SearchFilter.IsGreaterThanOrEqualTo(ItemSchema.DateTimeReceived, date);

            var result = exchange.FindItems(WellKnownFolderName.Inbox, filter, new ItemView(50));

            return result;
        }
    }
}
