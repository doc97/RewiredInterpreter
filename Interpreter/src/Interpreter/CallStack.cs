using System;
using System.Collections.Generic;

namespace Rewired.Interpreter {

    /// <summary>
    /// A light wrapper for a stack containing <see href="ActivationRecords" />.
    /// </summary>
    public class CallStack {

        /// <summary>
        /// Gets the number of elements stored in the stack.
        /// </summary>
        public int Count { get => stack.Count; }

        /// <summary>
        /// The container for the activation records.
        /// </summary>
        private Stack<ActivationRecord> stack;

        /// <summary>
        /// Initializes a new, empty call stack.
        /// </summary>
        public CallStack() {
            stack = new Stack<ActivationRecord>();
        }

        /// <summary>
        /// Pushes a non-null record on top of the stack.
        /// </summary>
        /// <param name="record">The record to push</param>
        /// <exception href="ArgumentNullException">
        /// Throws if trying to push a null record.
        /// </exception>
        public void Push(ActivationRecord record) {
            if (record == null) {
                throw new ArgumentNullException("record");
            }
            stack.Push(record);
        }

        /// <summary>
        /// Pops the element at the top of the stack. If the stack
        /// is empty, returns null instead.
        /// </summary>
        /// <returns>The record or null if the stack is empty.</returns>
        public ActivationRecord Pop() {
            ActivationRecord record;
            if (stack.TryPop(out record)) {
                return record;
            }
            return null;
        }

        /// <summary>
        /// Returns the element at the top of the stack without removing it.
        /// If the stack is empty, it returns null.
        /// </summary>
        /// <returns>The record or null if the stack is empty.</returns>
        public ActivationRecord Peek() {
            ActivationRecord record;
            if (stack.TryPeek(out record)) {
                return record;
            }
            return null;
        }
    }

}