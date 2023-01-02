using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using PCI.SafetyTestService.Util;

namespace PCI.SafetyTestService.Driver
{
    class FileWatcher
    {
        private readonly FileSystemWatcher _watcher;
        private readonly UseCase.ISafetyTest _usecase;
        public FileWatcher(FileSystemWatcher watcher, UseCase.ISafetyTest usecase)
        {
            _watcher = watcher;
            _usecase = usecase;
        }
        public void Init()
        {
            _watcher.NotifyFilter = NotifyFilters.Attributes
                                 | NotifyFilters.CreationTime
                                 | NotifyFilters.DirectoryName
                                 | NotifyFilters.FileName
                                 | NotifyFilters.LastAccess
                                 | NotifyFilters.LastWrite
                                 | NotifyFilters.Security
                                 | NotifyFilters.Size;
            _watcher.Changed += OnChanged;
            _watcher.Created += OnCreated;
            _watcher.Deleted += OnDeleted;
            _watcher.Renamed += OnRenamed;
            _watcher.Error += OnError;

            _watcher.Filter = "*.csv";
            _watcher.IncludeSubdirectories = true;
            _watcher.EnableRaisingEvents = true;
        }
        public void Exit()
        {
            _watcher.EnableRaisingEvents = false;
            _watcher.Dispose();
        }
        private void OnChanged(object sender, FileSystemEventArgs e)
        {
            if (e.ChangeType != WatcherChangeTypes.Changed)
            {
                return;
            }
            EventLogUtil.LogEvent($"Changed: {e.FullPath}", System.Diagnostics.EventLogEntryType.Information);
            _usecase.SomeLogic(",", e.FullPath);
        }

        private void OnCreated(object sender, FileSystemEventArgs e)
        {
            EventLogUtil.LogEvent($"Created: {e.FullPath}", System.Diagnostics.EventLogEntryType.Information);
            _usecase.SomeLogic(",", e.FullPath);
        }

        private void OnDeleted(object sender, FileSystemEventArgs e)
        {
            EventLogUtil.LogEvent($"Deleted: {e.FullPath}", System.Diagnostics.EventLogEntryType.Information);
        }

        private void OnRenamed(object sender, RenamedEventArgs e)
        {
            EventLogUtil.LogEvent($"Renamed: \n Old: {e.OldFullPath} \n New: {e.FullPath}", System.Diagnostics.EventLogEntryType.Information);
        }

        private void OnError(object sender, ErrorEventArgs e) =>
            PrintException(e.GetException());

        private void PrintException(Exception ex)
        {
            if (ex != null)
            {
                Console.WriteLine($"Message: {ex.Message}");
                Console.WriteLine("Stacktrace:");
                Console.WriteLine(ex.StackTrace);
                Console.WriteLine();
                PrintException(ex.InnerException);
            }
        }
    }
}
