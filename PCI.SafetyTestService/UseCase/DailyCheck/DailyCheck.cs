using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCI.SafetyTestService.UseCase
{
    public interface IDailyCheck : Abstraction.IUseCase
    {
        new void MainLogic(string delimiter, string sourceFile);
    }
    public class DailyCheck : IDailyCheck
    {
        private Repository.IDailyCheck _repository;
        private Util.IProcessFile _processFile;
        public DailyCheck(Repository.IDailyCheck repository, Util.IProcessFile processFile)
        {
            _repository = repository;
            _processFile = processFile;
        }
    
        public void MainLogic(string delimiter, string sourceFile)
        {
            List<Entity.DailyCheck> data = _repository.Reading(delimiter, sourceFile);
            foreach (var item in data)
            {
                Console.WriteLine($"{item.Step} {item.TestType} - {item.DataResult} - {item.Serial}");
            }
        }
    }

}
