using Microsoft.TeamFoundation.VersionControl.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TfvcBranchPolicy.CheckinPolicy.Common;

namespace TfvcBranchPolicy.CheckinPolicy.Editor.ViewModels
{
    public class WorkItemBranchPolicyViewModel : BranchPolicyViewModel
    {

        private WorkItemBranchPolicy RawWorkItemBranchPolicy
        {
            get
            {
                return ((WorkItemBranchPolicy)RawBranchPolicy);
            }
        }
        public Boolean WorkItemRequired
        {
            get
            {
                return RawWorkItemBranchPolicy.WorkItemRequired;
            }
            set
            {
                RawWorkItemBranchPolicy.WorkItemRequired = value;
                RaisePropertyChanged("WorkItemRequired");
            }
        }

        public Boolean SelfAssigned
        {
            get
            {
                return RawWorkItemBranchPolicy.SelfAssigned;
            }
            set
            {
                RawWorkItemBranchPolicy.SelfAssigned = value;
                RaisePropertyChanged("SelfAssigned");
            }
        }

        public WorkItemBranchPolicyViewModel(IPolicyEditArgs policyEditArgs, IBranchPolicy item)
           : base(policyEditArgs, item)
        {
        }


       
    }
}
