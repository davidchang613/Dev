using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAPIModel;

namespace APIReferenceModulePortable
{
    public interface IAPIReferenceModule
    {
        void TemplateGetCall();
        void TemplatePostCall();

        List<Reference> GetCountries();
        List<Reference> GetStates();
        Dictionary<string, List<Reference>> GetReferences(List<string> referenceNames);
        int GetNumber();
    }
}
