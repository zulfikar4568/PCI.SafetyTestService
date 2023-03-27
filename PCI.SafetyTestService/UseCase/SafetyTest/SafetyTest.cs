using Camstar.WCF.ObjectStack;
using PCI.SafetyTestService.Config;
using PCI.SafetyTestService.Entity;
using PCI.SafetyTestService.Repository.Opcenter;
using PCI.SafetyTestService.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
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
        private readonly ContainerTransaction _containerTransaction;
        public SafetyTest(Repository.ISafetyTest repository, Util.IProcessFile processFile, ContainerTransaction containerTransaction)
        {
            _repository = repository;
            _processFile = processFile;
            _containerTransaction = containerTransaction;
        }

        public float FilterTheValue(string value)
        {
            string numberMatch = Regex.Match(value, @"\d+\.?\d*").Value;
            float result;
            if (float.TryParse(numberMatch, out result))
            {
                return result;
            } else
            {
                return 0;
            }
        }

        public void MainLogic(string delimiter, string sourceFile)
        {
            List<Entity.SafetyTest> data = _repository.Reading(delimiter, sourceFile);
            DataPointDetails[] dataPointModelling = _repository.GetDataCollectionList();
            
            // Create Data Logic
            List<Entity.SafetyTest> groundTest = new List<Entity.SafetyTest>();
            SortedDictionary<string, float> dataCollection = new SortedDictionary<string, float>();
            foreach (var item in data)
            {
                if (int.Parse(item.Step) >= 1 && int.Parse(item.Step) <= 5)
                {
                    groundTest.Add(item);
                } else if (int.Parse(item.Step) > 5 && int.Parse(item.Step) != 20 && int.Parse(item.Step) != 47)
                {
                    dataCollection.Add(item.Step, FilterTheValue(item.DataResult));
                }
            }
            if (groundTest.Count != 0)
            { 
                Entity.SafetyTest groundMax = groundTest.OrderByDescending((x) => x.DataResult).First();
                if (groundMax != null) dataCollection.Add(groundMax.Step, FilterTheValue(groundMax.DataResult));
            }


            // Modify the Data Point Object and send 
            if (dataPointModelling.Length == dataCollection.Count)
            {
                int index = 0;
                foreach (var dat in dataCollection)
                {
                    if (dataPointModelling[index] != null)
                    {
                        dataPointModelling[index].DataValue = dat.Value.ToString();
                        #if DEBUG
                            Console.WriteLine("Key: {0}, Value: {1}", dataPointModelling[index].DataName, dataPointModelling[index].DataValue);
                        #endif
                    }
                    index++;
                }

                try
                {
                    bool result = _containerTransaction.ExecuteMoveStd(data[0].Serial, "", "", AppSettings.UserDataCollectionSafetyTestName, AppSettings.UserDataCollectionSafetyTestRevision, dataPointModelling);
                    if (!result)
                    {
                        EventLogUtil.LogEvent("Retry Move Std x2", System.Diagnostics.EventLogEntryType.Information, 3);
                        result = _containerTransaction.ExecuteMoveStd(data[0].Serial, "", "", AppSettings.UserDataCollectionSafetyTestName, AppSettings.UserDataCollectionSafetyTestRevision, dataPointModelling);
                        if (!result)
                        {
                            EventLogUtil.LogEvent("Retry Move Std x3", System.Diagnostics.EventLogEntryType.Information, 3);
                            result = _containerTransaction.ExecuteMoveStd(data[0].Serial, "", "", AppSettings.UserDataCollectionSafetyTestName, AppSettings.UserDataCollectionSafetyTestRevision, dataPointModelling);
                            if (!result) MovingFileFailed(System.IO.Path.GetFileName(sourceFile), data[0].Serial);
                        }
                    }
                    if (result) EventLogUtil.LogEvent("Success when doing Transaction Move std");
                }
                catch (Exception ex)
                {
                    ex.Source = AppSettings.AssemblyName == ex.Source ? MethodBase.GetCurrentMethod().Name : MethodBase.GetCurrentMethod().Name + "." + ex.Source;
                    EventLogUtil.LogErrorEvent(ex.Source, ex.Message);
                }
                
            } else
            {
                EventLogUtil.LogEvent("The Sending MoveStd was cancelled, because the Configuration Modelling is different with the Csv file", System.Diagnostics.EventLogEntryType.Warning, 3);
            }

            dataCollection.Clear();
            MovingFileSuccess(System.IO.Path.GetFileName(sourceFile), data[0].Serial);
            if (!File.Exists(sourceFile)) _processFile.CreateEmtyCSVFile(sourceFile, new List<Entity.SafetyTest>());
        }

        private void MovingFileSuccess(string fileName, string container)
        {
            _processFile.MoveTheFile($"{AppSettings.SourceFolderSafetyTest}\\{fileName}", $"{AppSettings.TargetFolderSafetyTest}\\[{DateTime.Now:MMddyyyyhhmmsstt}]_{fileName}_{container}");
        }

        private void MovingFileFailed(string fileName, string container)
        {
            _processFile.MoveTheFile($"{AppSettings.SourceFolderSafetyTest}\\{fileName}", $"{AppSettings.FailedFolderSafetyTest}\\FAILED_[{DateTime.Now:MMddyyyyhhmmsstt}]_{fileName}_{container}");
        }
    }
}
