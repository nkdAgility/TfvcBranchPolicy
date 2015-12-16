using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TfvcBranchPolicy.CheckinPolicy.Common
{
   public class BranchPolicyFailure
    {
       public string Message { get; set; }

       public BranchPolicyFailure(string message)
       {
           // TODO: Complete member initialization
           this.Message = message;
       }
    }
}
