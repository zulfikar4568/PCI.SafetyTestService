using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace PCI.SafetyTestService
{
    class Program
    {
        static void Main(string[] args)
        {
            FileWatcher myWatcher = new FileWatcher(new FileSystemWatcher(@"C:\Temp\Logs"));
            myWatcher.Init();
        }
    }
}
