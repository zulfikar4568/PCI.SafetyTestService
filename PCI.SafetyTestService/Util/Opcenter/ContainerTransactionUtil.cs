using Camstar.WCF.ObjectStack;
using Camstar.WCF.Services;
using PCI.SafetyTestService.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PCI.SafetyTestService.Util.Opcenter
{
    public class ContainerTransactionUtil
    {
        private readonly HelperUtil _helperUtil;
        public ContainerTransactionUtil(HelperUtil helperUtil)
        {
            _helperUtil = helperUtil;
        }
        public bool ExecuteMoveStd(string ContainerName, string ToResourceName = "", string Resource = "", string DataCollectionName = "", string DataCollectionRev = "", DataPointDetails[] DataPoints = null, string CarrierName = "", string Comments = "", bool IgnoreException = true)
        {
            string TxnId = Guid.NewGuid().ToString();
            MoveStdService oService = null;
            try
            {
                string sMessage = "";
                MoveStd oServiceObject = null;
                ResultStatus oResultStatus = null;
                oService = new MoveStdService(AppSettings.ExCoreUserProfile);
                EventLogUtil.LogEvent(Logging.LoggingContainer(ContainerName, TxnId, "Setting input data for MoveStd ..."), System.Diagnostics.EventLogEntryType.Information, 2);
                oServiceObject = new MoveStd() { Container = new ContainerRef(ContainerName) };
                if (Resource != "")
                {
                    oServiceObject.Resource = new NamedObjectRef() { Name = Resource };
                }
                if (ToResourceName != "")
                {
                    oServiceObject.ToResource = new NamedObjectRef() { Name = ToResourceName };
                }
                if (DataPoints != null)
                {
                    if (DataCollectionName != "")
                    {
                        oServiceObject.DataCollectionDef = new RevisionedObjectRef() { Name = DataCollectionName, Revision = DataCollectionRev, RevisionOfRecord = (DataCollectionRev == "") };
                        oServiceObject.ParametricData = _helperUtil.SetDataPointSummary(oServiceObject.DataCollectionDef, DataPoints);
                    }
                    else
                    {
                        DataPointSummary oDataPointSummaryRef = _helperUtil.GetDataPointSummaryRef(oService, oServiceObject, new MoveStd_Request(), new MoveStd_Info(), ref DataCollectionName, ref DataCollectionRev);
                        oServiceObject.ParametricData = _helperUtil.SetDataPointSummary(oDataPointSummaryRef, DataPoints);
                    }
                }

                if (CarrierName != "") //Attach = true
                {
                    oServiceObject.Carrier = new NamedObjectRef(CarrierName);
                }
                if (Comments != "") oServiceObject.Comments = Comments;
                EventLogUtil.LogEvent(Logging.LoggingContainer(ContainerName, TxnId, "Execution MoveStd ...."), System.Diagnostics.EventLogEntryType.Information, 2);
                oResultStatus = oService.ExecuteTransaction(oServiceObject);
                bool statusMoveStd = _helperUtil.ProcessResult(oResultStatus, ref sMessage, false);
                EventLogUtil.LogEvent(Logging.LoggingContainer(ContainerName, TxnId, sMessage), System.Diagnostics.EventLogEntryType.Information, 2);
                return statusMoveStd;
            }
            catch (Exception ex)
            {
                ex.Source = AppSettings.AssemblyName == ex.Source ? MethodBase.GetCurrentMethod().Name : MethodBase.GetCurrentMethod().Name + "." + ex.Source;
                EventLogUtil.LogErrorEvent(Logging.LoggingContainer(ContainerName, TxnId, ex.Source), ex);
                if (!IgnoreException) throw ex;
                return false;
            }
            finally
            {
                if (!(oService is null)) oService.Close();
            }
        }
    }
}
