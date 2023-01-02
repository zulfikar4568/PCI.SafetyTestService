using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using PCI.SafetyTestService.Util;
using PCI.SafetyTestService.Config;

namespace PCI.SafetyTestService
{
    class Program
    {
        public static void Initialization()
        {
            AppSettings.AssemblyName = "1" + System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;
        }
        static void Main(string[] args)
        {
            Initialization();
            FileWatcher myWatcher = new FileWatcher(new FileSystemWatcher(@"C:\Temp\Logs"));
            myWatcher.Init();
        }
    }
}
