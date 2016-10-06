using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebAPIModel;
using WebAPIClientBasePortable;
using APIReferenceModulePortable;
using APIMemberModulePortable;

namespace APIDHMSApplicationPortable
{
    public class APIDHMSApplication : APIClientBase, IWebAPIApplication, IAPIReferenceModule
        , IAPIMemberModule
    {
        public IAPIReferenceModule refAPIReference;
        public IAPIMemberModule refAPIMember;

        public APIDHMSApplication(string serverName) : base(serverName)
        {
            InitWebAPIObjects();
        }       

        #region IWebAPIApplication
        public void InitWebAPIObjects()
        {
            refAPIReference = new APIReferenceModule(this);
            refAPIMember = new APIMemberModule(this);
        }
        #endregion

        #region IAPIReferenceModule
        public List<Reference> GetCountries()
        {
            return refAPIReference.GetCountries();
        }

        public int GetNumber()
        {
            return refAPIReference.GetNumber();
        }

        public Dictionary<string, List<Reference>> GetReferences(List<string> referenceNames)
        {
            return refAPIReference.GetReferences(referenceNames);
        }

        public List<Reference> GetStates()
        {
            throw new NotImplementedException();
        }
        public void TemplateGetCall()
        {
            throw new NotImplementedException();
        }

        public void TemplatePostCall()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IAPIMemberModule
        public Member Get(string Id)
        {
            return refAPIMember.Get(Id);
        }

        public void Post(Member mem)
        {
            refAPIMember.Post(mem);
        }

        #endregion
    }
}
