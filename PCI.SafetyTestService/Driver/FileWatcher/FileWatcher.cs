using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using PCI.SafetyTestService.Util;
using PCI.SafetyTestService.Driver.FileWatcherInstance;

namespace PCI.SafetyTestService.Driver
{
    public interface IFileWatcher<T, F>
    {
        void Init();
        void Exit();
    }
    public class FileWatcher<T, F> : IFileWatcher<T, F> where T : UseCase.Abstraction.IUseCase where F : BaseFileWatcherInstance
    {
        private readonly F _watcher;
        private readonly T _usecase;
        private bool _status = true;

        public FileWatcher(F watcher, T usecase)
        {
            _watcher = watcher;
            _usecase = usecase;
        }
        public void Init()
        {
            _watcher.Instance.NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName;
            _watcher.Instance.Changed += OnChanged;
            _watcher.Instance.Created += OnCreated;
            _watcher.Instance.Deleted += OnDeleted;
            _watcher.Instance.Renamed += OnRenamed;
            _watcher.Instance.Error += OnError;

            _watcher.Instance.Filter = "*.csv";
            _watcher.Instance.EnableRaisingEvents = true;
        }
        public void Exit()
        {
            _watcher.Instance.EnableRaisingEvents = false;
            _watcher.Instance.Dispose();
        }
        private void OnChanged(object sender, FileSystemEventArgs e)
        {
            if (e.ChangeType != WatcherChangeTypes.Changed)
            {
                return;
            }
            #if DEBUG
                Console.WriteLine($"Changed: {e.FullPath}");
            #endif
            EventLogUtil.LogEvent($"Changed: {e.FullPath}", System.Diagnostics.EventLogEntryType.Information);
            if (_status) 
            {
                _status = false;
                _status = _usecase.MainLogic(",", e.FullPath); 
            }
        }

        private void OnCreated(object sender, FileSystemEventArgs e)
        {
            EventLogUtil.LogEvent($"Created: {e.FullPath}", System.Diagnostics.EventLogEntryType.Information);
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
