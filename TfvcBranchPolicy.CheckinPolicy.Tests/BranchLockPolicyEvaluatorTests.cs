using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using TfvcBranchPolicy.CheckinPolicy.Common;

namespace TfvcBranchPolicy.CheckinPolicy.Tests
{
    [TestClass]
    public class BranchLockPolicyEvaluatorTests
    {

        BranchPatternEvaluator evaluator; 

        [TestInitialize]
        public void Init()
        {
            evaluator = new BranchPatternEvaluator();
        }

        private List<BranchPattern> CreateSingleAlwaysMatch()
        {
            List<BranchPattern> lockedPaths = new List<BranchPattern>();
            lockedPaths.Add(new BranchPattern("^.*"));
            return lockedPaths;
        }

        private List<BranchPattern> CreateListOfLocks()
        {
            List<BranchPattern> lockedPaths = new List<BranchPattern>();
            lockedPaths.Add(new BranchPattern("^.*/2013/.*"));
            lockedPaths.Add(new BranchPattern("^.*/2014/.*"));
            lockedPaths.Add(new BranchPattern("^.*/2015/2015.1/.*"));
            lockedPaths.Add(new BranchPattern("^*.vb"));
            return lockedPaths;
        }


        [TestMethod]
        public void EvaluateOnePath()
        {
            List<BranchPattern> lockedPaths = CreateSingleAlwaysMatch();
            List<BranchPattern> matchingPolicies = evaluator.EvaluatePath("$/Petrel/DevOpsTools/TfvcBranchPolicy/2013/TfvcBranchPolicy.CheckinPolicy.Tests/fubar.cs", lockedPaths);
            Assert.AreEqual(1, evaluator.Failures.Count);
            Assert.AreEqual(1, matchingPolicies.Count);
        }

        [TestMethod]
        public void EvaluateTwoPaths()
        {
            List<BranchPattern> lockedPaths = CreateSingleAlwaysMatch();
            evaluator.EvaluatePath("$/Petrel/DevOpsTools/TfvcBranchPolicy/2013/TfvcBranchPolicy.CheckinPolicy.Tests/fubar.cs", lockedPaths);
            evaluator.EvaluatePath("$/Petrel/DevOpsTools/TfvcBranchPolicy/2014/TfvcBranchPolicy.BranchLockPolicy/fubar.cs", lockedPaths);
            Assert.AreEqual(1, evaluator.Failures.Count);
            Assert.AreEqual(2, evaluator.Failures[0].Count);
        }

        [TestMethod]
        public void EvaluateThreePathsOnePass()
        {
            List<BranchPattern> lockedPaths = CreateListOfLocks();
            evaluator.EvaluatePath("$/Petrel/DevOpsTools/TfvcBranchPolicy/2013/TfvcBranchPolicy.CheckinPolicy.Tests/fubar.cs", lockedPaths);
            evaluator.EvaluatePath("$/Petrel/DevOpsTools/TfvcBranchPolicy/2014/2014/TfvcBranchPolicy.BranchLockPolicy/fubar.cs", lockedPaths);
            evaluator.EvaluatePath("$/Petrel/DevOpsTools/TfvcBranchPolicy/2014/2014.3/TfvcBranchPolicy.BranchLockPolicy/fubar.cs", lockedPaths);
            evaluator.EvaluatePath("$/Petrel/DevOpsTools/TfvcBranchPolicy/2015/2015.1/TfvcBranchPolicy.BranchLockPolicy/fubar.vb", lockedPaths);
            evaluator.EvaluatePath("$/Petrel/DevOpsTools/TfvcBranchPolicy/2015/TfvcBranchPolicy.BranchLockPolicy/fubar.cs", lockedPaths);
            Assert.AreEqual(4, evaluator.Failures.Count);
            Assert.AreEqual(1, evaluator.Failures[0].Count);
            Assert.AreEqual(2, evaluator.Failures[1].Count);
            Assert.AreEqual(1, evaluator.Failures[2].Count);
            Assert.AreEqual(1, evaluator.Failures[3].Count);
        } 
    }
}
