using System.Runtime.Serialization;

namespace WcfServiceXmlAndJsonDemo
{
    /// <summary>
    /// EmployeeJSON class : This class is retrieved through servie
    /// as JSON(JavaScript Object Notation) representation.
    /// </summary>
    [DataContract]
    public class EmployeeJSON
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