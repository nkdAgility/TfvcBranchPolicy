using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TfvcBranchPolicy.CheckinPolicy.Common
{
    public class BranchPatternFailure
    {
        public BranchPatternFailure(string lockedPath, string serverItem)
        {
            this.ServerItem = serverItem;
            this.Count = 1;
            this.LockedPath = lockedPath;
        }

        public int Count { get; set; }
        public string ServerItem { get; set; }
        public string LockedPath { get; set; }
    }
}
