using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WebAPIModel;

namespace APIReferenceAppPortable
{
    public interface IAPIReferenceApp
    {
        List<Reference> GetCountries();
        List<Reference> GetStates();
        Dictionary<string, List<Reference>> GetReferences(List<string> referenceNames);
        int GetNumber();
    }
}
