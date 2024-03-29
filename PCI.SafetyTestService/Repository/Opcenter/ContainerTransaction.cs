﻿using Camstar.Util;
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

namespace PCI.SafetyTestService.Repository.Opcenter
{
    public class ContainerTransaction
    {
        private readonly Driver.Opcenter.ContainerTransaction _containerTxn;
        private readonly Driver.Opcenter.Helper _helper;
        public ContainerTransaction(Driver.Opcenter.ContainerTransaction containerTxn, Driver.Opcenter.Helper helper)
        {
            _containerTxn = containerTxn;
            _helper = helper;
        }
        public bool ExecuteMoveIn(string ContainerName, string ResourceName, string DataCollectionName = "", string DataCollectionRev = "", DataPointDetails[] DataPoints = null, string CarrierName = "", string Comments = "", bool IgnoreException = true)
        {
            MoveInService service = new MoveInService(AppSettings.ExCoreUserProfile);
            MoveIn serviceObject = new MoveIn() { Container = new ContainerRef(ContainerName) };
            if (ResourceName != "") serviceObject.Resource = new NamedObjectRef(ResourceName);
            if (DataPoints != null)
            {
                if (DataCollectionName != "")
                {
                    serviceObject.DataCollectionDef = new RevisionedObjectRef() { Name = DataCollectionName, Revision = DataCollectionRev, RevisionOfRecord = (DataCollectionRev == "") };
                    serviceObject.ParametricData = _helper.SetDataPointSummary(serviceObject.DataCollectionDef, DataPoints);
                }
                else
                {
                    DataPointSummary oDataPointSummaryRef = _helper.GetDataPointSummaryRef(service, serviceObject, new MoveIn_Request(), new MoveIn_Info(), ref DataCollectionName, ref DataCollectionRev);
                    serviceObject.ParametricData = _helper.SetDataPointSummary(oDataPointSummaryRef, DataPoints);
                }
            }
            if (CarrierName != "") //Attach = true
            {
                serviceObject.Carrier = new NamedObjectRef(CarrierName);
            }
            if (Comments != "") serviceObject.Comments = Comments;
            return _containerTxn.MoveInTxn(serviceObject, service, IgnoreException);
        }
        public bool ExecuteMoveStd(string ContainerName, string ToResourceName = "", string Resource = "", string DataCollectionName = "", string DataCollectionRev = "", DataPointDetails[] DataPoints = null, string CarrierName = "", string Comments = "", bool IgnoreException = true)
        {
            MoveStdService service = new MoveStdService(AppSettings.ExCoreUserProfile);
            MoveStd serviceObject = new MoveStd() { Container = new ContainerRef(ContainerName) };
            if (Resource != "")
            {
                serviceObject.Resource = new NamedObjectRef() { Name = Resource };
            }
            if (ToResourceName != "")
            {
                serviceObject.ToResource = new NamedObjectRef() { Name = ToResourceName };
            }
            if (DataPoints != null)
            {
                if (DataCollectionName != "")
                {
                    serviceObject.DataCollectionDef = new RevisionedObjectRef() { Name = DataCollectionName, Revision = DataCollectionRev, RevisionOfRecord = (DataCollectionRev == "") };
                    serviceObject.ParametricData = _helper.SetDataPointSummary(serviceObject.DataCollectionDef, DataPoints);
                }
                else
                {
                    DataPointSummary oDataPointSummaryRef = _helper.GetDataPointSummaryRef(service, serviceObject, new MoveStd_Request(), new MoveStd_Info(), ref DataCollectionName, ref DataCollectionRev);
                    serviceObject.ParametricData = _helper.SetDataPointSummary(oDataPointSummaryRef, DataPoints);
                }
            }

            if (CarrierName != "") //Attach = true
            {
                serviceObject.Carrier = new NamedObjectRef(CarrierName);
            }
            if (Comments != "") serviceObject.Comments = Comments;
            return _containerTxn.MoveStdTxn(serviceObject, service, IgnoreException);
        }
        public bool ContainerExists(string ContainerName, bool IgnoreException = true)
        {
            // Container Info
            ViewContainerStatus_Info containerInfo = new ViewContainerStatus_Info();
            containerInfo.ContainerName = new Info(true);

            ViewContainerStatus containerStatus = _containerTxn.ContainerInfo(containerInfo, ContainerName, IgnoreException);

            if (containerStatus is null) return false;
            return true;
        }
        public ViewContainerStatus GetCurrentContainer(string ContainerName, bool IgnoreException = true)
        {
            ViewContainerStatus_Info containerInfo = new ViewContainerStatus_Info();
            containerInfo.RequestValue = true;
            containerInfo.ContainerName = new Info(true);
            containerInfo.Step = new Info(true);
            containerInfo.Qty = new Info(true);
            containerInfo.Product = new Info(true);
            containerInfo.Operation = new Info(true);
            containerInfo.Product = new Info(true);
            return _containerTxn.ContainerInfo(containerInfo, ContainerName, IgnoreException);
        }
        public CurrentContainerStatus GetContainerStatusDetails(string ContainerName, string DataCollectionName = "", string DataCollectionRev = "", bool IgnoreException = true)
        {
            ContainerTxn serviceObject = new ContainerTxn();
            serviceObject.Container = new ContainerRef(ContainerName);
            if (DataCollectionName != "")
            {
                serviceObject.DataCollectionDef = new RevisionedObjectRef() { Name = DataCollectionName, Revision = DataCollectionRev, RevisionOfRecord = (DataCollectionRev == "") };
            }
            return _containerTxn.ContainerStatusInfo(serviceObject, ContainerName, IgnoreException);
        }
        public ContainerAttrDetail[] GetContainerAttrDetails(string ContainerName, bool IgnoreException = true)
        {
            ContainerAttrMaint_Info containerAttrInfo = new ContainerAttrMaint_Info();
            containerAttrInfo.ServiceDetails = new ContainerAttrDetail_Info();

            containerAttrInfo.ServiceDetails.Attribute = new Info(true);
            containerAttrInfo.ServiceDetails.Name = new Info(true);
            containerAttrInfo.ServiceDetails.DataType = new Info(true);
            containerAttrInfo.ServiceDetails.AttributeValue = new Info(true);
            containerAttrInfo.ServiceDetails.IsExpression = new Info(true);

            return _containerTxn.GetContainerAttrDetails(containerAttrInfo, ContainerName, IgnoreException);
        }
        public bool ExecuteCollectData(string ContainerName, string DataCollectionName = "", string DataCollectionRev = "", DataPointDetails[] DataPoints = null, string Comments = "", bool IgnoreException = true)
        {
            CollectDataService service = new CollectDataService(AppSettings.ExCoreUserProfile);
            CollectData serviceObject = new CollectData() { Container = new ContainerRef(ContainerName) };
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
            return _containerTxn.CollectDataTxn(serviceObject, service, IgnoreException);
        }
        public bool ExecuteContainerAttrMaint(string ContainerName, ContainerAttrDetail[] Attributes, bool IgnoreException = true)
        {
            ContainerAttrMaintService service = new ContainerAttrMaintService(AppSettings.ExCoreUserProfile);
            ContainerAttrMaint serviceObject = new ContainerAttrMaint() { Container = new ContainerRef(ContainerName) };
            ContainerAttrDetail[] currentAttrs = GetContainerAttrDetails(ContainerName);
            if (currentAttrs != null)
            {
                foreach (ContainerAttrDetail attr in Attributes)
                {
                    foreach (var currentAttr in currentAttrs)
                    {
                        if (attr.Name == currentAttr.Attribute.Name)
                        {
                            attr.Attribute = currentAttr.Attribute;
                            break;
                        }
                    }
                }
                foreach (ContainerAttrDetail currentAttr in currentAttrs)
                {
                    bool bFoundAttr = false;
                    foreach (ContainerAttrDetail oAttr in Attributes)
                    {
                        if (oAttr.Name == currentAttr.Name)
                        {
                            bFoundAttr = true;
                            break;
                        }
                    }
                    if (!bFoundAttr)
                    {
                        Array.Resize(ref Attributes, Attributes.Length + 1);
                        Attributes[Attributes.Length - 1] = new ContainerAttrDetail() { Attribute = currentAttr.Attribute, Name = currentAttr.Name, DataType = currentAttr.DataType, AttributeValue = currentAttr.AttributeValue, IsExpression = currentAttr.IsExpression };
                    }
                }
            }

            serviceObject.ServiceDetails = Attributes;

            return _containerTxn.ContainerAttrTxn(serviceObject, service, IgnoreException);
        }
    }
}
