using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIReferralAppPortable.Models
{
    public interface IReferral
    {
        DateTime? DOB { get; set; }
        string FirstName { get; set; }
        int Id { get; set; }
        string LastName { get; set; }
        string PatientId { get; set; }
        string PhoneNumber { get; set; }
        string Notes { get; set; }
        string CreatedBy { get; set; }
        string ModifiedBy { get; set; }
        string Address1 { get; set; }
        string Address2 { get; set; }
        string City { get; set; }
        string PostalCode { get; set; }
        Guid? StateTypeId { get; set; }
        string State { get; set; }
        Guid? CountryTypeId { get; set; }
        string Country { get; set; }

        Guid? AccountId { get; set; }

        Guid? AccountGroupId
        { get; set; }

        Guid? ProgramTypeId
        { get; set; }

    }

    public class Referral : IReferral
    {
        public int Id { get; set; }

        public string PatientId
        { get; set; }

        public string FirstName
        { get; set; }

        public string LastName
        { get; set; }
        public DateTime? DOB
        { get; set; }

        public string PhoneNumber
        { get; set; }

        public string Notes
        { get; set; }

        public string CreatedBy
        { get; set; }

        public string ModifiedBy
        { get; set; }

        public Guid? AccountId
        {
            get; set;
        }

        public Guid? AccountGroupId
        { get; set; }

        public Guid? ProgramTypeId
        { get; set; }

        public Guid? ReferralStatusId
        { get; set; }

        public string ReferralStatus
        { get; set; }

        public string AdditionalData
        { get; set; }

        public string Address1
        { get; set; }

        public string Address2
        { get; set; }

        public string City
        { get; set; }

        public string PostalCode
        { get; set; }

        public Guid? StateTypeId
        { get; set; }

        public Guid? CountryTypeId
        { get; set; }

        public string State
        {
            get;
            set;
        }

        public string Country
        {
            get;
            set;
        }
    }
}
