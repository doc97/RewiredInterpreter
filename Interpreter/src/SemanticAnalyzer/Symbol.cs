namespace Rewired.Interpreter {

    public class Symbol {

        public string Name { get; }
        public Symbol Type { get; }
        public string TypeName { get => Type?.Name ?? ""; }

        public Symbol(string name, Symbol type = null) {
            Name = name;
            Type = type;
        }
    }

}