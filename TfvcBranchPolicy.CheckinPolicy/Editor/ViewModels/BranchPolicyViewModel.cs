using GalaSoft.MvvmLight;
using Microsoft.TeamFoundation.VersionControl.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TfvcBranchPolicy.CheckinPolicy.Common;

namespace TfvcBranchPolicy.CheckinPolicy.Editor.ViewModels
{
   public abstract class BranchPolicyViewModel : ViewModelBase, IBranchPolicyViewModel
    {
        private IPolicyEditArgs _rawPolicyEditArgs;
        private IBranchPolicy _rawBranchPolicy;

        public IBranchPolicy RawBranchPolicy
        {
            get { return _rawBranchPolicy; }
        }

        public string Name { get { return _rawBranchPolicy.Name; } }

        public BranchPolicyViewModel(IPolicyEditArgs _policyEditArgs, IBranchPolicy branchPolicy)
        {
            // TODO: Complete member initialization
            this._rawPolicyEditArgs = _policyEditArgs;
            this._rawBranchPolicy = branchPolicy;
        }

       
    }
}
