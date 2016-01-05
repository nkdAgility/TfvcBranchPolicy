using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TfvcBranchPolicy.CheckinPolicy.Editor.ViewModels
{
    public class TeamFoundationTeamFakeViewModel : ViewModelBase, ITeamFoundationTeamViewModel
    {

        string _name;

        public string Name
        {
            get { return _name; }
        }

         public TeamFoundationTeamFakeViewModel(string name)
        {
            _name = name;
        }
    }
}
