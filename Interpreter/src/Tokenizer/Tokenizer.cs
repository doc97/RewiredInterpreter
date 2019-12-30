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
        private Dictionary<string, ReservedKeyword> reservedKeywords;

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
        /// Gets the line number of the next token..
        /// </summary>
        public int Line { get; }

        /// <summary>
        /// Gets the column number of the next token.
        /// </summary>
        public int Column { get; }

        /// <summary>
        /// Instantiates a new instance out of the source text.
        /// 
        /// The current token is of the type Eof.
        /// </summary>
        /// <param name="text">The text to convert</param>
        public Tokenizer(string text) : this(text, new Token(TokenType.Eof, "", 0, 0), 1, 1) { }

        /// <summary>
        /// Instantiates a new instance out of the source text and token.
        /// </summary>
        /// <param name="text">The text to convert</param>
        /// <param name="token">The current converted token</param>
        /// <param name="line">The current line number</param>
        /// <param name="column">The current column number</param>
        private Tokenizer(string text, Token token, int line, int column) {
            Text = text;
            Token = token;
            Line = line;
            Column = column;
            reservedKeywords = new Dictionary<string, ReservedKeyword>() {
                { "func", new ReservedKeyword(TokenType.Func, "func") },
                { "int", new ReservedKeyword(TokenType.IntegerType, "int") },
                { "float", new ReservedKeyword(TokenType.FloatType, "float") },
            };
        }

        /// <summary>
        /// Extracts the next token from the text.
        /// </summary>
        /// <returns>
        /// A new `Tokenizer` with the next token and the remaining text.
        /// Returns and empty tokenizer the text is empty.
        /// </returns>
        /// <exception cref="TokenizerError">
        /// Throws if no token could be recognized.
        /// </exception>
        public Tokenizer Next() {
            string text = Text;
            int line = Line;
            int column = Column;
            char currentChar = NextChar(text);

            while (currentChar != '\0') {
                Token token = new Token(TokenType.Eof, null, line, column);

                if (char.IsWhiteSpace(currentChar)) {
                    text = SkipWhitespace(text, ref line, ref column);
                    currentChar = NextChar(text);
                    continue;
                } else if (char.IsLetter(currentChar)) {
                    token = GetId(text, line, column);
                } else if ("0123456789".Contains(currentChar)) {
                    token = new Token(TokenType.IntegerConst, GetInteger(text), line, column);
                } else if (currentChar == '+') {
                    token = new Token(TokenType.Plus, "+", line, column);
                } else if (currentChar == '-') {
                    token = new Token(TokenType.Minus, "-", line, column);
                } else if (currentChar == '*') {
                    token = new Token(TokenType.Asterisk, "*", line, column);
                } else if (currentChar == '/') {
                    token = new Token(TokenType.Slash, "/", line, column);
                } else if (currentChar == '(') {
                    token = new Token(TokenType.LeftParenthesis, "(", line, column);
                } else if (currentChar == ')') {
                    token = new Token(TokenType.RightParenthesis, ")", line, column);
                } else if (currentChar == '{') {
                    token = new Token(TokenType.LeftCurlyBracket, "{", line, column);
                } else if (currentChar == '}') {
                    token = new Token(TokenType.RightCurlyBracket, "}", line, column);
                } else if (currentChar == ';') {
                    token = new Token(TokenType.SemiColon, ";", line, column);
                } else if (currentChar == ',') {
                    token = new Token(TokenType.Comma, ",", line, column);
                } else if (NextString(text, 2) == ":=") {
                    token = new Token(TokenType.Assign, ":=", line, column);
                }

                if (token.Type == TokenType.Eof) {
                    throw new TokenizerError(currentChar, line, column, "Invalid syntax");
                }

                return new Tokenizer(text.Substring(token.Value.Length), token, line, column + token.Value.Length);
            }

            return new Tokenizer("", new Token(TokenType.Eof, "", line, column), line, column);
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
        /// <param name="line">The current line number (may changes)</param>
        /// <param name="text">The current column number (may change)</param>
        /// <returns>A new string with the whitespace removed.</returns>
        private string SkipWhitespace(string text, ref int line, ref int column) {
            for (int i = 0; i < text.Length; i++) {
                if (text[i] == '\n') {
                    line++;
                    column = 1;
                } else if (char.IsWhiteSpace(text[i])) {
                    column++;
                } else {
                    return text.Substring(i);
                }
            }
            return "";
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
        private Token GetId(string text, int line, int column) {
            string word = GetMultiCharValue(text, c => char.IsLetter(c));
            if (reservedKeywords.ContainsKey(word)) {
                ReservedKeyword keyword = reservedKeywords[word];
                return new Token(keyword.type, keyword.name, line, column);
            }

            string id = GetMultiCharValue(text, c => char.IsLetterOrDigit(c));
            return new Token(TokenType.Id, id, line, column);
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