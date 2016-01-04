using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TfvcBranchPolicy.CheckinPolicy.Common
{
    public class BranchPatternsRepository: IBranchPatternsRepository
    {
        private readonly IEnumerable<BranchPattern> _collection;

         public BranchPatternsRepository(IEnumerable<BranchPattern> collection)
        {
            _collection = collection;
        }

         public IEnumerable<BranchPattern> FindAll()
         {
             return _collection;
         }

         public IEnumerable<BranchPattern> Find(string text)
         {
             throw new NotImplementedException();
         }

         public void Add(BranchPattern branchPattern)
         {
             throw new NotImplementedException();
         }

         public void Remove(BranchPattern branchPattern)
         {
             throw new NotImplementedException();
         }

         public BranchPattern Get(string id)
         {
             throw new NotImplementedException();
         }

         public void Save(BranchPattern entity)
         {
             throw new NotImplementedException();
         }

         public void Delete(BranchPattern entity)
         {
             throw new NotImplementedException();
         }
    }
}
