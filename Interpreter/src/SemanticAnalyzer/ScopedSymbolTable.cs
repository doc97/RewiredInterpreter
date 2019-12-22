using System.Collections.Generic;
using System.Text;

namespace Rewired.Interpreter {

    /// <summary>
    /// ScopedSymbolTable is a dictionary wrapper for symbols.
    /// </summary>
    public class ScopedSymbolTable {

        /// <summary>
        /// The container for the symbols.
        /// </summary>
        private Dictionary<string, Symbol> symbols;

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
        }

        /// <summary>
        /// Adds built-in types to the symbol table.
        /// </summary>
        public void InitBuiltInTypes() {
            Insert(new BuiltInTypeSymbol("INTEGER"));
            Insert(new BuiltInTypeSymbol("REAL"));
        }

        /// <summary>
        /// Adds a symbol to the table.
        /// </summary>
        /// <param name="symbol"></param>
        public void Insert(Symbol symbol) {
            symbols[symbol.Name] = symbol;
        }

        /// <summary>
        /// Searches for a symbol by name in the current scope and if not found,
        /// it searches for it in the parent scope.
        /// </summary>
        /// <param name="name">The name to search for</param>
        /// <returns>The stored symbol or null if the name cannot be found.</returns>
        public Symbol Lookup(string name) {
            if (symbols.ContainsKey(name)) {
                return symbols[name];
            }

            return Parent?.Lookup(name);
        }

        public override string ToString() {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("{0, -12} | {1}\n", "Name", "Symbol");
            sb.Append("---------------------------------------------------------------\n");
            foreach (KeyValuePair<string, Symbol> pair in symbols) {
                sb.AppendFormat("{0, -12} | {1}\n", pair.Key, pair.Value, "\n");
            }
            return sb.ToString();
        }
    }
}