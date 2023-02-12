using Camstar.WCF.ObjectStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCI.SafetyTestService.Repository.Opcenter
{
    public class MaintenanceTransaction
    {
        private readonly Driver.Opcenter.MaintenanceTransaction _maintenanceTxn;
        public MaintenanceTransaction(Driver.Opcenter.MaintenanceTransaction maintenanceTxn)
        {
            _maintenanceTxn = maintenanceTxn;
        }
        public UserDataCollectionDefChanges GetUserDataCollectionDef(string UserDataCollectionDefName, string UserDataCollectionDefRevision = "", bool IgnoreException = true)
        {
            RevisionedObjectRef objectToChange = new RevisionedObjectRef(UserDataCollectionDefName);
            if (UserDataCollectionDefName != "" && UserDataCollectionDefRevision != "")
            {
                objectToChange = new RevisionedObjectRef(UserDataCollectionDefName, UserDataCollectionDefRevision);
            }
            UserDataCollectionDefChanges_Info userDataCollectionDefInfo = new UserDataCollectionDefChanges_Info();

            userDataCollectionDefInfo.Name = new Info(true);
            userDataCollectionDefInfo.Revision = new Info(true);
            userDataCollectionDefInfo.Description = new Info(true);
            userDataCollectionDefInfo.DataPoints = new DataPointChanges_Info() 
            {
                RequestValue = true,
                RequestSelectionValues = true,
                Name = new Info(true),
                ColumnPosition =new Info(true),
                DataType =new Info(true),
                Description1 = new Info(true),
                DecimalScale =new Info(true),
                DisplayName =new Info(true),
                RowPosition =new Info(true),
                TxnDate =new Info(true),
                TypeName =new Info(true),
                ParentName =new Info(true),
                Parent = new Info(true),
            };

            return _maintenanceTxn.UserDataCollectionInfo(objectToChange, userDataCollectionDefInfo, IgnoreException);
        }
    }
}
