using Microsoft.ApplicationInsights;
using Microsoft.TeamFoundation.VersionControl.Client;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using TfvcBranchPolicy.CheckinPolicy.Common;
using TfvcBranchPolicy.Common;

namespace TfvcBranchPolicy.CheckinPolicy.Common
{
    [Serializable]
    public class BranchPattern : ObservableBase
    {
        [OptionalField(VersionAdded = 2)]
        private string _pattern;
        [OptionalField(VersionAdded = 2)]
        private string _name;
        [OptionalField(VersionAdded = 2)]
        public ObservableCollection<IBranchPolicy> _branchPolicies;

        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
                OnPropertyChanged("Name");
            }
        }

        public string Pattern
        {
            get
            {
                return _pattern;
            }
            set
            {
                if (_pattern == _name) { Name = value; }
                _pattern = value;               
                OnPropertyChanged("Pattern");
            }
        }

        public BranchPattern(string pattern)
        {
            // TODO: Complete member initialization
            this.Pattern = pattern;
            this.Name = pattern;
            _branchPolicies = new ObservableCollection<IBranchPolicy>();
            BranchPolicies.Add(new LockBranchPolicy());
            BranchPolicies.Add(new CodeReviewBranchPolicy());
            BranchPolicies.Add(new WorkItemBranchPolicy());
        }

        public ObservableCollection<IBranchPolicy> BranchPolicies
        {
            get
            {
                return _branchPolicies;
            }
            set
            {
                _branchPolicies = value;
                OnPropertyChanged("BranchPolicies");
            }

        }

        public List<BranchPolicyFailure> EvaluatePendingCheckin(IPendingCheckin pendingCheckin)
        {
            TellMe.Instance.TrackTrace(string.Format("EvaluatePendingCheckin for {0}", this.Name));
            Stopwatch polictTiming = new Stopwatch();
            List<BranchPolicyFailure> branchPolicyFailures = new List<BranchPolicyFailure>();
            foreach (IBranchPolicy branchPolicy in BranchPolicies)
            {
                TellMe.Instance.TrackTrace(string.Format("BEGIN-EvaluatePendingCheckin for {0}", branchPolicy.Name));
                try
                {
                    polictTiming.Restart();
                    IList<BranchPolicyFailure> bpfs = new List<BranchPolicyFailure>();
                    bpfs = branchPolicy.EvaluatePendingCheckin(this, pendingCheckin);
                    TellMe.Instance.TrackMetric(string.Format("Policy-{0}-PolicyFailures", branchPolicy.Name), branchPolicyFailures.Count);
                    branchPolicyFailures.AddRange(bpfs);
                    TellMe.Instance.TrackMetric(string.Format("Policy-{0}-Timepan", branchPolicy.Name), polictTiming.ElapsedMilliseconds);
                }
                catch (Exception ex)
                {
                    TellMe.Instance.TrackException(ex);
                    branchPolicyFailures.Add(new BranchPolicyFailure(string.Format("An error occured in policy '{0}' for the BranchPattern '{1}'. Please contact the Build Team providing the session ID {2}:\n\n{3}", branchPolicy.Name, this.Name, TellMe.Instance.Context.Session.Id, ex.ToString())));
                }
                TellMe.Instance.TrackTrace(string.Format("END-EvaluatePendingCheckin for {0}", branchPolicy.Name));
            }
            TellMe.Instance.TrackTrace(string.Format("Returning {0} Policy failures", branchPolicyFailures.Count));
            return branchPolicyFailures;
        }
    }
}
