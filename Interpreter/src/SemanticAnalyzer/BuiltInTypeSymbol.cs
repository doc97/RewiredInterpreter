namespace Rewired.Interpreter {

    public class BuiltInTypeSymbol : Symbol {

        public BuiltInTypeSymbol(string name) : base(name) { }

        public override string ToString() {
            return string.Format("<BuiltInTypeSymbol(name='{0}')>", Name);
        }
    }

}