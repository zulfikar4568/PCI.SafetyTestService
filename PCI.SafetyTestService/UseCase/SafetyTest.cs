using PCI.SafetyTestService.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCI.SafetyTestService.UseCase
{
    public class SafetyTest
    {
        private Repository.SafetyTest _repository;
        private string _delimiter;
        public SafetyTest(Repository.SafetyTest repository, string delimiter)
        {
            _repository = repository;
            _delimiter = delimiter;
        }

        public void SomeLogic(string sourceFile)
        {
          List<Entity.SafetyTest> data = _repository.Reading(_delimiter, sourceFile);
            foreach (var item in data)
            {
                Console.WriteLine($"{item.Step} {item.TestType} - {item.DataResult} - {item.Serial}");
            }
        }
    }
}
