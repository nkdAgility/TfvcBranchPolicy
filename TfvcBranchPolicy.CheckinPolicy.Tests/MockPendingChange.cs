using Microsoft.TeamFoundation.VersionControl.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TfvcBranchPolicy.CheckinPolicy.Tests
{
    class MockPendingChange : IPendingCheckin
    {
        public IPendingCheckinNotes CheckinNotes
        {
            get { throw new NotImplementedException(); }
        }

        public IPendingCheckinPendingChanges PendingChanges
        {
            get { return new MockPendingCheckinPendingChanges(); }
        }

        public IPendingCheckinPolicies Policies
        {
            get { throw new NotImplementedException(); }
        }

        public IPendingCheckinWorkItems WorkItems
        {
            get { throw new NotImplementedException(); }
        }

        public object GetService(Type serviceType)
        {
            throw new NotImplementedException();
        }
    }
}
