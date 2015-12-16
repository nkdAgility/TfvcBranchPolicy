using Microsoft.TeamFoundation.VersionControl.Client;
using Microsoft.TeamFoundation.WorkItemTracking.Client;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace TfvcBranchPolicy.CheckinPolicy.Common
{
    [Serializable]
    public class CodeReviewBranchPolicy : ObservableBase, IBranchPolicy
    {
        [OptionalField(VersionAdded = 2)]
        private Boolean _codeReviewRequired;
        [OptionalField(VersionAdded = 3)]
        private int _minimumReviewers;
        [OptionalField(VersionAdded = 3)]
        private Boolean _canApproveOwnChanges;
        [OptionalField(VersionAdded = 3)]
        private ObservableCollection<BranchPatternReview> _branchPatternReviews;

        public string Name
        {
            get
            {
                return "Code review";
            }
        }

        public Boolean CodeReviewRequired
        {
            get
            {
                return _codeReviewRequired;
            }
            set
            {
                _codeReviewRequired = value;
                OnPropertyChanged("CodeReviewRequired");
            }
        }

        public int MinimumReviewers
        {
            get
            {
                return _minimumReviewers;
            }
            set
            {
                _minimumReviewers = value;
                OnPropertyChanged("MinimumReviewers");
            }
        }

        public Boolean CanApproveOwnChanges
        {
            get
            {
                return _canApproveOwnChanges;
            }
            set
            {
                _canApproveOwnChanges = value;
                OnPropertyChanged("CanApproveOwnChanges");
            }
        }

        public ObservableCollection<BranchPatternReview> BranchPatternReviews
        {
            get
            {
                return _branchPatternReviews;
            }
            set
            {
                _branchPatternReviews = value;
                OnPropertyChanged("BranchPatternReviews");
            }

        }

        [NonSerialized]
        const string SelectBugs = "SELECT [System.Id] FROM WorkItems WHERE [System.Id] = '{0}' AND [System.WorkItemType] = 'Bug'";

        public List<BranchPolicyFailure> EvaluatePendingCheckin(BranchPattern branchPattern, Microsoft.TeamFoundation.VersionControl.Client.IPendingCheckin pendingCheckin)
        {
            List<BranchPolicyFailure> branchPolicyFailures = new List<BranchPolicyFailure>();
            if (CodeReviewRequired)
            {
                bool foundCodeReviewWorkItem = false;
                bool allCodeReviewsApproved = false;

                foreach (WorkItemCheckinInfo wiInfo in pendingCheckin.WorkItems.CheckedWorkItems)
                {
                    if (wiInfo.WorkItem.Type.Name == "Code Review Request")
                    {
                        foundCodeReviewWorkItem = true;
                        if (wiInfo.WorkItem.State == "Closed" || wiInfo.WorkItem.State == "Resolved")
                        {
                            if (((string)wiInfo.WorkItem.Fields["Resolution"].Value == "Approved"))
                            {
                                allCodeReviewsApproved = true;
                            }
                        }

                    }

                }
                if (!foundCodeReviewWorkItem) branchPolicyFailures.Add(new BranchPolicyFailure("There were no associated Code Review Work Items for this check in."));
                if (!allCodeReviewsApproved) branchPolicyFailures.Add(new BranchPolicyFailure("Not all associated Code Review work items were marked as Approved."));
            }
            return branchPolicyFailures;
        }

    }
}
