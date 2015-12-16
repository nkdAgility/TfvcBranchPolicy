using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TfvcBranchPolicy.CheckinPolicy.Common
{
    public class BranchPatternEvaluator
    {

        public BranchPatternEvaluator()
        {

        }

        Dictionary<string, BranchPatternFailure> _failures = new Dictionary<string, BranchPatternFailure>();

        public List<BranchPattern> EvaluatePath(string serverItem, List<BranchPattern> branchPolicies)
        {
            List<BranchPattern> bps = new List<BranchPattern>();
            foreach (var branchPolicy in branchPolicies)
            {
                if (Regex.IsMatch(serverItem, branchPolicy.Pattern))
                {
                    if (!_failures.ContainsKey(branchPolicy.Pattern))
                    {
                        bps.Add(branchPolicy);
                        _failures.Add(branchPolicy.Pattern, new BranchPatternFailure(branchPolicy.Pattern, serverItem));
                    }
                    else
                    {
                        _failures[branchPolicy.Pattern].Count++;
                    }
                }
            }
            return bps;
        }

        public List<BranchPattern> EvaluateServerPaths(List<string> serverItems, List<BranchPattern> branchPolicies)
        {
            List<BranchPattern> bps = new List<BranchPattern>();
            foreach (string serverItem in serverItems)
            {
                bps.AddRange(this.EvaluatePath(serverItem, branchPolicies));
            }
            return bps.Distinct().ToList() ;
        }
        
        public List<BranchPatternFailure> Failures { get { return _failures.Values.ToList(); } }

     
    }
}
