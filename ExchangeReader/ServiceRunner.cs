using System.Timers;

namespace ExchangeReader
{
    public class ServiceRunner
    {
        readonly Timer _timer;
        public ServiceRunner()
        {
            _timer = new Timer(600000) { AutoReset = true };
            _timer.Elapsed += ElapsedAction;
            _timer.Start();
        }

        public void Start()
        {
            ActionClass.RetrieveEmails(true);
            _timer.Start();
        }

        public void Stop()
        {
            _timer.Stop();
        }

        public void Continue()
        {
            ActionClass.RetrieveEmails();
            _timer.Start();
        }

        public void Pause()
        {
            _timer.Stop();
        }

        private void ElapsedAction(object sender, ElapsedEventArgs elapsedEventArgs)
        {
            ActionClass.RetrieveEmails();
        }
    }
}
