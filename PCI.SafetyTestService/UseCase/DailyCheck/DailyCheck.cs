using Camstar.WCF.ObjectStack;
using PCI.SafetyTestService.Config;
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
    public interface IDailyCheck : Abstraction.IUseCase
    {
        new bool MainLogic(string delimiter, string sourceFile);
    }
    public class DailyCheck : IDailyCheck
    {
        private readonly Repository.IDailyCheck _repository;
        private readonly Util.IProcessFile _processFile;
        private readonly ResourceTransaction _resourceTransaction;
        public DailyCheck(Repository.IDailyCheck repository, Util.IProcessFile processFile, ResourceTransaction resourceTransaction)
        {
            _repository = repository;
            _processFile = processFile;
            _resourceTransaction = resourceTransaction;
        }
        public float FilterTheValue(string value)
        {
            string numberMatch = Regex.Match(value, @"\d+\.?\d*").Value;
            float result;
            if (float.TryParse(numberMatch, out result))
            {
                return result;
            }
            else
            {
                return 0;
            }
        }

        public bool MainLogic(string delimiter, string sourceFile)
        {
            List<Entity.DailyCheck> data = _repository.Reading(delimiter, sourceFile);
            DataPointDetails[] dataPointModelling = _repository.GetDataCollectionList();

            SortedDictionary<string, float> dataCollection = new SortedDictionary<string, float>();
            foreach (var item in data)
            {
                dataCollection.Add(item.Step, FilterTheValue(item.DataResult));
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
                    bool result = _resourceTransaction.ExecuteCollectResourceData(AppSettings.ResourceName, AppSettings.UserDataCollectionDailyCheckName, AppSettings.UserDataCollectionDailyCheckRevision, dataPointModelling);
                    if (!result)
                    {
                        EventLogUtil.LogEvent("Retry Resource Data Collection x2", System.Diagnostics.EventLogEntryType.Information, 3);
                        result = _resourceTransaction.ExecuteCollectResourceData(AppSettings.ResourceName, AppSettings.UserDataCollectionDailyCheckName, AppSettings.UserDataCollectionDailyCheckRevision, dataPointModelling);
                        if (!result)
                        {
                            EventLogUtil.LogEvent("Retry Resource Data Collection x3", System.Diagnostics.EventLogEntryType.Information, 3);
                            result = _resourceTransaction.ExecuteCollectResourceData(AppSettings.ResourceName, AppSettings.UserDataCollectionDailyCheckName, AppSettings.UserDataCollectionDailyCheckRevision, dataPointModelling);
                        }
                    }
                    if (result) EventLogUtil.LogEvent("Success when doing Transaction Resource Data Collection");
                }
                catch (Exception ex)
                {
                    ex.Source = AppSettings.AssemblyName == ex.Source ? MethodBase.GetCurrentMethod().Name : MethodBase.GetCurrentMethod().Name + "." + ex.Source;
                    EventLogUtil.LogErrorEvent(ex.Source, ex.Message);
                }

            }
            else
            {
                EventLogUtil.LogEvent("The Sending Resource Data Collection was cancelled, because the Configuration Modelling is different with the Csv file", System.Diagnostics.EventLogEntryType.Warning, 3);
            }

            // Logic Opcenter must be in here
            dataCollection.Clear();
            MovingFile(System.IO.Path.GetFileName(sourceFile));
            if (!File.Exists(sourceFile)) _processFile.CreateEmtyCSVFile(sourceFile, new List<Entity.DailyCheck>());
            return true;
        }

        private void MovingFile(string fileName)
        {
            _processFile.MoveTheFile($"{AppSettings.SourceFolderDailyCheck}\\{fileName}", $"{AppSettings.TargetFolderDailyCheck}\\[{DateTime.Now:MMddyyyyhhmmsstt}]_{fileName}");
        }
    }
}