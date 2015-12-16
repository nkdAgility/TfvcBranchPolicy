using Microsoft.TeamFoundation.VersionControl.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using Microsoft.ApplicationInsights;
using System.Diagnostics;
using TfvcBranchPolicy.Common;

namespace TfvcBranchPolicy.CheckinPolicy
{
    [Serializable]
    public abstract class CustomPolicyBase : PolicyBase
    {

        /// <summary>
        /// Dispose method unsubscribes to the event so we don't get into 
        /// scenarios that can create null ref exceptions
        /// </summary>
        public override void Dispose()
        {
            base.Dispose();
            if (TellMe.Instance != null)
            {
                TellMe.Instance.Flush(); // only for desktop apps
                // Allow time for flushing:
                System.Threading.Thread.Sleep(1000);
            }
        }

        /// <summary>
        /// Initializes this policy for the specified pending checkin.
        /// </summary>
        /// <param name="pendingCheckin" />The pending checkin.</param>
        public override void Initialize(IPendingCheckin pendingCheckin)
        {
            try
            {
                base.Initialize(pendingCheckin);
            }
            catch (Exception ex)
            {
                TellMe.Instance.TrackException(ex);
                throw ex;
            }
            base.InstallationInstructions = "Download policy from http://somewhere";
        }

    }
}
