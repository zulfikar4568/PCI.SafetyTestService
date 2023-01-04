using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCI.SafetyTestService.Util.Opcenter
{
    class Opcenter : Module
    {
        protected override void Load(ContainerBuilder moduleBuilder)
        {
            moduleBuilder.RegisterType<ContainerTransactionUtil>().As<ContainerTransactionUtil>();
            moduleBuilder.RegisterType<HelperUtil>().As<HelperUtil>();
        }
    }
}
