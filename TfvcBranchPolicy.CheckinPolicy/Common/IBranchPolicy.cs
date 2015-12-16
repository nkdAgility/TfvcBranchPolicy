using Microsoft.TeamFoundation.VersionControl.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TfvcBranchPolicy.CheckinPolicy.Common
{
    public interface IBranchPolicy
    {
        List<BranchPolicyFailure> EvaluatePendingCheckin(BranchPattern branchPattern, IPendingCheckin pendingCheckin);
        string Name  {get;}
    }
}
