using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using PCI.SafetyTestService.Config;
using PCI.SafetyTestService.Util;
using Topshelf;

namespace PCI.SafetyTestService
{
    class Program
    {
        static void Main(string[] _)
        {
            var exitCode = HostFactory.Run(x =>
            {
                AppSettings.AssemblyName = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;
                x.Service<StreamFile>(s =>
                {
                    s.ConstructUsing(streamfile => new StreamFile());
                    s.WhenStarted(streamfile => {
                        streamfile.Start();
                        EventLogUtil.LogEvent("PCI MES Safety Service started successfully", System.Diagnostics.EventLogEntryType.Information, 3);
                    });
                    s.WhenStopped(streamfile => {
                        streamfile.Stop();
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
    }
}
