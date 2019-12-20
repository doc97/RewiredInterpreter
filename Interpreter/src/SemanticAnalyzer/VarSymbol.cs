namespace Rewired.Interpreter {

    /// <summary>
    /// VarSymbol represents the variable symbol category.
    /// </summary>
    public class VarSymbol : Symbol {

        /// <summary>
        /// Instantiates a new instance with a name and an underlying type.
        /// </summary>
        /// <param name="name">The name of the symbol</param>
        /// <param name="type">The type of the symbol</param>
        /// <returns></returns>
        public VarSymbol(string name, Symbol type) : base(name, type) { }

        public override string ToString() {
            return string.Format("<VarSymbol(name='{0}', type='{1}')>", Name, Type.Name);
        }
    }

}