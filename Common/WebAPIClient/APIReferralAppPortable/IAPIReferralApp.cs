using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using APIReferralAppPortable.Models;

namespace APIReferralAppPortable
{
    public interface IAPIReferralApp
    {
        List<IReferral> GetReferrals(string userName);
    }
}
