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
            containerBuilder.RegisterModule(new Driver.Driver());
            containerBuilder.RegisterModule(new Repository.Repository());
            containerBuilder.RegisterModule(new UseCase.UseCase());
            containerBuilder.RegisterModule(new Util.Util());
            containerBuilder.RegisterInstance(new FileSystemWatcher(AppSettings.SourceFolder)).AsSelf();

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
