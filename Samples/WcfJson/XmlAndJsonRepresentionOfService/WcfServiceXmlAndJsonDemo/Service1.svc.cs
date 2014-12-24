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
    /// Implement the methods declared in the interface
    /// </summary>
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

        #endregion
    }
}
