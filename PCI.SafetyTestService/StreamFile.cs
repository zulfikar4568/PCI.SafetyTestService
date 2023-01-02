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
        private readonly Driver.FileWatcher _watcher;
        public StreamFile()
        {
            var containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterType<UseCase.SafetyTest>().As<UseCase.ISafetyTest>();
            containerBuilder.RegisterType<Repository.SafetyTest>().As<Repository.ISafetyTest>();
            containerBuilder.RegisterType<Util.ProcessFile>().As<Util.IProcessFile>();
            containerBuilder.RegisterInstance(new FileSystemWatcher(AppSettings.SourceFolder)).AsSelf();
            containerBuilder.RegisterType<Driver.FileWatcher>().AsSelf();

            var container = containerBuilder.Build();

            _watcher = container.Resolve<Driver.FileWatcher>();
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
