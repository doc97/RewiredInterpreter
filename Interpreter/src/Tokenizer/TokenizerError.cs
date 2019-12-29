using System;

namespace Rewired.Interpreter {

    public class TokenizerError : Exception {

        /// <summary>
        /// Gets the line number on which the error occurred.
        /// </summary>
        public int Line { get; }

        /// <summary>
        /// Gets the column number on which the error occurred.
        /// </summary>
        public int Column { get; }

        /// <summary>
        /// Gets the character on which the error occurred.
        /// </summary>
        public char Lexeme { get; }

        public TokenizerError(char lexeme, int line, int column, string message = "")
            : base(string.Format("Lexer error on '{0}' (line: {1}, col: {2}): {3}",
                                lexeme, line, column, message)) {
            Line = line;
            Column = column;
            Lexeme = lexeme;
        }
    }

}