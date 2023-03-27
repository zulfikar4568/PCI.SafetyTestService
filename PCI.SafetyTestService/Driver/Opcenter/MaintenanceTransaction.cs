using Camstar.WCF.ObjectStack;
using Camstar.WCF.Services;
using PCI.SafetyTestService.Config;
using PCI.SafetyTestService.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PCI.SafetyTestService.Driver.Opcenter
{
    public class MaintenanceTransaction
    {
        private readonly Helper _helper;
        public MaintenanceTransaction(Helper helper)
        {
            _helper = helper;
        }
        public UserDataCollectionDefChanges UserDataCollectionInfo(RevisionedObjectRef ObjectRevisionRef, UserDataCollectionDefChanges_Info ObjectChanges, bool IgnoreException = true)
        {
            UserDataCollectionDefMaintService oService = null;
            try
            {
                oService = new UserDataCollectionDefMaintService(AppSettings.ExCoreUserProfile);
                UserDataCollectionDefMaint oServiceObject = new UserDataCollectionDefMaint();
                oServiceObject.ObjectToChange = ObjectRevisionRef;
                UserDataCollectionDefMaint_Request oServiceRequest = new UserDataCollectionDefMaint_Request();
                oServiceRequest.Info = new UserDataCollectionDefMaint_Info();
                oServiceRequest.Info.ObjectChanges = ObjectChanges;

                UserDataCollectionDefMaint_Result oServiceResult = null;
                ResultStatus oResultStatus = oService.Load(oServiceObject, oServiceRequest, out oServiceResult);

                EventLogUtil.LogEvent(oResultStatus.Message, System.Diagnostics.EventLogEntryType.Information, 3);
                if (oServiceResult == null) return null;
                if (oServiceResult.Value.ObjectChanges != null)
                {
                    return oServiceResult.Value.ObjectChanges;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                ex.Source = AppSettings.AssemblyName == ex.Source ? MethodBase.GetCurrentMethod().Name : MethodBase.GetCurrentMethod().Name + "." + ex.Source;
                EventLogUtil.LogErrorEvent(ex.Source, ex);
                if (!IgnoreException) throw ex;
                return null;
            }
            finally
            {
                oService?.Close();
            }
        }
    }
}
