using Camstar.WCF.ObjectStack;
using CsvHelper;
using CsvHelper.Configuration;
using PCI.SafetyTestService.Config;
using PCI.SafetyTestService.Entity;
using PCI.SafetyTestService.Repository.Opcenter;
using PCI.SafetyTestService.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PCI.SafetyTestService.Repository
{
    public interface ISafetyTest
    {
        List<Entity.SafetyTest> Reading(string delimiter, string sourceFile);
        Dictionary<int, DataPointDetails> GetDataCollectionList();
    }
    public class SafetyTest : ISafetyTest
    {
        private readonly MaintenanceTransaction _maintenanceTransaction;
        public SafetyTest(MaintenanceTransaction maintenanceTransaction) 
        {
            _maintenanceTransaction = maintenanceTransaction;
        }
        public List<Entity.SafetyTest> Reading(string delimiter, string sourceFile)
        {
            List<Entity.SafetyTest> result = new List<Entity.SafetyTest>();
            var configuration = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Encoding = Encoding.UTF8, // Our file uses UTF-8 encoding.
                Delimiter = delimiter,
            };

            configuration.MissingFieldFound = (missingField) =>
            {
                EventLogUtil.LogEvent($"There's missing data field in column index {missingField.Index} {missingField.HeaderNames} was not found! you can ignore.", System.Diagnostics.EventLogEntryType.Warning, 6);
            };

            configuration.BadDataFound = (badData) =>
            {
                EventLogUtil.LogEvent($"Bad data at {badData.RawRecord}, {badData.Field}, {badData.Context}", System.Diagnostics.EventLogEntryType.Warning, 6);
            };
            
            try
            {

                using (var reader = new StreamReader(sourceFile))
                using (var csv = new CsvReader(reader, configuration))
                {
                    var records = csv.GetRecords<Entity.SafetyTest>();
                    result = records.ToList();
                }
            }
            catch (Exception ex)
            {
                ex.Source = AppSettings.AssemblyName == ex.Source ? MethodBase.GetCurrentMethod().Name : MethodBase.GetCurrentMethod().Name + "." + ex.Source;
                EventLogUtil.LogErrorEvent(ex.Source, ex);
            }

            return result;
        }

        public Dictionary<int, DataPointDetails> GetDataCollectionList()
        {
            Dictionary<int, DataPointDetails> results = new Dictionary<int, DataPointDetails>();
            var data = _maintenanceTransaction.GetUserDataCollectionDef(AppSettings.UserDataCollectionSafetyTestName, AppSettings.UserDataCollectionSafetyTestRevision);
            if (data != null)
            {
                foreach (var dataPoint in data.DataPoints)
                {
                    bool validateKey = int.TryParse(dataPoint.Name.ToString().Split('|')[0], out int isKeyOk);
                    if (validateKey)
                    {
                        results.Add(isKeyOk, new DataPointDetails() { DataName = dataPoint.Name.ToString(), DataType = dataPoint.DataType });
                    }
                }
            }
            return results;
        }
    }
}
