using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCI.SafetyTestService.Driver.FileWatcherInstance
{
    public abstract class BaseFileWatcherInstance
    {
        public FileSystemWatcher Instance;
        public abstract string patternFile();
    }
}
