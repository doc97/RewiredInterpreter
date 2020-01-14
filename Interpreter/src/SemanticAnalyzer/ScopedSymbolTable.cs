using System.Collections.Generic;
using System.Text;

namespace Rewired.Interpreter {

    /// <summary>
    /// ScopedSymbolTable is a dictionary wrapper for symbols.
    /// </summary>
    public class ScopedSymbolTable {

        /// <summary>
        /// The container for non-variable symbols.
        /// </summary>
        private Dictionary<string, Symbol> symbols;

        /// <summary>
        /// The container for variable symbols.
        /// </summary>
        private Dictionary<string, Symbol> variables;

        /// <summary>
        /// Gets the name of the symbol table.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the scope level of the symbol table.
        /// </summary>
        /// <value>Level 1 is the global scope</value>
        public int Level { get; }

        /// <summary>
        /// Gets the parent scope.
        /// </summary>
        public ScopedSymbolTable Parent { get; }

        /// <summary>
        /// Instantiates a new empty instance.
        /// </summary>
        public ScopedSymbolTable(string name, int level, ScopedSymbolTable parent) {
            Name = name;
            Level = level;
            Parent = parent;
            symbols = new Dictionary<string, Symbol>();
            variables = new Dictionary<string, Symbol>();
        }

        /// <summary>
        /// Adds built-in types to the symbol table.
        /// </summary>
        public void InitBuiltInTypes() {
            InsertSymbol(new BuiltInTypeSymbol("int"));
            InsertSymbol(new BuiltInTypeSymbol("float"));
            InsertSymbol(new BuiltInTypeSymbol("bool"));
        }

        /// <summary>
        /// Adds a non-variable symbol to the table.
        /// </summary>
        /// <param name="symbol">The symbol to add</param>
        public void InsertSymbol(Symbol symbol) {
            symbols[symbol.Name] = symbol;
        }

        /// <summary>
        /// Adds a variable symbol to the table.
        /// </summary>
        /// <param name="symbol">The symbol to add</param>
        public void InsertVariable(Symbol variable) {
            variables[variable.Name] = variable;
        }

        /// <summary>
        /// Searches for a non-variable symbol by name in the current scope and if not found,
        /// it searches for it in the parent scope.
        /// </summary>
        /// <param name="name">The name to search for</param>
        /// <returns>The stored symbol or null if the name cannot be found.</returns>
        public Symbol LookupSymbol(string name) {
            return HasSymbol(name) ? symbols[name] : Parent?.LookupSymbol(name);
        }

        /// <summary>
        /// Searches for a variable symbol by name in the current scope (only).
        /// </summary>
        /// <param name="name">The name to search for</param>
        /// <returns>The stored symbol or null if the name cannot be found.</returns>
        public Symbol LookupVariable(string name) {
            return HasVariable(name) ? variables[name] : null;
        }

        /// <summary>
        /// Checks whether a non-variable symbol with the name exists.
        /// </summary>
        /// <param name="name">The name to check for</param>
        /// <returns>True if found, false otherwise</returns>
        public bool HasSymbol(string name) {
            return symbols.ContainsKey(name);
        }

        /// <summary>
        /// Checks whether a variable symbol with the name exists.
        /// </summary>
        /// <param name="name">The name to check for</param>
        /// <returns>True if found, false otherwise</returns>
        public bool HasVariable(string name) {
            return variables.ContainsKey(name);
        }

        public override string ToString() {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("-------------------------------------------------=( VARIABLES )=--");
            sb.AppendFormat("{0, -12} | {1}\n\n", "Name", "Symbol");
            foreach (KeyValuePair<string, Symbol> pair in variables) {
                sb.AppendFormat("{0, -12} | {1}\n", pair.Key, pair.Value, "\n");
            }
            sb.AppendLine("-----------------------------------------------------=( OTHER )=--");
            sb.AppendFormat("{0, -12} | {1}\n\n", "Name", "Symbol");
            foreach (KeyValuePair<string, Symbol> pair in symbols) {
                sb.AppendFormat("{0, -12} | {1}\n", pair.Key, pair.Value, "\n");
            }
            sb.AppendLine("------------------------------------------------------------------");
            return sb.ToString();
        }
    }
}