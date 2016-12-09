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
        EmployeeJSON benGetEmployeeJSON();

        [OperationContract]       // if  WebMessageBodyStyle.Wrapped, the json object returned in the ajax is the name GetJsonWithParameterResult, if bare, just the json object return
        [WebInvoke(Method = "POST", UriTemplate = "GetJsonWithParameter/id={id}/action={action}", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped)]
        EmployeeJSON GetEmployeeJSONWithParameter(string id, string action, SomeType someType);

        #endregion
    }
    
    [DataContract]
    public class SomeType
    {
        [DataMember]
        public string Name;

        [DataMember]
        public string ValueField;

        [DataMember]
        public SomeType ListSomeType;

        [DataMember]
        public List<SomeType> ListSomeTypeArray;

        //[DataMember]
        //public Dictionary<string, string> DictionaryList;

    }
}
