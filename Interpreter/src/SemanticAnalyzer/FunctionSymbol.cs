namespace Rewired.Interpreter {

    public class FunctionSymbol : Symbol {

        private VarSymbol[] parameters;

        public FunctionSymbol(string name, params VarSymbol[] parameters) : base(name) {
            this.parameters = parameters;
        }
    }
}