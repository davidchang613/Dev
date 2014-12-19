using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace WcfServiceXmlAndJsonDemo
{
    /// <summary>
    /// This interface contains the operation contracts.
    /// </summary>
    [ServiceContract]
    public interface IService1
    {
        #region OperationContracts

        [OperationContract]
        [WebInvoke(Method="GET",UriTemplate="GetXml",ResponseFormat=WebMessageFormat.Xml,BodyStyle=WebMessageBodyStyle.Bare)]
        EmployeeXML GetEmployeeXML();

        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "GetJson", ResponseFormat = WebMessageFormat.Json,BodyStyle = WebMessageBodyStyle.Wrapped)]
        EmployeeJSON GetEmployeeJSON();

        #endregion
    }
}
