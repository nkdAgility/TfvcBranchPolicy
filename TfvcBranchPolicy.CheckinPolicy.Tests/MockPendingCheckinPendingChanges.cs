using Microsoft.TeamFoundation.VersionControl.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TfvcBranchPolicy.CheckinPolicy.Tests
{
    class MockPendingCheckinPendingChanges : IPendingCheckinPendingChanges
    {
        public string[] AffectedTeamProjectPaths
        {
            get { throw new NotImplementedException(); }
        }

        public event AffectedTeamProjectsEventHandler AffectedTeamProjectsChanged;

        public PendingChange[] AllPendingChanges
        {
            get { throw new NotImplementedException(); }
        }

        public PendingChange[] CheckedPendingChanges
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public event EventHandler CheckedPendingChangesChanged;

        public string Comment
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public Workspace Workspace
        {
            get { throw new NotImplementedException(); }
        }
    }
}
