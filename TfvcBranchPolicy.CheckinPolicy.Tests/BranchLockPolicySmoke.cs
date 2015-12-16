using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Collections.Generic;
using TfvcBranchPolicy.CheckinPolicy.Common;

namespace TfvcBranchPolicy.CheckinPolicy.Tests
{
    [TestClass]
    public class BranchLockPolicySmoke
    {
        [TestMethod]
        public void BranchLockPolicyCreateSmokeTest()
        {
            TfvcBranchPolicy bp = new TfvcBranchPolicy();
            Assert.IsNotNull(bp);
        }

        [TestMethod]
        public void BranchLockPolicyCreateSmokeTest2()
        {
            TfvcBranchPolicy bp = new TfvcBranchPolicy();
            bp.Initialize(null);
            Assert.IsNotNull(bp);
        }

        [TestMethod]
        public void BranchPolicuSerilizationTest()
        {
            TfvcBranchPolicy bp = new TfvcBranchPolicy();
            bp.Initialize(null);
            bp.branchPatterns = new List<BranchPattern>();
            bp.branchPatterns.Add(new BranchPattern("^.*"));
            BinaryFormatter bf = bp.GetBinaryFormatter();
            Stream stream = new MemoryStream();
            bf.Serialize(stream, bp);
            stream.Position= 0;
            bf.Deserialize(stream);
            Assert.IsNotNull(bp);
        }

        //[TestMethod]
        //public void BranchPolicyEvaluation()
        //{
        //    BranchCheckinPolicy bp = new BranchCheckinPolicy();
        //    bp.Initialize(new MockPendingChange());
        //    bp.branchPatterns = new List<BranchPattern>();
        //    bp.branchPatterns.Add(new BranchPattern("^.*"));
        //    bp.Evaluate();
        //}

    }
}
