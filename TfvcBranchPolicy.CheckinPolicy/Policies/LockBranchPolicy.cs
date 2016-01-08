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

            // Evaluate LockPolicy
            if (IsLocked)
            {
                string byPassState = IsByPassEnabled ?
                    string.Format("Type '{0}' in the comment field to bypass the lock.", BypassString) :
                    "An override has not been configured.";

                if (!IsByPassEnabled ||
                    pendingCheckin.PendingChanges.Comment == null ||
                    !System.Text.RegularExpressions.Regex.Match(pendingCheckin.PendingChanges.Comment, BypassString).Success)
                {
                    branchPolicyFailures.Add(new BranchPolicyFailure(String.Format(
                        "{0} Check-in policy [{1}] ({2}):\n" +
                        "There is a lock one or more of the files that you are checking in.\n{3}\n",
                        Name, branchPattern.Name, branchPattern.Pattern, byPassState)));
                }
            }
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
