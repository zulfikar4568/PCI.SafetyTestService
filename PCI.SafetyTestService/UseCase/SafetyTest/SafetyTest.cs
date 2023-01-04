using PCI.SafetyTestService.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCI.SafetyTestService.UseCase
{
    public interface ISafetyTest : Abstraction.IUseCase
    {
        new void MainLogic(string delimiter, string sourceFile);
    }
    public class SafetyTest : ISafetyTest
    {
        private Repository.ISafetyTest _repository;
        private Util.IProcessFile _processFile;
        public SafetyTest(Repository.ISafetyTest repository, Util.IProcessFile processFile)
        {
            _repository = repository;
            _processFile = processFile;
        }

        public void MainLogic(string delimiter, string sourceFile)
        {
            List<Entity.SafetyTest> data = _repository.Reading(delimiter, sourceFile);
            List<Entity.SafetyTest> groundTest = new List<Entity.SafetyTest>();
            foreach (var item in data)
            {
                if (int.Parse(item.Step) >= 1 && int.Parse(item.Step) <= 5)
                {
                    groundTest.Add(item);
                }
                Entity.SafetyTest groundMax = groundTest.OrderByDescending((x) => x.DataResult).First();
                Console.WriteLine($"{item.Step} {item.TestType} - {item.DataResult} - {item.Serial}");
            }
        }
    }
}
