using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCI.SafetyTestService.Driver.FileWatcherInstance
{
    public class DailyCheckFileWatcherInstance : BaseFileWatcherInstance
    {
        public DailyCheckFileWatcherInstance(string path)
        {
            Instance = new FileSystemWatcher(path);
        }
    }
}
