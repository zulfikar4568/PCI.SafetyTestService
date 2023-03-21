using Autofac;
using PCI.SafetyTestService.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCI.SafetyTestService.Driver
{
    public class Driver : Module
    {
        protected override void Load(ContainerBuilder moduleBuilder)
        {
            moduleBuilder.RegisterInstance(new FileWatcherInstance.SafetyTestFileWatcherInstance(AppSettings.SourceFolderSafetyTest, new Util.ProcessFile())).As<FileWatcherInstance.SafetyTestFileWatcherInstance>();
            moduleBuilder.RegisterInstance(new FileWatcherInstance.DailyCheckFileWatcherInstance(AppSettings.SourceFolderDailyCheck, new Util.ProcessFile())).As<FileWatcherInstance.DailyCheckFileWatcherInstance>();

            moduleBuilder.RegisterType<FileWatcher<UseCase.ISafetyTest, FileWatcherInstance.SafetyTestFileWatcherInstance>>().As<IFileWatcher<UseCase.ISafetyTest, FileWatcherInstance.SafetyTestFileWatcherInstance>>();
            moduleBuilder.RegisterType<FileWatcher<UseCase.IDailyCheck, FileWatcherInstance.DailyCheckFileWatcherInstance>>().As<IFileWatcher<UseCase.IDailyCheck, FileWatcherInstance.DailyCheckFileWatcherInstance>>();


            moduleBuilder.RegisterType<Opcenter.Helper>().AsSelf();
            moduleBuilder.RegisterType<Opcenter.ContainerTransaction>().AsSelf();
            moduleBuilder.RegisterType<Opcenter.MaintenanceTransaction>().AsSelf();
            moduleBuilder.RegisterType<Opcenter.ResourceTransaction>().AsSelf();
        }
    }
}
