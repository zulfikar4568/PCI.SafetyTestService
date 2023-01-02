using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PCI.SafetyTestService.Config;
using PCI.SafetyTestService.Driver;

namespace PCI.SafetyTestService
{
    public class StreamFile
    {
        private readonly FileWatcher _watcher;
        public StreamFile()
        {
            _watcher = new FileWatcher(new FileSystemWatcher(AppSettings.SourceFolder), new UseCase.SafetyTest(new Repository.SafetyTest(), ","));
        }

        public void Start()
        {
            _watcher.Init();
        }

        public void Stop()
        {
            _watcher.Exit();
        }
    }
}
