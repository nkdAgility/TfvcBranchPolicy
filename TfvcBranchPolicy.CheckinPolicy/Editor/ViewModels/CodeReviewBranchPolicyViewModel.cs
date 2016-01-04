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

        public IEnumerable<TeamFoundationTeam> TeamsToChooseFrom
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

        private IEnumerable<TeamFoundationTeam> PopulateTeams()
        {
           //ICommonStructureService3 css = this.RawPolicyEditArg.TeamProject.TeamProjectCollection.GetService<ICommonStructureService3>();
            //css.GetProject(this.RawPolicyEditArg.TeamProject.Name)
            TfsTeamService tfsts = this.RawPolicyEditArg.TeamProject.TeamProjectCollection.GetService<TfsTeamService>();
            return tfsts.QueryTeams(this.RawPolicyEditArg.TeamProject.Name);
        }


    }
}
