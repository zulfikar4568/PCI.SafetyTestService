using PCI.SafetyTestService.Config;
using System;
using System.Collections;
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
            Hashtable dataCollection = new Hashtable();
            foreach (var item in data)
            {
                dataCollection[item.Step] = $"{item.TestType} - {item.DataResult}";
            }

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