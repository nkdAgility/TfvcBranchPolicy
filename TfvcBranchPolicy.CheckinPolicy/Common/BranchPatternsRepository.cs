using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TfvcBranchPolicy.CheckinPolicy.Common
{
    public class BranchPatternsRepository: IBranchPatternsRepository
    {
        private readonly List<BranchPattern> _collection;

         public BranchPatternsRepository(IEnumerable<BranchPattern> collection)
        {
            if (collection == null)
            {
                collection = new List<BranchPattern>();
            }
            _collection = collection.ToList();
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
             _collection.Add(branchPattern);
         }

         public void Remove(BranchPattern branchPattern)
         {
             _collection.Remove(branchPattern);
         }

         public BranchPattern Get(string id)
         {
             return (from bp in _collection where bp.Name == id select bp).SingleOrDefault();
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
