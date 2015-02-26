using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Web.Script.Serialization;
using System.Security.Authentication;

using System.ServiceModel.Activation;


namespace WcfServiceXmlAndJsonDemo
{
    /// <summary>
    /// Implement the methods declared in the interface
    /// </summary>      
    [AspNetCompatibilityRequirements(RequirementsMode= AspNetCompatibilityRequirementsMode.Allowed)]
    public class Service1 : IService1
    {
        #region ImplementedMethods       
        public EmployeeXML GetEmployeeXML()
        {
            EmployeeXML xml = new EmployeeXML() {Name="Sudheer",Id=100,Salary=4000.00 };
            return xml; ;
        }

        public EmployeeJSON GetEmployeeJSON()
        {
            EmployeeJSON json = new EmployeeJSON() {Name="Sumanth",Id=101,Salary=5000.00 };
            return json;
        }
        
        public EmployeeJSON GetEmployeeJSONWithParameter(string id, string action, SomeType someType)
        {
            string myaction = action;
            
            // if json object is specified in the parameter
            // if json object is not 
            string JSONString = OperationContext.Current.RequestContext.RequestMessage.ToString();
            JSONString = WebOperationContext.Current.IncomingRequest.ToString();
            JavaScriptSerializer serializer = new JavaScriptSerializer();

            string username = string.Empty;
            if (OperationContext.Current.ServiceSecurityContext != null)
                username = OperationContext.Current.ServiceSecurityContext.WindowsIdentity.Name;
                       
            EmployeeJSON json = new EmployeeJSON() { Name = "Name with Id: " + id, Id = int.Parse(id), Salary = 5000.00 };
            return json;
        }

        #endregion
    }
}
