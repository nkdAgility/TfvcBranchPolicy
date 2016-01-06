using GalaSoft.MvvmLight;
using Microsoft.TeamFoundation.VersionControl.Client;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TfvcBranchPolicy.CheckinPolicy.Common;

namespace TfvcBranchPolicy.CheckinPolicy.Editor.ViewModels
{
    public class BranchPatternViewModel :  ViewModelBase, IBranchPatternViewModel
    {
        private IPolicyEditArgs _rawPolicyEditArgs;
        private BranchPattern _rawBranchPattern;
        public ObservableCollection<IBranchPolicyViewModel> _branchPolicies;

        public BranchPattern RawBranchPattern
        {
            get { return _rawBranchPattern; }
        }

        public string Name
        {
            get
            {
                return _rawBranchPattern.Name;
            }
            set
            {
                _rawBranchPattern.Name = value;
                RaisePropertyChanged("Name");
            }
        }

        public string Pattern
        {
            get
            {
                return _rawBranchPattern.Pattern;
            }
            set
            {
                if (_rawBranchPattern.Pattern == _rawBranchPattern.Name) { Name = value; }
                _rawBranchPattern.Pattern = value;
                RaisePropertyChanged("Pattern");
            }
        }

        public ObservableCollection<IBranchPolicyViewModel> BranchPolicies
        {
            get
            {
                return _branchPolicies;
            }
        }


        public BranchPatternViewModel(IPolicyEditArgs _policyEditArgs, BranchPattern branchPattern)
        {
            // TODO: Complete member initialization
            this._rawPolicyEditArgs = _policyEditArgs;
            this._rawBranchPattern = branchPattern;
            this._branchPolicies = new ObservableCollection<IBranchPolicyViewModel>();
            RefreshBranchPolicies();

        }

        public void RefreshBranchPolicies()
        {
            BranchPolicies.Clear();
            foreach (IBranchPolicy item in _rawBranchPattern.BranchPolicies)
            {
                if (item is CodeReviewBranchPolicy)
                {
                    BranchPolicies.Add(new CodeReviewBranchPolicyViewModel(_rawPolicyEditArgs, item));
                }
                if (item is LockBranchPolicy)
                {
                    BranchPolicies.Add(new LockBranchPolicyViewModel(_rawPolicyEditArgs, item));
                }
                if (item is WorkItemBranchPolicy)
                {
                    BranchPolicies.Add(new WorkItemBranchPolicyViewModel(_rawPolicyEditArgs, item));
                }
                
            }
        }

    }
}
