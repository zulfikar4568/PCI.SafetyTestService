using PCI.SafetyTestService.Config;
using System;
using System.Collections;
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
        private readonly Repository.ISafetyTest _repository;
        private readonly Util.IProcessFile _processFile;
        public SafetyTest(Repository.ISafetyTest repository, Util.IProcessFile processFile)
        {
            _repository = repository;
            _processFile = processFile;
        }

        public void MainLogic(string delimiter, string sourceFile)
        {
            List<Entity.SafetyTest> data = _repository.Reading(delimiter, sourceFile);
            List<Entity.SafetyTest> groundTest = new List<Entity.SafetyTest>();
            Hashtable dataCollection = new Hashtable();
            foreach (var item in data)
            {
                if (int.Parse(item.Step) >= 1 && int.Parse(item.Step) <= 5)
                {
                    groundTest.Add(item);
                } else if (int.Parse(item.Step) > 5 && int.Parse(item.Step) != 20 && int.Parse(item.Step) != 47)
                {
                    dataCollection[item.Step] = $"{item.TestType} - {item.DataResult}";
                }
            }
            Entity.SafetyTest groundMax = groundTest.OrderByDescending((x) => x.DataResult).First();
            dataCollection[groundMax.Step] = $"{groundMax.TestType} - {groundMax.DataResult}";

            foreach (DictionaryEntry dat in dataCollection)
            {
                Console.WriteLine("Key: {0}, Value: {1}", dat.Key, dat.Value);
            }

            MovingFile(System.IO.Path.GetFileName(sourceFile));
        }

        private void MovingFile(string fileName)
        {
            _processFile.MoveTheFile($"{AppSettings.SourceFolderSafetyTest}\\{fileName}", $"{AppSettings.TargetFolderSafetyTest}\\{fileName}");
        }
    }
}
