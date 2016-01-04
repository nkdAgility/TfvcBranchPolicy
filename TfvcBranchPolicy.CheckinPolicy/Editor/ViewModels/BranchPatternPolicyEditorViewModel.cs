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

namespace TfvcBranchPolicy.CheckinPolicy.Editor
{
    public class BranchPatternPolicyEditorViewModel : ViewModelBase, IBranchPatternPolicyEditorViewModel
    {
        public IEnumerable<BranchPattern> _incommingBranchPatterns;
        public BranchPattern _currentBranchPattern;

        public ObservableCollection<BranchPattern> BranchPatterns { get; set; }
        public BranchPattern CurrentBranchPattern
        {
            get { return _currentBranchPattern; }
            set
            {
                _currentBranchPattern = value;
                RaisePropertyChanged("CurrentBranchPattern");
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

        public RelayCommand<BranchPattern> DeleteCommand
        {
            get;
            private set;
        }


        internal BranchPatternPolicyEditorViewModel(IEnumerable<BranchPattern> collection)
        {
            _incommingBranchPatterns = collection;
            CreateCommand = new RelayCommand(ExecuteCreateCommand);
            ResetCommand = new RelayCommand(ExecuteResetCommand);
            DeleteCommand = new RelayCommand<BranchPattern>(ExecuteDeleteCommand);
            ExecuteResetCommand();
            
        }

        private void ExecuteResetCommand()
        {
            if (_incommingBranchPatterns == null)
            {
                BranchPatterns = new ObservableCollection<BranchPattern>();
                ExecuteCreateCommand();
            }
            else
            {
                BranchPatterns = new ObservableCollection<BranchPattern>(_incommingBranchPatterns);
            }
            RaisePropertyChanged("BranchPolicys");
        }

        private void ExecuteCreateCommand()
        {
            BranchPattern nBP = new BranchPattern("^.*");
            BranchPatterns.Add(nBP);
            CurrentBranchPattern = nBP;
        }

        private void ExecuteDeleteCommand(BranchPattern bpToDelete)
        {
            BranchPatterns.Remove(bpToDelete);
            CurrentBranchPattern = null;
        }

        IEnumerable<BranchPattern> IBranchPatternPolicyEditorViewModel.GetBranchPatterns()
        {
            return BranchPatterns.ToList();
        }

    }
}
