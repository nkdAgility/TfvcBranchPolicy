using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using Microsoft.TeamFoundation.VersionControl.Client;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using TfvcBranchPolicy.CheckinPolicy.Common;
using TfvcBranchPolicy.CheckinPolicy.Editor.ViewModels;

namespace TfvcBranchPolicy.CheckinPolicy.Editor
{
    public class BranchPatternPolicyEditorViewModel : ViewModelBase, IBranchPatternPolicyEditorViewModel
    {
        public IBranchPatternsRepository _repo;
        public BranchPatternViewModel _SelectedBranchPattern;
        public IPolicyEditArgs _policyEditArgs;

        public ObservableCollection<BranchPatternViewModel> BranchPatterns { get; set; }
        public BranchPatternViewModel SelectedBranchPattern
        {
            get { return _SelectedBranchPattern; }
            set
            {
                _SelectedBranchPattern = value;
                RaisePropertyChanged("SelectedBranchPattern");
        }}

        public RelayCommand CreateCommand
        {
            get;
            private set;
        }

        public RelayCommand ResetCommand
        {
            get;
            private set;
        }

        public RelayCommand<BranchPatternViewModel> DeleteCommand
        {
            get;
            private set;
        }


        internal BranchPatternPolicyEditorViewModel(IPolicyEditArgs policyEditArgs, IBranchPatternsRepository repo)
        {
            _policyEditArgs = policyEditArgs;
            _repo = repo;
            BranchPatterns = new ObservableCollection<BranchPatternViewModel>();
            CreateCommand = new RelayCommand(ExecuteCreateCommand);
            ResetCommand = new RelayCommand(ExecuteResetCommand);
            DeleteCommand = new RelayCommand<BranchPatternViewModel>(ExecuteDeleteCommand);
            ExecuteResetCommand();
            
        }

        private void ExecuteResetCommand()
        {
            BranchPatterns.Clear();
            foreach (BranchPattern item in _repo.FindAll())
            {
                BranchPatterns.Add(new BranchPatternViewModel( _policyEditArgs, item ));
            }
            RaisePropertyChanged("BranchPolicys");
        }

        private void ExecuteCreateCommand()
        {
            BranchPattern newBranchPattern = new BranchPattern("^.*");

            newBranchPattern.BranchPolicies.Add(new LockBranchPolicy());
            newBranchPattern.BranchPolicies.Add(new CodeReviewBranchPolicy());
            newBranchPattern.BranchPolicies.Add(new WorkItemBranchPolicy());

            BranchPatterns.Add(new BranchPatternViewModel(_policyEditArgs, newBranchPattern));

            SelectedBranchPattern = (from bpvm in BranchPatterns where bpvm.RawBranchPattern == newBranchPattern select bpvm).Single();
        }

        private void ExecuteDeleteCommand(BranchPatternViewModel bpToDelete)
        {
            _repo.Remove(bpToDelete.RawBranchPattern);
            BranchPatterns.Remove(bpToDelete);
            SelectedBranchPattern = null;
        }

    }
}
