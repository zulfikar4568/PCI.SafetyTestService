using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCI.SafetyTestService.UseCase
{
    class UseCase : Module
    {
        protected override void Load(ContainerBuilder moduleBuilder)
        {
            moduleBuilder.RegisterType<SafetyTest>().As<ISafetyTest>();
        }
    }
}
