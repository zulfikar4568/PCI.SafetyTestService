using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCI.SafetyTestService.UseCase.Abstraction
{
    public interface IUseCase
    {
        bool MainLogic(string delimiter, string sourceFile);
    }
}
