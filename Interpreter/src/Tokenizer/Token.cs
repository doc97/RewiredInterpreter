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
        /// Gets the line number on which the token is.
        /// </summary>
        public int Line { get; }

        /// <summary>
        /// Gets the column number on which the token starts.
        /// </summary>
        public int Column { get; }

        /// <summary>
        /// Instantiates a new instance.
        /// </summary>
        /// <param name="type">The token type</param>
        /// <param name="value">The token value</param>
        /// <param name="line">The line number</param>
        /// <param name="column">The column number</param>
        public Token(TokenType type, string value, int line, int column) {
            Type = type;
            Value = value;
            Line = line;
            Column = column;
        }

        public override string ToString() {
            return string.Format("Token({0}, {1}, {2}:{3})", Type, Value, Line, Column);
        }
    }

}