namespace Rewired.Interpreter {

    public class VarSymbol : Symbol {

        public VarSymbol(string name, Symbol type) : base(name, type) { }

        public override string ToString() {
            return string.Format("<VarSymbol(name='{0}', type='{1}')>", Name, Type.Name);
        }
    }

}