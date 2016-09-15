using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WebAPIClientBasePortable;
using APIReferenceAppPortable;
using APIReferenceAppPortable.Models;
using APIReferralAppPortable;
using APIReferralAppPortable.Models;

namespace ReferralApplicationPortable
{
    public class ReferralApplication : APIClientBase, IWebAPIApplication, IAPIReferenceApp, IAPIReferralApp
    {
        public APIReferenceApp refAPI;
        public APIReferralApp referalAPI;

        public ReferralApplication(string serverName) : base(serverName)
        {
            InitWebAPIObjects();
        }

        #region IAPIReferenceApp 
        public List<Reference> GetCountries()
        {
            return refAPI.GetCountries();
        }       

        public List<Reference> GetStates()
        {
            return refAPI.GetStates();
        }

        public int GetNumber()
        {
            return refAPI.GetNumber();
        }
        #endregion

        #region IWebAPIApplication
        public void InitWebAPIObjects()
        {
            refAPI = new APIReferenceApp(this);
            referalAPI = new APIReferralApp(this);
        }
        #endregion

        #region IAPIReferralApp
        public List<IReferral> GetReferrals(string userName)
        {
            return referalAPI.GetReferrals(userName);
        }

        #endregion


    }
}
