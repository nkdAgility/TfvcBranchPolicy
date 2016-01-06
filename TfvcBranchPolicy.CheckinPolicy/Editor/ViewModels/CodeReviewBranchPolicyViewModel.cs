using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.Server;
using Microsoft.TeamFoundation.VersionControl.Client;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using TfvcBranchPolicy.CheckinPolicy.Common;

namespace TfvcBranchPolicy.CheckinPolicy.Editor.ViewModels
{
    public class CodeReviewBranchPolicyViewModel : BranchPolicyViewModel
    {

        public IEnumerable<ITeamFoundationTeamViewModel> TeamsToChooseFrom
        {
            get {
                return PopulateTeams();
        }
        }

        private CodeReviewBranchPolicy RawCodeReviewBranchPolicy
        {
            get
            {
                return ((CodeReviewBranchPolicy)RawBranchPolicy);
            }
        }

        public Boolean CodeReviewRequired
        {
            get
            {
                return RawCodeReviewBranchPolicy.CodeReviewRequired;
            }
            set
            {
                RawCodeReviewBranchPolicy.CodeReviewRequired = value;
                RaisePropertyChanged("CodeReviewRequired");
            }
        }

        public int MinimumReviewers
        {
            get
            {
                return RawCodeReviewBranchPolicy.MinimumReviewers;
            }
            set
            {
                RawCodeReviewBranchPolicy.MinimumReviewers = value;
                RaisePropertyChanged("MinimumReviewers");
            }
        }

        public Boolean CanApproveOwnChanges
        {
            get
            {
                return RawCodeReviewBranchPolicy.CanApproveOwnChanges;
            }
            set
            {
                RawCodeReviewBranchPolicy.CanApproveOwnChanges = value;
                RaisePropertyChanged("CanApproveOwnChanges");
            }
        }
        public Boolean CanMergeWithoutReview
        {
            get
            {
                return RawCodeReviewBranchPolicy.CanMergeWithoutReview;
            }
            set
            {
                RawCodeReviewBranchPolicy.CanMergeWithoutReview = value;
                RaisePropertyChanged("CanMergeWithoutReview");
            }
        }
        public CodeReviewBranchPolicyViewModel(IPolicyEditArgs policyEditArgs, IBranchPolicy item) : base(policyEditArgs, item)
        {
        
        }

        private IEnumerable<ITeamFoundationTeamViewModel> PopulateTeams()
        {
            if (this.RawPolicyEditArg != null)
            {
                TfsTeamService tfsts = this.RawPolicyEditArg.TeamProject.TeamProjectCollection.GetService<TfsTeamService>();
                IEnumerable<TeamFoundationTeam> teams = tfsts.QueryTeams(this.RawPolicyEditArg.TeamProject.Name);
                return (from t in teams select new TeamFoundationTeamViewModel(t));
            }
            else
            {
                List<ITeamFoundationTeamViewModel> fakeTeams = new List<ITeamFoundationTeamViewModel>();
                fakeTeams.Add(new TeamFoundationTeamFakeViewModel("Team A"));
                fakeTeams.Add(new TeamFoundationTeamFakeViewModel("Team B"));
                return fakeTeams;
            }
           
        }


    }
}
