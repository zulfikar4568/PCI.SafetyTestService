using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCI.SafetyTestService.Repository
{
    class Repository : Module
    {
        protected override void Load(ContainerBuilder moduleBuilder)
        {
            moduleBuilder.RegisterType<SafetyTest>().As<ISafetyTest>();
            moduleBuilder.RegisterType<DailyCheck>().As<IDailyCheck>();

            moduleBuilder.RegisterType<Opcenter.ContainerTransaction>().AsSelf();
            moduleBuilder.RegisterType<Opcenter.MaintenanceTransaction>().AsSelf();
            moduleBuilder.RegisterType<Opcenter.ResourceTransaction>().AsSelf();
        }
    }
}
