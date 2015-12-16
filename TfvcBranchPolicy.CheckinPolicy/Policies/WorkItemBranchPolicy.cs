using Microsoft.TeamFoundation.VersionControl.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace TfvcBranchPolicy.CheckinPolicy.Common
{
    [Serializable]
    public class WorkItemBranchPolicy : ObservableBase, IBranchPolicy
    {
        [OptionalField(VersionAdded = 2)]
        private Boolean _workItemRequired;
        [OptionalField(VersionAdded = 2)]
        private Boolean _selfAssigned;


        public string Name
        {
            get
            {
                return "Work item ";
            }
        }

        public Boolean WorkItemRequired
        {
            get
            {
                return _workItemRequired;
            }
            set
            {
                _workItemRequired = value;
                OnPropertyChanged("WorkItemRequired");
            }
        }

        public Boolean SelfAssigned
        {
            get
            {
                return _selfAssigned;
            }
            set
            {
                _selfAssigned = value;
                OnPropertyChanged("SelfAssigned");
            }
        }

        public List<BranchPolicyFailure> EvaluatePendingCheckin(BranchPattern branchPattern, Microsoft.TeamFoundation.VersionControl.Client.IPendingCheckin pendingCheckin)
        {
            List<BranchPolicyFailure> branchPolicyFailures = new List<BranchPolicyFailure>();
            // Evaluate LockPolicy
            if (WorkItemRequired)
            {
                bool foundWorkItems = false;
                foreach (WorkItemCheckinInfo item in pendingCheckin.WorkItems.CheckedWorkItems)
                {
                    foundWorkItems = true;
                }
                if (!foundWorkItems)  {branchPolicyFailures.Add(new BranchPolicyFailure(String.Format("The policy for {0} failed because there are no Work Items associated. Bypass has not been enabled", branchPattern.Pattern)));}
            }
            return branchPolicyFailures;
        }

    }
}
