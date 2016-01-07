using Microsoft.TeamFoundation.Client;
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
    public class CodeReviewBranchPolicy : IBranchPolicy
    {
        [OptionalField(VersionAdded = 2)]
        private Boolean _codeReviewRequired;
        [OptionalField(VersionAdded = 3)]
        private int _minimumReviewers;
        [OptionalField(VersionAdded = 3)]
        private Boolean _canApproveOwnChanges;
        [OptionalField(VersionAdded = 3)][Obsolete]
        private ObservableCollection<BranchPatternReview> _branchPatternReviews;
 [OptionalField(VersionAdded = 4)]
        private Boolean _canMergeWithoutReview;

        public string Name
        {
            get
            {
                return "Code Review";
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
            }
        }

        [Obsolete]
        public ObservableCollection<BranchPatternReview> BranchPatternReviews
        {
            get
            {
                return _branchPatternReviews;
            }
            set
            {
                _branchPatternReviews = value;
            }

        }

        public bool CanMergeWithoutReview
        {
            get { return _canMergeWithoutReview; }
            set
            {
                _canMergeWithoutReview = value;
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
                            allCodeReviewsApproved = true;
                            foreach (WorkItemLink item in wiInfo.WorkItem.WorkItemLinks)
                            {
                                WorkItem child = wiInfo.WorkItem.Type.Store.GetWorkItem(item.TargetId);
                                if (child.Type.Name == "Code Review Response")
                                {
                                    string closedStatus = child.Fields["Closed Status"].Value.ToString();
                                    if (closedStatus != "Looks Good" && closedStatus != "Removed")
                                        allCodeReviewsApproved = false;
                                }
                            }
                        }

                    }

                }

                if (!allCodeReviewsApproved)
                {
                    if (foundCodeReviewWorkItem)
                    {
                        branchPolicyFailures.Add(new BranchPolicyFailure(string.Format(
                            "{0} Check-in policy [{1}] ({2}):\n" +
                            "All Code Reviews associated with the check-in must be Approved ('Looks Good').",
                            Name, branchPattern.Name, branchPattern.Pattern)));
                    }
                    else
                    {
                        branchPolicyFailures.Add(new BranchPolicyFailure(string.Format(
                            "{0} Check-in policy [{1}] ({2}):\n" +
                            "Check-in must be associated with at least one Code Review.\n" +
                            "All Code Reviews associated with the check-in must be Approved ('Looks Good').",
                            Name, branchPattern.Name, branchPattern.Pattern)));
                    }
                }
            }
            return branchPolicyFailures;
        }
    }
}
