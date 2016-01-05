using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace TfvcBranchPolicy.CheckinPolicy.Common
{
    [Serializable]
    public class LockBranchPolicy : IBranchPolicy
    {
        [OptionalField(VersionAdded = 2)]
        private Boolean _IsLocked;
        [OptionalField(VersionAdded = 2)]
        private Boolean _IsByPassEnabled;
        [OptionalField(VersionAdded = 2)]
        private string _BypassString;

        public string BypassString
        {
            get
            {
                return _BypassString;
            }
            set
            {
                _BypassString = value;
            }
        }
        public Boolean IsLocked
        {
            get
            {
                return _IsLocked;
            }
            set
            {
                _IsLocked = value;
            }
        }

        public Boolean IsByPassEnabled
        {
            get
            {
                return _IsByPassEnabled;
            }
            set
            {
                _IsByPassEnabled = value;
            }
        }

        public List<BranchPolicyFailure> EvaluatePendingCheckin(BranchPattern branchPattern, Microsoft.TeamFoundation.VersionControl.Client.IPendingCheckin pendingCheckin)
        {
            List<BranchPolicyFailure> branchPolicyFailures = new List<BranchPolicyFailure>();
            if (branchPolicyFailures == null)
            {
                throw new ArgumentNullException("branchPolicyFailures");
            }

            Boolean lockedWithNoBypass = false;
            Boolean lockedWithUnmatchedBypass = false;
            // Evaluate LockPolicy
            if (IsLocked && !IsByPassEnabled)
            {
                lockedWithNoBypass = true;
            }
            else if (IsLocked && IsByPassEnabled)
            {
                if ((pendingCheckin.PendingChanges.Comment != null) && (BypassString != null))
                {

                    if (!System.Text.RegularExpressions.Regex.Match(pendingCheckin.PendingChanges.Comment, BypassString).Success)
                    {
                        lockedWithUnmatchedBypass = true;
                    }
                }
                else
                {
                    lockedWithUnmatchedBypass = true;
                }
            }
            else
            {
                //do nothing
            }

            if (lockedWithNoBypass) { branchPolicyFailures.Add(new BranchPolicyFailure(String.Format("There is a lock on the files that you are checking in. One or more of the files in your checkin match '{0}[{1}]' which has been locked by an administrator. An override has not been configured", branchPattern.Pattern, branchPattern.Name))); }
            if (lockedWithUnmatchedBypass) { branchPolicyFailures.Add(new BranchPolicyFailure(String.Format("There is a lock on the files that you are checking in. One or more of the files in your checkin match '{0}[{1}]' which has been locked by an administrator. You can override by entering '{2}' in the comment.", branchPattern.Pattern, branchPattern.Name, this.BypassString))); }
            return branchPolicyFailures;
        }

        public string Name
        {
            get
            {
                return "Branch Lock";
            }
        }
    }
}
