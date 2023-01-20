using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCI.SafetyTestService
{
    public class ServiceMain
    {
        private readonly Driver.IFileWatcher<UseCase.ISafetyTest, Driver.FileWatcherInstance.SafetyTestFileWatcherInstance> _watcherSafetyTest;
        private readonly Driver.IFileWatcher<UseCase.IDailyCheck, Driver.FileWatcherInstance.DailyCheckFileWatcherInstance> _watcherDailyCheck;
        public ServiceMain(Driver.IFileWatcher<UseCase.ISafetyTest, Driver.FileWatcherInstance.SafetyTestFileWatcherInstance> watcherSafetyTest, Driver.IFileWatcher<UseCase.IDailyCheck, Driver.FileWatcherInstance.DailyCheckFileWatcherInstance> watcherDailyCheck)
        {
            _watcherSafetyTest = watcherSafetyTest;
            _watcherDailyCheck = watcherDailyCheck;
        }

        public void Start()
        {
            _watcherSafetyTest.Init();
            _watcherDailyCheck.Init();
        }

        public void Stop()
        {
            _watcherSafetyTest.Exit();
            _watcherDailyCheck.Exit();
        }
    }
}
