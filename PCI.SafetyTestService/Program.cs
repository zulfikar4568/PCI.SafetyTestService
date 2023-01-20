using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using PCI.SafetyTestService.Config;
using PCI.SafetyTestService.Util;
using Topshelf;
using Autofac;
using System.Collections.Specialized;
using System.Security.Cryptography;

namespace PCI.SafetyTestService
{
    class Program
    {
        static void Main(string[] _)
        {
            // Setup DI
            var containerBuilder = DependendyInjectionBuilder(new ContainerBuilder());
            var container = containerBuilder.Build();

            #if DEBUG
                Console.WriteLine("Start TopSelf");
            #endif
            var exitCode = HostFactory.Run(x =>
            {
                AppSettings.AssemblyName = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;
                x.Service<ServiceMain>(s =>
                {
                    s.ConstructUsing(service => container.Resolve<ServiceMain>());
                    s.WhenStarted(service => {
                        service.Start();
                        EventLogUtil.LogEvent("PCI MES Safety Service started successfully", System.Diagnostics.EventLogEntryType.Information, 3);
                    });
                    s.WhenStopped(service => {
                        service.Stop();
                        EventLogUtil.LogEvent("PCI MES Safety Service stopped successfully", System.Diagnostics.EventLogEntryType.Information, 3);
                    });
                });
                x.RunAsLocalSystem();
                x.SetServiceName("PCI MES Safety Service");
                x.SetDisplayName("PCI MES Safety Service");
                x.SetDescription("This is service for MES PCI in Safety Tester");
            });

            int exitCodeValue = (int)Convert.ChangeType(exitCode, exitCode.GetTypeCode());
            Environment.ExitCode = exitCodeValue;
        }
        private static ContainerBuilder DependendyInjectionBuilder(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterModule(new Driver.Driver());
            containerBuilder.RegisterModule(new Repository.Repository());
            containerBuilder.RegisterModule(new UseCase.UseCase());
            containerBuilder.RegisterModule(new Util.Util());
            containerBuilder.RegisterType<ServiceMain>().As<ServiceMain>();

            return containerBuilder;
        }
    }
}
