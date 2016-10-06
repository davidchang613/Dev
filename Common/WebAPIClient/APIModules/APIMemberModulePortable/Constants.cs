using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIMemberModulePortable
{
    public static class MemberAPIKey
    {
        public const string GetKey = "Member.Get";
        public const string PostKey = "Member.Post";

    }

    public static class MemberAPIPath
    {
        public const string GetPath = @"/api/Member/Get";
        public const string PostPath = @"/api/Member/Post";
    }
}
