using GalaSoft.MvvmLight;
using Microsoft.TeamFoundation.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TfvcBranchPolicy.CheckinPolicy.Editor.ViewModels
{
    public class TeamFoundationTeamViewModel : ViewModelBase, ITeamFoundationTeamViewModel
    {
        private TeamFoundationTeam _rawTeamFoundationTeam;

public string Name
        {
            get
            {
                return _rawTeamFoundationTeam.Name;
            }
        }

        public TeamFoundationTeamViewModel(TeamFoundationTeam team)
        {
            _rawTeamFoundationTeam = team;
        }


    }
}
