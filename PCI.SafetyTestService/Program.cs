using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using PCI.SafetyTestService.Driver;
using PCI.SafetyTestService.Config;
using PCI.SafetyTestService.Util;

namespace PCI.SafetyTestService
{
    class Program
    {
        public static void Initialization()
        {
            AppSettings.AssemblyName = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;
            EventLogUtil.LogEvent("Running The Application", System.Diagnostics.EventLogEntryType.Information, 3);
        }
        static void Main(string[] _)
        {
            Initialization();
            FileWatcher myWatcher = new FileWatcher(new FileSystemWatcher(AppSettings.SourceFolder), new UseCase.SafetyTest(new Repository.SafetyTest(), ","));
            myWatcher.Init();
        }
    }
}
