using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIReferenceModulePortable
{
    public static class ReferenceAPIKey
    {
        public const string GetKey = "Reference.Get";
        public const string PostKey = "Reference.Post";

        public const string GetCountries = "GetCountries";
        public const string GetStates = "GetStates";
        public const string GetReferencesByName = "GetReferencesByName";
        public const string GetNumber = "GetNumber";
        public const string GetReferences = "GetReferences";

    }

    public static class ReferenceAPIPath
    {
        public const string GetPath = @"/api/Reference/Get";
        public const string PostPath = @"/api/Reference/Post";

        public const string GetCountriesPath = @"/api/Reference/GetCountries";
        public const string GetStatePaths = @"/api/Reference/GetStates";
        public const string GetReferencesByNamePath = @"/api/Reference/GetReferencesByName";
        public const string GetNumberPath = "GetNumber";
        public const string GetReferencesPath = "GetReferences";

    }
}
