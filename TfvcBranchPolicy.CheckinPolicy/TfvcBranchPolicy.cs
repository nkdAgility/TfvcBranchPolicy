using Microsoft.TeamFoundation.VersionControl.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.Integration;
using System.Collections.ObjectModel;
using Microsoft.ApplicationInsights;
using System.Diagnostics;
using TfvcBranchPolicy.CheckinPolicy.Common;
using TfvcBranchPolicy.Common;
using System.Runtime.Serialization;
using TfvcBranchPolicy.CheckinPolicy.Editor;
using System.Runtime.Serialization.Formatters.Binary;
using Microsoft.ApplicationInsights.DataContracts;

namespace TfvcBranchPolicy.CheckinPolicy
{
    [Serializable]
    public class TfvcBranchPolicy : CustomPolicyBase
    {
        [NonSerialized]
        IPendingCheckin _pendingCheckin;

        [OptionalField(VersionAdded = 2)]
        public List<BranchPattern> branchPatterns;

        /// <summary>
        /// Gets a value indicating whether this check-in policy has an editable configuration
        /// </summary>
        /// <value><c>true</c> if this instance can edit; otherwise, <c>false</c>.</value>
        public override bool CanEdit
        {
            get
            {
                return true;
            }
        }

        public override string InstallationInstructions
        {
            get
            {
                return "You need to install the TFVC Branch Policy to continue.\n\n" +
                       " . \n Download from http://nkdalm.net/TfvcBranchPolicy";
            }
            set
            {
                base.InstallationInstructions = value;
            }
        }

        public override void DisplayHelp(PolicyFailure failure)
        {


            System.Diagnostics.Process.Start("https://github.com/nkdAgility/TfvcBranchPolicy/wiki");
        }

        /// <summary>
        /// Gets the description.
        /// </summary>
        /// <value>The description.</value>
        public override string Description
        {
            get { return "Applied policies to folder paths in source control"; }
        }

        /// <summary>
        /// Gets the type.
        /// <value>The type.</value>
        public override string Type
        {
            get { return string.Format("Branch Policies v{0}", System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString(3)); }
        }

        /// <summary>
        /// Gets the type description.
        /// </summary>
        /// <value>The type description.</value>
        public override string TypeDescription
        {
            get { return "This policy applies other policies to a specific path determined by a regular ecpression."; }
        }

        /// <summary>
        /// Dispose method unsubscribes to the event so we don't get into 
        /// scenarios that can create null ref exceptions
        /// </summary>
        public override void Dispose()
        {
            base.Dispose();
            if (_pendingCheckin != null)
            {
                _pendingCheckin.PendingChanges.CheckedPendingChangesChanged -= PendingChanges_CheckedPendingChangesChanged;
            }
            /// </summary>Changed -= PendingChanges_CheckedPendingChangesChanged;
        }

        public override bool Edit(IPolicyEditArgs policyEditArgs)
        {
            PageViewTelemetry pvt = new PageViewTelemetry("BranchPolicyEdit");
            Stopwatch pvtrak = new Stopwatch();
            pvtrak.Start();
            bool result = false;
            try
            {
                IBranchPatternsRepository repo = new BranchPatternsRepository(branchPatterns);
                var wpfwindow = new BranchLockPolicyEditorWindow(policyEditArgs, repo);
                ElementHost.EnableModelessKeyboardInterop(wpfwindow);
                wpfwindow.ShowDialog();
                branchPatterns = repo.FindAll().ToList();
                TellMe.Instance.TrackMetric("BranchPolicyCount", branchPatterns.Count);
                result= true;
            }
            catch (Exception ex)
            {
                TellMe.Instance.TrackException(ex);
                MessageBox.Show(string.Format("There was an error loading the Edit Vew for BranchPatternPolicy:/n/n {0}", ex.Message));
                result= false;
            }
            pvt.Duration = pvtrak.Elapsed;
            TellMe.Instance.TrackPageView(pvt);
            return result;
        }

        /// <summary>
        /// Evaluates the pending changes for policy violations
        /// </summary>
        /// <returns></returns>
        public override PolicyFailure[] Evaluate()
        {
            try
            {
                TellMe.Instance.TrackEvent("BranchPolicyEvaluate");
                List<PolicyFailure> policyFailures = new List<PolicyFailure>();
                Stopwatch partialTimer = new Stopwatch();
                Stopwatch wholeProcessTimer = new Stopwatch();
                wholeProcessTimer.Start();

                BranchPatternEvaluator ev = new BranchPatternEvaluator();
                // process each file in the set of pending changes
                if (_pendingCheckin != null)
                {
                    partialTimer.Start();
                    List<String> serverItems = (from x in PendingCheckin.PendingChanges.CheckedPendingChanges select x.ServerItem).ToList();
                    TellMe.Instance.TrackMetric("PendingCheckinRetrieve", partialTimer.ElapsedMilliseconds);
                    TellMe.Instance.TrackMetric("PendingCheckinCount", serverItems.Count);
                    List<BranchPattern> matchingBranchPatterns = ev.EvaluateServerPaths(serverItems, branchPatterns);
                    TellMe.Instance.TrackMetric("BranchPolicyDetectedCount", matchingBranchPatterns.Count);
                    List<BranchPolicyFailure> branchPolicyFailures = new List<BranchPolicyFailure>();
                    foreach (BranchPattern bp in matchingBranchPatterns)
                    {
                     branchPolicyFailures  = bp.EvaluatePendingCheckin(_pendingCheckin );
                    }
                    policyFailures = (from bpf in branchPolicyFailures select new PolicyFailure(bpf.Message, this)).ToList();
                }
                TellMe.Instance.TrackMetric("BranchPolicyEvaluateTimespan", wholeProcessTimer.ElapsedMilliseconds);
                return policyFailures.ToArray();
            }
            catch (Exception ex)
            {
                TellMe.Instance.TrackException(ex);
                throw ex;
            }
            return new PolicyFailure[] { new PolicyFailure(string.Format("There was an error evaluating the policy. Pass the session ID {0} to your administrator.", TellMe.Instance.Context.Session.Id), this) };

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
                _pendingCheckin = pendingCheckin;
                TellMe.Instance.Context.Properties["PolicyType"] = this.Type;
                if (pendingCheckin != null)
                {
                    pendingCheckin.PendingChanges.CheckedPendingChangesChanged += PendingChanges_CheckedPendingChangesChanged;
                }
            }
            catch (Exception ex)
            {
                TellMe.Instance.TrackException(ex);
                throw ex;
            }
        }


        /// <summary>
        /// Subscribes to the PendingChanges_CheckedPendingChangesChanged event 
        /// and reevaluates the policy. You should not do this if your policy takes 
        /// a long time to evaluate since this will get fired every time there is a
        /// change to one of the items in the pending changes window.
        /// </summary>
        /// <param name="sender" />The source of the event.</param>
        /// <param name="e" />The <see cref="System.EventArgs" /> instance containing the event data.</param>
        void PendingChanges_CheckedPendingChangesChanged(object sender, EventArgs e)
        {
            if (!Disposed)
            {
                OnPolicyStateChanged(Evaluate());
            }
        }

    }
}
