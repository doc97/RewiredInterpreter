using System;
using System.Collections.Generic;

namespace Rewired.Interpreter {

    /// <summary>
    /// Tokenizer takes a string of text and converts it into a stream of tokens.
    /// </summary>
    public class Tokenizer {

        /// <summary>
        /// Contains the reserved keywords of the language.
        /// </summary>
        private Dictionary<string, Token> reservedKeywords;

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
            reservedKeywords = new Dictionary<string, Token>() {
                { "func", new Token(TokenType.Func, "func") }
            };
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
                    token = GetId(text);
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
                } else if (currentChar == '{') {
                    token = new Token(TokenType.LeftCurlyBracket, "{");
                } else if (currentChar == '}') {
                    token = new Token(TokenType.RightCurlyBracket, "}");
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
        /// The string of peeked characters or "" if the text is shorter than length.
        /// </returns>
        private string NextString(string text, int length) {
            return text.Length < length ? "" : text.Substring(0, length);
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
        /// Checks if the next word in the text is one of the reserved ones.
        /// </summary>
        /// <param name="text">The text to check</param>
        /// <returns>True if is a reserved keyword, false otherwise.</returns>
        private bool IsReservedKeyword(string text) {
            string word = GetMultiCharValue(text, c => char.IsLetter(c));
            return reservedKeywords.ContainsKey(word);
        }

        /// <summary>
        /// Gets the next token which consists of alphanumeric characters.
        /// </summary>
        /// <param name="text">The text to traverse</param>
        /// <returns>The token, either an Id or a reserved keyword token.</returns>
        private Token GetId(string text) {
            string word = GetMultiCharValue(text, c => char.IsLetter(c));
            if (reservedKeywords.ContainsKey(word)) {
                return reservedKeywords[word];
            }

            string id = GetMultiCharValue(text, c => char.IsLetterOrDigit(c));
            return new Token(TokenType.Id, id);
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