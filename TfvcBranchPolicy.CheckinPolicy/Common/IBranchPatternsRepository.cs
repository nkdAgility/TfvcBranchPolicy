using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TfvcBranchPolicy.CheckinPolicy.Common
{
    public interface IBranchPatternsRepository : IRepository<BranchPattern, string>
    {
        IEnumerable<BranchPattern> FindAll();
        IEnumerable<BranchPattern> Find(string text);

        void Add(BranchPattern branchPattern);

        void Remove(BranchPattern branchPattern);
    }
}
