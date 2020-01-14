using System.Collections.Generic;
using NUnit.Framework;

namespace Rewired.Interpreter.Tests {

    [TestFixture]
    public class TestActivationRecord {

        [Test]
        public void Count_IsZeroAtStart() {
            ActivationRecord ar = new ActivationRecord(ActivationRecord.Type.Program, "", 1);
            Assert.AreEqual(0, ar.Count);
        }

        [Test]
        public void Count_IsUpdatedWhenOneMemberIsAdded() {
            ActivationRecord ar = new ActivationRecord(ActivationRecord.Type.Program, "", 1);
            ar.Set("a", 0);
            Assert.AreEqual(1, ar.Count);
        }

        [Test]
        public void Count_IsUpdatedWhenTwoMembersAreAdded() {
            ActivationRecord ar = new ActivationRecord(ActivationRecord.Type.Program, "", 1);
            ar.Set("a", 0);
            ar.Set("b", 0);
            Assert.AreEqual(2, ar.Count);
        }

        [Test]
        public void Count_IsUpdatedCorrectlyWhenMembersWithSameNameAreAdded() {
            ActivationRecord ar = new ActivationRecord(ActivationRecord.Type.Program, "", 1);
            ar.Set("a", 0);
            ar.Set("a", 1);
            Assert.AreEqual(1, ar.Count);
        }

        [TestCase(0, ExpectedResult = 0)]
        [TestCase(1, ExpectedResult = 1)]
        [TestCase(2, ExpectedResult = 2)]
        public int Get_IsCorrectValueWhenMemberIsSet(int value) {
            ActivationRecord ar = new ActivationRecord(ActivationRecord.Type.Program, "", 1);
            ar.Set("a", value);
            return (int) ar.Get("a");
        }

        [Test]
        public void Get_IsCorrectValueWhenSameMemberIsSet() {
            ActivationRecord ar = new ActivationRecord(ActivationRecord.Type.Program, "", 1);
            ar.Set("a", 1);
            ar.Set("a", 2);
            Assert.AreEqual(2, ar.Get("a"));
        }

        [Test]
        public void Get_ThrowsExceptionWhenAccessingNonExistingMember() {
            ActivationRecord ar = new ActivationRecord(ActivationRecord.Type.Program, "", 1);
            Assert.Throws<KeyNotFoundException>(() => ar.Get("a"));
        }

        [Test]
        public void ToString_NoMembers() {
            ActivationRecord ar = new ActivationRecord(ActivationRecord.Type.Program, "Test", 1);
            Assert.AreEqual("1: PROGRAM Test\n", ar.ToString());
        }

        [Test]
        public void ToString_WithMembers() {
            ActivationRecord ar = new ActivationRecord(ActivationRecord.Type.Program, "Test", 1);
            ar.Set("a", 1);
            Assert.AreEqual("1: PROGRAM Test\n    a                   : 1\n", ar.ToString());
        }
    }
}