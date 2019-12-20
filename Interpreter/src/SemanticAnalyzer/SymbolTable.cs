using System.Collections.Generic;
using System.Text;

namespace Rewired.Interpreter {

    /// <summary>
    /// SymbolTable is a dictionary wrapper for symbols.
    /// </summary>
    public class SymbolTable {

        /// <summary>
        /// The container for the symbols.
        /// </summary>
        private Dictionary<string, Symbol> symbols;

        /// <summary>
        /// Instantiates a new empty instance.
        /// </summary>
        public SymbolTable() {
            symbols = new Dictionary<string, Symbol>();
        }

        /// <summary>
        /// Adds a symbol to the table.
        /// </summary>
        /// <param name="symbol"></param>
        public void Insert(Symbol symbol) {
            symbols[symbol.Name] = symbol;
        }

        /// <summary>
        /// Gets a symbol by name.
        /// </summary>
        /// <param name="name">The name to search for</param>
        /// <returns>The stored symbol or null if the name cannot be found.</returns>
        public Symbol Lookup(string name) {
            if (symbols.ContainsKey(name)) {
                return symbols[name];
            }
            return null;
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