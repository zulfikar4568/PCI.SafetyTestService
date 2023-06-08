using Camstar.WCF.ObjectStack;
using CsvHelper;
using CsvHelper.Configuration;
using PCI.SafetyTestService.Config;
using PCI.SafetyTestService.Repository.Opcenter;
using PCI.SafetyTestService.Util;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PCI.SafetyTestService.Repository
{
    public interface IDailyCheck
    {
        List<Entity.DailyCheck> Reading(string delimiter, string sourceFile);
        DataPointDetails[] GetDataCollectionList();
    }
    public class DailyCheck : IDailyCheck
    {
        private readonly MaintenanceTransaction _maintenanceTransaction;
        public DailyCheck(MaintenanceTransaction maintenanceTransaction)
        {
            _maintenanceTransaction = maintenanceTransaction;
        }
        public List<Entity.DailyCheck> Reading(string delimiter, string sourceFile)
        {
            List<Entity.DailyCheck> result = new List<Entity.DailyCheck>();
            var configuration = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Encoding = Encoding.UTF8, // Our file uses UTF-8 encoding.
                Delimiter = delimiter
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
                    var records = csv.GetRecords<Entity.DailyCheck>();
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

        public DataPointDetails[] GetDataCollectionList()
        {
            List<DataPointDetails> result = new List<DataPointDetails>();
            var data = _maintenanceTransaction.GetUserDataCollectionDef(AppSettings.UserDataCollectionDailyCheckName, AppSettings.UserDataCollectionDailyCheckRevision);
            if (data != null) {
                foreach (var dataPoint in data.DataPoints)
                {
                    result.Add(new DataPointDetails() { DataName = dataPoint.Name.ToString(), DataType = dataPoint.DataType });
                }
            }
            return result.ToArray();
        }
    }
}
