using System.Runtime.Serialization;

namespace WcfServiceXmlAndJsonDemo
{
    /// <summary>
    /// EmployeeXML class : This class is retrieved through servie
    /// as XML representation.
    /// </summary>
    [DataContract]
    public class EmployeeXML
    {
        #region Properties

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public double Salary { get; set; }

        #endregion
    }
}