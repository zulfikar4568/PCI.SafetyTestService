using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using PCI.SafetyTestService.Config;

namespace PCI.SafetyTestService
{
    public class StreamFile
    {
        private readonly Driver.IFileWatcher<UseCase.ISafetyTest, Driver.FileWatcherInstance.SafetyTestFileWatcherInstance> _watcherSafetyTest;
        private readonly Driver.IFileWatcher<UseCase.IDailyCheck, Driver.FileWatcherInstance.DailyCheckFileWatcherInstance> _watcherDailyCheck;
        public StreamFile()
        {
            var containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterModule(new Driver.Driver());
            containerBuilder.RegisterModule(new Repository.Repository());
            containerBuilder.RegisterModule(new UseCase.UseCase());
            containerBuilder.RegisterModule(new Util.Util());
            containerBuilder.RegisterModule(new Util.Opcenter.Opcenter());

            var container = containerBuilder.Build();

            _watcherSafetyTest = container.Resolve<Driver.IFileWatcher<UseCase.ISafetyTest, Driver.FileWatcherInstance.SafetyTestFileWatcherInstance>>();
            _watcherDailyCheck = container.Resolve<Driver.IFileWatcher<UseCase.IDailyCheck, Driver.FileWatcherInstance.DailyCheckFileWatcherInstance>>();
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
