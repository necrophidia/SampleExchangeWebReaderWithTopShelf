namespace ExchangeReader
{
    public static class Constants
    {
        public static string MailCredentialUsername = System.Configuration.ConfigurationManager.AppSettings["MailCredentialUsername"];
        public static string MailCredentialPassword = System.Configuration.ConfigurationManager.AppSettings["MailCredentialPassword"];
        public static string MailCredentialDomain = System.Configuration.ConfigurationManager.AppSettings["MailCredentialDomain"];
        public static string MailCredentialUrl = System.Configuration.ConfigurationManager.AppSettings["MailCredentialUrl"];

        public static string ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["EE_DB"].ConnectionString;
    }
}
