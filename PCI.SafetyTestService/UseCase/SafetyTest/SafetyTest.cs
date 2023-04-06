using Camstar.WCF.ObjectStack;
using PCI.SafetyTestService.Config;
using PCI.SafetyTestService.Entity;
using PCI.SafetyTestService.Repository.Opcenter;
using PCI.SafetyTestService.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
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

        public Dictionary<int, float> GetLogValue(List<Entity.SafetyTest> CompletedData)
        {
            Dictionary<int, float> results = new Dictionary<int, float>();
            if (CompletedData.Count > 0)
            {
                foreach (var data in CompletedData)
                {
                    bool validateKey = int.TryParse(data.Step, out int isKeyOk);
                    bool validateValue = float.TryParse(data.Value, out float isValueOk);
                    if (data.Value != null && data.Value != "" &&  validateKey && validateValue)
                    {
                        results.Add(isKeyOk, isValueOk);
                    }
                }
            }
            return results;
        }

        private DataPointDetails[] CombineDataPoint(Dictionary<int, float> LogValue, Dictionary<int, DataPointDetails> ModellingValue)
        {
            List<DataPointDetails> dataPointDetails = new List<DataPointDetails>();
            foreach (KeyValuePair<int, DataPointDetails> dataModel in ModellingValue)
            {
                if (LogValue.ContainsKey(dataModel.Key))
                {
                    var dataFill = dataModel.Value;
                    dataFill.DataValue = LogValue[dataModel.Key].ToString();
                    dataPointDetails.Add(dataFill);
                }
            }
            return dataPointDetails.ToArray();
        }

        public void MainLogic(string delimiter, string sourceFile)
        {
            List<Entity.SafetyTest> data = _repository.Reading(delimiter, sourceFile);
            var serialNumber = System.IO.Path.GetFileNameWithoutExtension(sourceFile);
            var dataPointDetails = CombineDataPoint(GetLogValue(data), _repository.GetDataCollectionList());

            if (dataPointDetails.Length > 0)
            {
                try
                {
                    bool result = _containerTransaction.ExecuteCollectData(serialNumber, AppSettings.UserDataCollectionSafetyTestName, AppSettings.UserDataCollectionSafetyTestRevision, dataPointDetails);
                    if (!result)
                    {
                        EventLogUtil.LogEvent("Retry Collect Data x2", System.Diagnostics.EventLogEntryType.Information, 3);
                        result = _containerTransaction.ExecuteCollectData(serialNumber, AppSettings.UserDataCollectionSafetyTestName, AppSettings.UserDataCollectionSafetyTestRevision, dataPointDetails);
                        if (!result)
                        {
                            EventLogUtil.LogEvent("Retry Collect Data x3", System.Diagnostics.EventLogEntryType.Information, 3);
                            result = _containerTransaction.ExecuteCollectData(serialNumber, AppSettings.UserDataCollectionSafetyTestName, AppSettings.UserDataCollectionSafetyTestRevision, dataPointDetails);
                            if (!result) MovingFileFailed(System.IO.Path.GetFileName(sourceFile));
                        }
                    }
                    if (result) EventLogUtil.LogEvent("Success when doing Transaction Collect Data");
                }
                catch (Exception ex)
                {
                    ex.Source = AppSettings.AssemblyName == ex.Source ? MethodBase.GetCurrentMethod().Name : MethodBase.GetCurrentMethod().Name + "." + ex.Source;
                    EventLogUtil.LogErrorEvent(ex.Source, ex.Message);
                }
                MovingFileSuccess(System.IO.Path.GetFileName(sourceFile));
            } else
            {
                EventLogUtil.LogEvent("There's no data Model match!", System.Diagnostics.EventLogEntryType.Warning, 3);
                MovingFileFailed(System.IO.Path.GetFileName(sourceFile));
            }
        }

        private void MovingFileSuccess(string fileName)
        {
            _processFile.MoveTheFile($"{AppSettings.SourceFolderSafetyTest}\\{fileName}", $"{AppSettings.TargetFolderSafetyTest}\\[{DateTime.Now:MMddyyyyhhmmsstt}]_{fileName}");
        }

        private void MovingFileFailed(string fileName)
        {
            _processFile.MoveTheFile($"{AppSettings.SourceFolderSafetyTest}\\{fileName}", $"{AppSettings.FailedFolderSafetyTest}\\FAILED_[{DateTime.Now:MMddyyyyhhmmsstt}]_{fileName}");
        }
    }
}
