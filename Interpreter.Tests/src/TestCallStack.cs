
using NUnit.Framework;

namespace Rewired.Interpreter.Tests {

    [TestFixture]
    public class TestCallStack {

        [Test]
        public void Count_IsZeroAtStart() {
            CallStack stack = new CallStack();
            Assert.AreEqual(0, stack.Count);
        }

        [Test]
        public void Count_IsUpdatedWhenOneRecordIsAdded() {
            CallStack stack = new CallStack();
            stack.Push(new ActivationRecord(ActivationRecord.Type.Program, "", 1));
            Assert.AreEqual(1, stack.Count);
        }

        [Test]
        public void Count_IsUpdatedWhenTwoRecordsAreAdded() {
            CallStack stack = new CallStack();
            stack.Push(new ActivationRecord(ActivationRecord.Type.Program, "", 1));
            stack.Push(new ActivationRecord(ActivationRecord.Type.Program, "", 1));
            Assert.AreEqual(2, stack.Count);
        }

        [Test]
        public void Pop_ReturnsNullWhenEmpty() {
            CallStack stack = new CallStack();
            Assert.IsNull(stack.Pop());
        }

        public void Peek_ReturnsNullWhenEmpty() {
            CallStack stack = new CallStack();
            Assert.IsNull(stack.Peek());
        }

    }

}