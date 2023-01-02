using PCI.SafetyTestService.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCI.SafetyTestService.UseCase
{
    public interface ISafetyTest
    {
        void SomeLogic(string delimiter, string sourceFile);
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

        public void SomeLogic(string delimiter, string sourceFile)
        {
          List<Entity.SafetyTest> data = _repository.Reading(delimiter, sourceFile);
            foreach (var item in data)
            {
                Console.WriteLine($"{item.Step} {item.TestType} - {item.DataResult} - {item.Serial}");
            }
        }
    }
}
