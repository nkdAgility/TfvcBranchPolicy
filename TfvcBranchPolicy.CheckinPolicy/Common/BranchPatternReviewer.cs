using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace TfvcBranchPolicy.CheckinPolicy.Common
{
    [Serializable]
    class BranchPatternReviewer
    {
        [OptionalField(VersionAdded = 3)]
        private String _identity;
        [OptionalField(VersionAdded = 3)]
        private Boolean _IsGroup;
    }
}
