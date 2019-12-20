namespace Rewired.Interpreter {

    /// <summary>
    /// Symbol represents a semantic symbol category, be it a value or a type.
    /// </summary>
    public class Symbol {

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value></value>
        public string Name { get; }

        /// <summary>
        /// Gets the type.
        /// </summary>
        /// <value>Another Symbol representing the type</value>
        public Symbol Type { get; }

        /// <summary>
        /// Gets the name of the symbol.
        /// </summary>
        /// <value>The name or "" if the type is null</value>
        public string TypeName { get => Type?.Name ?? ""; }

        /// <summary>
        /// Instatiates a new instance with the name and type.
        /// </summary>
        /// <param name="name">The name of the symbol</param>
        /// <param name="type">The type of the symbol</param>
        public Symbol(string name, Symbol type = null) {
            Name = name;
            Type = type;
        }
    }

}