using System.Text;
using System.Collections.Generic;

namespace Rewired.Interpreter {

    /// <summary>
    /// Also called a stack frame, the activation record represents a context
    /// in which code and functions run. It contains all the declared members.
    /// </summary>
    public class ActivationRecord {

        /// <summary>
        /// The type of activation record, like a program or a function.
        /// </summary>
        public enum Type {
            Program
        }

        /// <summary>
        /// Gets the number of members stored in the activation record.
        /// </summary>
        public int Count { get => members.Count; }

        /// <summary>
        /// Holds the stored members.
        /// </summary>
        private Dictionary<string, int> members;

        /// <summary>
        /// The name of the function or program to which the activation
        /// record belongs.
        /// </summary>
        private string name;

        /// <summary>
        /// Holds the current activation record type.
        /// </summary>
        private Type type;

        /// <summary>
        /// The nesting level of the record, 1 for the top-level program.
        /// </summary>
        private int nestingLevel;

        /// <summary>
        /// Instantiates a new instance with a type, name and nesting level. 
        /// </summary>
        /// <param name="type">The type of activation record</param>
        /// <param name="name">The name of the activation record</param>
        /// <param name="nestingLevel">The level of nesting, 1 for a top-level program</param>
        public ActivationRecord(Type type, string name, int nestingLevel) {
            this.type = type;
            this.name = name;
            this.nestingLevel = nestingLevel;
            members = new Dictionary<string, int>();
        }

        /// <summary>
        /// Stores a member in the activation record.
        /// </summary>
        /// <param name="name">The name of the member</param>
        /// <param name="value">The value of the member</param>
        public void Set(string name, int value) {
            members[name] = value;
        }

        /// <summary>
        /// Gets a stored member by name.
        /// </summary>
        /// <param name="name">The name of the member to retrieve</param>
        /// <returns>The value of the member</returns>
        /// <exception href="KeyNotFoundException">
        /// Throws if no member with the name has been stored.
        /// </exception>
        public int Get(string name) {
            return members[name];
        }

        public override string ToString() {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("{0}: {1} {2}\n", nestingLevel, type.ToString().ToUpper(), name);
            foreach (KeyValuePair<string, int> pair in members) {
                sb.AppendFormat("    {0,-20}: {1}\n", pair.Key, pair.Value);
            }
            return sb.ToString();

        }
    }

}