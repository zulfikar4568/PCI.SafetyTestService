using Autofac.Core;
using Camstar.WCF.ObjectStack;
using Camstar.WCF.Services;
using PCI.SafetyTestService.Config;
using PCI.SafetyTestService.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace PCI.SafetyTestService.Driver.Opcenter
{
    public class ContainerTransaction
    {
        private readonly Helper _helper;
        public ContainerTransaction(Helper helper)
        {
            _helper = helper;
        }
        public bool MoveInTxn(MoveIn ServiceObject, MoveInService Service, bool IgnoreException = true)
        {
            string TxnId = Guid.NewGuid().ToString();
            try
            {
                string sMessage = "";
                MoveIn oServiceObject = null;
                ResultStatus oResulstStatus = null;
                EventLogUtil.LogEvent(Logging.LoggingContainer(ServiceObject.Container.Name, TxnId, "Setting input data for MoveIn ..."), System.Diagnostics.EventLogEntryType.Information, 2);
                oServiceObject = ServiceObject;

                // Execute Transaction
                EventLogUtil.LogEvent(Logging.LoggingContainer(ServiceObject.Container.Name, TxnId, "Execution a Move In"), System.Diagnostics.EventLogEntryType.Information, 2);
                oResulstStatus = Service.ExecuteTransaction(oServiceObject);

                // Process Result
                bool statusMoveIn = _helper.ProcessResult(oResulstStatus, ref sMessage, false);
                EventLogUtil.LogEvent(Logging.LoggingContainer(ServiceObject.Container.Name, TxnId, sMessage), System.Diagnostics.EventLogEntryType.Information, 2);
                return statusMoveIn;
            }
            catch (Exception ex)
            {
                ex.Source = AppSettings.AssemblyName == ex.Source ? MethodBase.GetCurrentMethod().Name : MethodBase.GetCurrentMethod().Name + "." + ex.Source;
                string exceptionMsg = Logging.LoggingContainer(ServiceObject.Container.Name, TxnId, ex.Message);
                EventLogUtil.LogErrorEvent(ex.Source, exceptionMsg);
                if (!IgnoreException) throw ex;
                return false;
            }
            finally
            {
                if (!(Service is null)) Service.Close();
            }
        }
        public bool MoveStdTxn(MoveStd ServiceObject, MoveStdService Service, bool IgnoreException = true)
        {
            string TxnId = Guid.NewGuid().ToString();
            try
            {
                string sMessage = "";
                MoveStd oServiceObject = null;
                ResultStatus oResultStatus = null;
                EventLogUtil.LogEvent(Logging.LoggingContainer(ServiceObject.Container.Name, TxnId, "Setting input data for MoveStd ..."), System.Diagnostics.EventLogEntryType.Information, 2);
                oServiceObject = ServiceObject;

                EventLogUtil.LogEvent(Logging.LoggingContainer(ServiceObject.Container.Name, TxnId, "Execution MoveStd ...."), System.Diagnostics.EventLogEntryType.Information, 2);
                oResultStatus = Service.ExecuteTransaction(oServiceObject);
                bool statusMoveStd = _helper.ProcessResult(oResultStatus, ref sMessage, false);
                EventLogUtil.LogEvent(Logging.LoggingContainer(ServiceObject.Container.Name, TxnId, sMessage), System.Diagnostics.EventLogEntryType.Information, 2);
                return statusMoveStd;
            }
            catch (Exception ex)
            {
                ex.Source = AppSettings.AssemblyName == ex.Source ? MethodBase.GetCurrentMethod().Name : MethodBase.GetCurrentMethod().Name + "." + ex.Source;
                EventLogUtil.LogErrorEvent(Logging.LoggingContainer(ServiceObject.Container.Name, TxnId, ex.Source), ex);
                if (!IgnoreException) throw ex;
                return false;
            }
            finally
            {
                if (!(Service is null)) Service.Close();
            }
        }
        public bool CollectDataTxn(CollectData ServiceObject, CollectDataService Service, bool IgnoreException = true)
        {
            string TxnId = Guid.NewGuid().ToString();
            try
            {
                string sMessage = "";
                CollectData oServiceObject = null;
                ResultStatus oResultStatus = null;
                EventLogUtil.LogEvent(Logging.LoggingContainer(ServiceObject.Container.Name, TxnId, "Setting input data for Collect Data ..."), System.Diagnostics.EventLogEntryType.Information, 2);
                oServiceObject = ServiceObject;

                EventLogUtil.LogEvent(Logging.LoggingContainer(ServiceObject.Container.Name, TxnId, "Execution Collect Data ...."), System.Diagnostics.EventLogEntryType.Information, 2);
                oResultStatus = Service.ExecuteTransaction(oServiceObject);
                bool statusMoveStd = _helper.ProcessResult(oResultStatus, ref sMessage, false);
                EventLogUtil.LogEvent(Logging.LoggingContainer(ServiceObject.Container.Name, TxnId, sMessage), System.Diagnostics.EventLogEntryType.Information, 2);
                return statusMoveStd;
            }
            catch (Exception ex)
            {
                ex.Source = AppSettings.AssemblyName == ex.Source ? MethodBase.GetCurrentMethod().Name : MethodBase.GetCurrentMethod().Name + "." + ex.Source;
                EventLogUtil.LogErrorEvent(Logging.LoggingContainer(ServiceObject.Container.Name, TxnId, ex.Source), ex);
                if (!IgnoreException) throw ex;
                return false;
            }
            finally
            {
                if (!(Service is null)) Service.Close();
            }
        }
        public bool ContainerAttrTxn(ContainerAttrMaint ServiceObject, ContainerAttrMaintService Service, bool IgnoreException = true)
        {
            string TxnId = Guid.NewGuid().ToString();
            try
            {
                string message = "";
                ResultStatus resultStatus = null;
                EventLogUtil.LogEvent(Logging.LoggingContainer(ServiceObject.Container.Name, TxnId, "Execution Container Attribute ...."), System.Diagnostics.EventLogEntryType.Information, 2);
                resultStatus = Service.ExecuteTransaction(ServiceObject);
                bool statusContainerAttr = _helper.ProcessResult(resultStatus, ref message, false);

                EventLogUtil.LogEvent(Logging.LoggingContainer(ServiceObject.Container.Name, TxnId, message), System.Diagnostics.EventLogEntryType.Information, 2);
                return statusContainerAttr;
            }
            catch (Exception ex)
            {
                ex.Source = AppSettings.AssemblyName == ex.Source ? MethodBase.GetCurrentMethod().Name : MethodBase.GetCurrentMethod().Name + "." + ex.Source;
                EventLogUtil.LogErrorEvent(Logging.LoggingContainer(ServiceObject.Container.Name, TxnId, ex.Source), ex);
                if (!IgnoreException) throw ex;
                return false;
            }
            finally
            {
                if (!(Service is null)) Service.Close();
            }

        }
        public ViewContainerStatus ContainerInfo(ViewContainerStatus_Info ContainerInfo, string ContainerName, bool IgnoreException = true)
        {
            string TxnId = Guid.NewGuid().ToString();
            ViewContainerStatusService oService = null;
            try
            {
                oService = new ViewContainerStatusService(AppSettings.ExCoreUserProfile);

                //Set input Data
                ViewContainerStatus oServiceObject = new ViewContainerStatus();
                oServiceObject.Container = new ContainerRef(ContainerName);
                ViewContainerStatus_Request oServiceRequest = new ViewContainerStatus_Request
                {
                    Info = ContainerInfo
                };

                //Request the Data
                ResultStatus oResultStatus = oService.ExecuteTransaction(oServiceObject, oServiceRequest, out ViewContainerStatus_Result oServiceResult);

                //Return Result
                string sMessage = "";
                bool processResult = _helper.ProcessResult(oResultStatus, ref sMessage, false);
                EventLogUtil.LogEvent(Logging.LoggingContainer(ContainerName, TxnId, sMessage), System.Diagnostics.EventLogEntryType.Information, 3);
                if (processResult)
                {
                    return oServiceResult.Value;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                ex.Source = AppSettings.AssemblyName == ex.Source ? MethodBase.GetCurrentMethod().Name : MethodBase.GetCurrentMethod().Name + "." + ex.Source;
                string exceptionMsg = Logging.LoggingContainer(ContainerName, TxnId, ex.Message);
                EventLogUtil.LogErrorEvent(ex.Source, exceptionMsg);
                if (!IgnoreException) throw ex;
                return null;
            }
            finally
            {
                if (!(oService is null)) oService.Close();
            }
        }
        public CurrentContainerStatus ContainerStatusInfo(ContainerTxn ServiceObject, string ContainerName, bool IgnoreException = true)
        {
            string TxnId = Guid.NewGuid().ToString();
            ContainerTxnService oService = null;
            try
            {
                oService = new ContainerTxnService(AppSettings.ExCoreUserProfile);

                //Set input Data
                ContainerTxn oServiceObject = ServiceObject;
                ContainerTxn_Request oServiceRequest = new ContainerTxn_Request();
                oServiceRequest.Info = new ContainerTxn_Info();
                oServiceRequest.Info.CurrentContainerStatus = new CurrentContainerStatus_Info();
                oServiceRequest.Info.CurrentContainerStatus.RequestValue = true;

                //Requets the Data
                ResultStatus oResultStatus = oService.GetEnvironment(oServiceObject, oServiceRequest, out ContainerTxn_Result oServiceResult);

                //Return Result
                string sMessage = "";
                bool processResult = _helper.ProcessResult(oResultStatus, ref sMessage, false);
                EventLogUtil.LogEvent(Logging.LoggingContainer(ContainerName, TxnId, sMessage), System.Diagnostics.EventLogEntryType.Information, 3);
                if (processResult)
                {
                    return oServiceResult.Value.CurrentContainerStatus;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                ex.Source = AppSettings.AssemblyName == ex.Source ? MethodBase.GetCurrentMethod().Name : MethodBase.GetCurrentMethod().Name + "." + ex.Source;
                string exceptionMsg = Logging.LoggingContainer(ContainerName, TxnId, ex.Message);
                EventLogUtil.LogErrorEvent(ex.Source, exceptionMsg);
                if (!IgnoreException) throw ex;
                return null;
            }
            finally
            {
                if (!(oService is null)) oService.Close();
            }
        }
        public ContainerAttrDetail[] GetContainerAttrDetails(ContainerAttrMaint_Info ContainerAttrInfo, string ContainerName, bool IgnoreException = true)
        {
            string TxnId = Guid.NewGuid().ToString();
            ContainerAttrMaintService oService = null;
            try
            {
                oService = new ContainerAttrMaintService(AppSettings.ExCoreUserProfile);

                // Setting Input Data
                ContainerAttrMaint oServiceObject = new ContainerAttrMaint() { Container = new ContainerRef(ContainerName) };
                ContainerAttrMaint_Request oServiceRequest = new ContainerAttrMaint_Request() { Info = ContainerAttrInfo };

                //Request the Data
                ResultStatus oResultStatus = oService.GetAttributes(oServiceObject, oServiceRequest, out ContainerAttrMaint_Result oServiceResult);

                //Return Result
                string sMessage = "";
                if (_helper.ProcessResult(oResultStatus, ref sMessage, false))
                {
                    EventLogUtil.LogEvent(Logging.LoggingContainer(ContainerName, TxnId, sMessage), System.Diagnostics.EventLogEntryType.Information, 3);
                    return oServiceResult.Value.ServiceDetails;
                } else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                ex.Source = AppSettings.AssemblyName == ex.Source ? MethodBase.GetCurrentMethod().Name : MethodBase.GetCurrentMethod().Name + "." + ex.Source;
                string exceptionMsg = Logging.LoggingContainer(ContainerName, TxnId, ex.Message);
                EventLogUtil.LogErrorEvent(ex.Source, exceptionMsg);
                if (!IgnoreException) throw ex;
                return null;
            }
            finally
            {
                if (!(oService is null)) oService.Close();
            }
        }
    }
}
