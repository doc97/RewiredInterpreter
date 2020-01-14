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
            Program,
            Function,
        }

        /// <summary>
        /// Gets the number of members stored in the activation record.
        /// </summary>
        public int Count { get => intMembers.Count + boolMembers.Count; }

        /// <summary>
        /// Holds the stored integer members.
        /// </summary>
        private Dictionary<string, int> intMembers;

        /// <summary>
        /// Holds the stored boolean members.
        /// </summary>
        private Dictionary<string, bool> boolMembers;

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
            intMembers = new Dictionary<string, int>();
            boolMembers = new Dictionary<string, bool>();
        }

        /// <summary>
        /// Stores a member in the activation record.
        /// </summary>
        /// <param name="name">The name of the member</param>
        /// <param name="value">The value of the member</param>
        public void Set(string name, object value) {
            if (value is int) {
                intMembers[name] = (int) value;
            } else if (value is bool) {
                boolMembers[name] = (bool) value;
            } else {
                throw new System.InvalidCastException("Interpreter encountered an unknown type: " + value.GetType());
            }
        }

        /// <summary>
        /// Gets the value of a stored integer member by name.
        /// </summary>
        /// <param name="name">The name of the member</param>
        /// <returns>The value of the member</returns>
        /// <exception cref="KeyNotFoundException">
        /// Throws if no member with the name has been stored.
        /// </exception>
        public object Get(string name) {
            if (intMembers.ContainsKey(name)) {
                return intMembers[name];
            } else if (boolMembers.ContainsKey(name)) {
                return boolMembers[name];
            }

            throw new KeyNotFoundException("No such variable");
        }

        /// <summary>
        /// Tries to get the value of a stored integer member by name.
        /// </summary>
        /// <param name="name">The name of the member</param>
        /// <param name="member">The value of the member</param>
        /// <param name="def">The value of the member parameter if this function returns false</param>
        /// <returns>True if found, false otherwise</returns>
        public bool TryGet(string name, out int member, int def = 0) {
            if (intMembers.ContainsKey(name)) {
                member = intMembers[name];
                return true;
            }
            member = def;
            return false;
        }

        /// <summary>
        /// Tries to get the value of a stored boolean member by name.
        /// </summary>
        /// <param name="name">The name of the member</param>
        /// <param name="member">The value of the member</param>
        /// <param name="def">The value of the member parameter if this function returns false</param>
        /// <returns>True if found, false otherwise</returns>
        public bool TryGet(string name, out bool member, bool def = false) {
            if (boolMembers.ContainsKey(name)) {
                member = boolMembers[name];
                return true;
            }
            member = def;
            return false;
        }

        public override string ToString() {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("{0}: {1} {2}\n", nestingLevel, type.ToString().ToUpper(), name);
            foreach (KeyValuePair<string, int> pair in intMembers) {
                sb.AppendFormat("    {0,-20}: {1}\n", pair.Key, pair.Value);
            }
            foreach (KeyValuePair<string, bool> pair in boolMembers) {
                sb.AppendFormat("    {0,-20}: {1}\n", pair.Key, pair.Value);
            }
            return sb.ToString();

        }
    }

}