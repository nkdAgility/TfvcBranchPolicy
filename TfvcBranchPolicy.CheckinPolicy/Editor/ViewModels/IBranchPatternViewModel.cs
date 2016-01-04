using Microsoft.TeamFoundation.VersionControl.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TfvcBranchPolicy.CheckinPolicy.Common;

namespace TfvcBranchPolicy.CheckinPolicy.Editor.ViewModels
{
    public interface IBranchPatternViewModel
    {
       BranchPattern RawBranchPattern { get; }
    }
}
