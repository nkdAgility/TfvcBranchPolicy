using Microsoft.TeamFoundation.VersionControl.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TfvcBranchPolicy.CheckinPolicy.Common;

namespace TfvcBranchPolicy.CheckinPolicy.Editor.ViewModels
{
    public class CodeReviewBranchPolicyViewModel : BranchPolicyViewModel
    {

        private CodeReviewBranchPolicy RawCodeReviewBranchPolicy
        {
            get
            {
                return ((CodeReviewBranchPolicy)RawBranchPolicy);
            }
        }

        public Boolean CodeReviewRequired
        {
            get
            {
                return RawCodeReviewBranchPolicy.CodeReviewRequired;
            }
            set
            {
                RawCodeReviewBranchPolicy.CodeReviewRequired = value;
                RaisePropertyChanged("CodeReviewRequired");
            }
        }

        public int MinimumReviewers
        {
            get
            {
                return RawCodeReviewBranchPolicy.MinimumReviewers;
            }
            set
            {
                RawCodeReviewBranchPolicy.MinimumReviewers = value;
                RaisePropertyChanged("MinimumReviewers");
            }
        }

        public Boolean CanApproveOwnChanges
        {
            get
            {
                return RawCodeReviewBranchPolicy.CanApproveOwnChanges;
            }
            set
            {
                RawCodeReviewBranchPolicy.CanApproveOwnChanges = value;
                RaisePropertyChanged("CanApproveOwnChanges");
            }
        }

        public CodeReviewBranchPolicyViewModel(IPolicyEditArgs policyEditArgs, IBranchPolicy item) : base(policyEditArgs, item)
        {
        }


    }
}
