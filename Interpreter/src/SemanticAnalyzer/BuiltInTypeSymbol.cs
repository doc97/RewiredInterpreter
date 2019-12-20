namespace Rewired.Interpreter {

    /// <summary>
    /// BuiltInTypeSymbol represents the built-in symbol category.
    /// </summary>
    public class BuiltInTypeSymbol : Symbol {

        /// <summary>
        /// Instantiates a new instance with a name.
        /// 
        /// The (underlying) type is null for built-ins.
        /// </summary>
        /// <param name="name">The name of the symbol</param>
        /// <returns></returns>
        public BuiltInTypeSymbol(string name) : base(name) { }

        public override string ToString() {
            return string.Format("<BuiltInTypeSymbol(name='{0}')>", Name);
        }
    }

}