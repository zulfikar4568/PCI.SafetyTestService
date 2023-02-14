using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCI.SafetyTestService.Util
{
    public class Logging
    {
        internal static string LoggingContainer(string Container, string TxnId, string Message = "")
        {
            return $"Container: {Container}, LogId: {TxnId}, Message: {Message}";
        }
        internal static string LoggingResource(string Resource, string TxnId, string Message = "")
        {
            return $"Resource: {Resource}, LogId: {TxnId}, Message: {Message}";
        }
    }
}
