using System.Collections.Generic;
using System.Text;

namespace Rewired.Interpreter {

    public class SymbolTable {

        private Dictionary<string, Symbol> symbols;

        public SymbolTable() {
            symbols = new Dictionary<string, Symbol>();
        }

        public void Insert(Symbol symbol) {
            symbols[symbol.Name] = symbol;
        }

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