using Camstar.WCF.ObjectStack;
using Camstar.WCF.Services;
using PCI.SafetyTestService.Config;
using PCI.SafetyTestService.Driver.Opcenter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace PCI.SafetyTestService.Repository.Opcenter
{
    public class ResourceTransaction
    {
        private readonly Driver.Opcenter.ResourceTransaction _resourceTxn;
        private readonly Driver.Opcenter.Helper _helper;
        public ResourceTransaction(Driver.Opcenter.ResourceTransaction resourceTransaction, Driver.Opcenter.Helper helper)
        {
            _resourceTxn = resourceTransaction;
            _helper = helper;
        }

        public bool ExecuteCollectResourceData(string ResourceName, string DataCollectionName = "", string DataCollectionRev = "", DataPointDetails[] DataPoints = null, string Comments = "", bool IgnoreException = true)
        {
            CollectResourceDataService service = new CollectResourceDataService(AppSettings.ExCoreUserProfile);
            CollectResourceData serviceObject = new CollectResourceData() { Resource = new NamedObjectRef(ResourceName) };
            if (DataPoints != null)
            {
                if (DataCollectionName != "")
                {
                    serviceObject.DataCollectionDef = new RevisionedObjectRef() { Name = DataCollectionName, Revision = DataCollectionRev, RevisionOfRecord = (DataCollectionRev == "") };
                    serviceObject.ParametricData = _helper.SetDataPointSummary(serviceObject.DataCollectionDef, DataPoints);
                }
                else
                {
                    DataPointSummary oDataPointSummaryRef = _helper.GetDataPointSummaryRef(service, serviceObject, new CollectResourceData_Request(), new CollectResourceData_Info(), ref DataCollectionName, ref DataCollectionRev);
                    serviceObject.ParametricData = _helper.SetDataPointSummary(oDataPointSummaryRef, DataPoints);
                }
            }

            if (Comments != "") serviceObject.Comments = Comments;
            return _resourceTxn.CollectResourceDataTxn(serviceObject, service, IgnoreException);
        }
    }
}
