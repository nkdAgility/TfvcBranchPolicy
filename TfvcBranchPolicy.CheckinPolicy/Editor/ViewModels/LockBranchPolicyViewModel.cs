using Microsoft.TeamFoundation.VersionControl.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TfvcBranchPolicy.CheckinPolicy.Common;

namespace TfvcBranchPolicy.CheckinPolicy.Editor.ViewModels
{
   public  class LockBranchPolicyViewModel : BranchPolicyViewModel
    {
       private LockBranchPolicy RawLockBranchPolicy
       {
           get
           {
               return ((LockBranchPolicy)RawBranchPolicy);
           }
       }

       public string BypassString
       {
           get
           {
               return RawLockBranchPolicy.BypassString;
           }
           set
           {
               RawLockBranchPolicy.BypassString = value;
               RaisePropertyChanged("BypassString");
           }
       }
       public Boolean IsLocked
       {
           get
           {
               return RawLockBranchPolicy.IsLocked;
           }
           set
           {
               RawLockBranchPolicy.IsLocked = value;
               RaisePropertyChanged("IsLocked");
           }
       }

       public Boolean IsByPassEnabled
       {
           get
           {
               return RawLockBranchPolicy.IsByPassEnabled;
           }
           set
           {
               RawLockBranchPolicy.IsByPassEnabled = value;
               RaisePropertyChanged("IsByPassEnabled");
           }
       }

       public LockBranchPolicyViewModel(IPolicyEditArgs policyEditArgs, IBranchPolicy item)
           : base(policyEditArgs, item)
        {
        }
    }
}
