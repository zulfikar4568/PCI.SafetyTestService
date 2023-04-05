using PCI.SafetyTestService.Config;
using PCI.SafetyTestService.Util;
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
        public DailyCheckFileWatcherInstance(string path, IProcessFile processFile)
        {
            processFile.CheckAndCreateDirectory(path);
            Instance = new FileSystemWatcher(path);
        }
        public override string patternFile()
        {
            return AppSettings.DailyCheckFileName is null || AppSettings.DailyCheckFileName == "" ? "*.csv" : AppSettings.DailyCheckFileName;
        }
    }
}
