using System;

namespace Rewired.Interpreter {

    /// <summary>
    /// Tokenizer takes a string of text and converts it into a stream of tokens.
    /// </summary>
    public class Tokenizer {

        /// <summary>
        /// Gets the source text. 
        /// </summary>
        /// <value></value>
        public string Text { get; }

        /// <summary>
        /// Gets the current converted token.
        /// </summary>
        /// <value></value>
        public Token Token { get; }

        /// <summary>
        /// Instantiates a new instance out of the source text.
        /// 
        /// The current token is of the type Eof.
        /// </summary>
        /// <param name="text">The text to convert</param>
        public Tokenizer(string text) : this(text, new Token(TokenType.Eof, "")) {}

        /// <summary>
        /// Instantiates a new instance out of the source text and token.
        /// </summary>
        /// <param name="text">The text to convert</param>
        /// <param name="token">The current converted token</param>
        private Tokenizer(string text, Token token) {
            Text = text;
            Token = token;
        }

        /// <summary>
        /// Extracts the next token from the text.
        /// </summary>
        /// <returns>
        /// A new `Tokenizer` with the next token and the remaining text.
        /// Returns and empty tokenizer the text is empty.
        /// </returns>
        /// <exception>
        /// Throws a <see href="System.Exception" /> if no token could be
        /// recognized.
        /// </exception>
        public Tokenizer Next() {
            string text = Text;
            char currentChar = NextChar(text);

            while (currentChar != '\0') {
                Token token = new Token(TokenType.Eof, null);

                if (char.IsWhiteSpace(currentChar)) {
                    text = text.TrimStart();
                    currentChar = NextChar(text);
                    continue;
                } else if (char.IsLetter(currentChar)) {
                    token = new Token(TokenType.Id, GetId(text));
                } else if ("0123456789".Contains(currentChar)) {
                    token = new Token(TokenType.Integer, GetInteger(text));
                } else if (currentChar == '+') {
                    token = new Token(TokenType.Plus, "+");
                } else if (currentChar == '-') {
                    token = new Token(TokenType.Minus, "-");
                } else if (currentChar == '*') {
                    token = new Token(TokenType.Asterisk, "*");
                } else if (currentChar == '/') {
                    token = new Token(TokenType.Slash, "/");
                } else if (currentChar == '(') {
                    token = new Token(TokenType.LeftParenthesis, "(");
                } else if (currentChar == ')') {
                    token = new Token(TokenType.RightParenthesis, ")");
                } else if (currentChar == ';') {
                    token = new Token(TokenType.SemiColon, ";");
                } else if (NextString(text, 2) == ":=") {
                    token = new Token(TokenType.Assign, ":=");
                }

                if (token.Type == TokenType.Eof) {
                    throw new Exception("Invalid syntax");
                }

                return new Tokenizer(text.Substring(token.Value.Length), token);
            }

            return new Tokenizer("", new Token(TokenType.Eof, ""));
        }

        /// <summary>
        /// Peeks at the next character in the text.
        /// </summary>
        /// <param name="text">The text to peek</param>
        /// <returns>The next character or '\0' if the text is empty.</returns>
        private char NextChar(string text) {
            if (text.Length == 0) {
                return '\0';
            } else {
                return text[0];
            }
        }

        /// <summary>
        /// Peeks n characters forward in the text.
        /// </summary>
        /// <param name="text">The text to peek</param>
        /// <param name="length">The amount of characters to peek</param>
        /// <returns>
        /// The string of peeked characters or "" if the text is empty.
        /// </returns>
        private string NextString(string text, int length) {
            if (text.Length <= length) {
                return "";
            } else {
                return text.Substring(0, length);
            }
        }

        /// <summary>
        /// Trims the start of text of whitespace.
        /// </summary>
        /// <param name="text">The text to trim (not changed)</param>
        /// <returns>A new string with the whitespace removed.</returns>
        private string SkipWhitespace(string text) {
            return text.TrimStart();
        }

        /// <summary>
        /// Gets the next string where the characters are letters or digits.
        /// </summary>
        /// <param name="text">The text to traverse</param>
        /// <returns>The characters as a string.</returns>
        private string GetId(string text) {
            return GetMultiCharValue(text, c => char.IsLetterOrDigit(c));
        }
        
        /// <summary>
        /// Gets the next string where the characters are digits.
        /// </summary>
        /// <param name="text">The text to traverse</param>
        /// <returns>The integer as a string.</returns>
        private string GetInteger(string text) {
            return GetMultiCharValue(text, c => "0123456789".Contains(c));
        }

        /// <summary>
        /// Gets the next string where the characters match a predicate.
        /// </summary>
        /// <param name="text">The text to traverse</param>
        /// <param name="accept">The predicate of each character</param>
        /// <returns>The characters as a string</returns>
        private string GetMultiCharValue(string text, Predicate<char> accept) {
            string result = "";
            int pos = 0;
            while (pos < text.Length && accept.Invoke(text[pos])) {
                result += text[pos];
                pos++;
            }
            return result;
        }

        public override string ToString() {
            return "Tokenizer('" + Text + "', " + Token + ")";
        }
    }

}