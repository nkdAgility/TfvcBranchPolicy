using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TfvcBranchPolicy.CheckinPolicy.Common;

namespace TfvcBranchPolicy.CheckinPolicy.Editor.ViewModels
{
    public interface IBranchPolicyViewModel
    {
        IBranchPolicy RawBranchPolicy { get; }
    }
}
