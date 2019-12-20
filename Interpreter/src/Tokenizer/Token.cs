namespace Rewired.Interpreter {

    /// <summary>
    /// Token represents a token that has a type and a string value.
    /// </summary>
    public class Token {
        
        /// <summary>
        /// Gets the token type.
        /// </summary>
        /// <value></value>
        public TokenType Type { get; }

        /// <summary>
        /// Gets the token value.
        /// </summary>
        /// <value></value>
        public string Value { get; }

        /// <summary>
        /// Instantiates a new instance.
        /// </summary>
        /// <param name="type">The token type</param>
        /// <param name="value">The token value</param>
        public Token(TokenType type, string value) {
            Type = type;
            Value = value;
        }

        public override string ToString() {
            return "Token(" + Type + ", " + Value + ")";
        }
    }

}