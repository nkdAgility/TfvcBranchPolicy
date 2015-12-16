using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace TfvcBranchPolicy.CheckinPolicy.Common
{
     [Serializable]
    public class BranchPatternReview : ObservableBase
    {
        [OptionalField(VersionAdded = 3)]
        private Boolean _enabled;
        [OptionalField(VersionAdded = 3)]
        private Boolean _required;
        [OptionalField(VersionAdded = 3)]
        private String _pattern;
        [OptionalField(VersionAdded = 3)]
        private ObservableCollection<BranchPatternReviewer> _branchPatternReviewers;
    }
}
