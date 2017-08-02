using Topshelf;

namespace ExchangeReader
{
    class Program
    {
        static void Main(string[] args)
        {
            HostFactory.Run(factoryService =>
            {
                factoryService.Service<ServiceRunner>(srv =>
                {
                    srv.ConstructUsing(name => new ServiceRunner());
                    srv.WhenStarted(runner => runner.Start());
                    srv.WhenStopped(runner => runner.Stop());
                    srv.WhenContinued(runner => runner.Continue());
                    srv.WhenPaused(runner => runner.Pause());
                });

                factoryService.RunAsLocalSystem();
                factoryService.SetDescription("Exchange Web Reader Service");
                factoryService.SetDisplayName("ExchangeWebReaderService");
                factoryService.SetServiceName("ExchangeWebReaderService");
            });

            
        }
    }
}
