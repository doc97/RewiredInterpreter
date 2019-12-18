using System;

namespace Rewired.Interpreter {

    public class Lexer {

        public string Text { get; }
        public Token Token { get; }

        public Lexer(string text) : this(text, new Token(TokenType.Eof, "")) {}

        private Lexer(string text, Token token) {
            Text = text;
            Token = token;
        }

        public Lexer Next() {
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

                return new Lexer(text.Substring(token.Value.Length), token);
            }

            return new Lexer("", new Token(TokenType.Eof, ""));
        }

        private char NextChar(string text) {
            if (text.Length == 0) {
                return '\0';
            } else {
                return text[0];
            }
        }

        private string NextString(string text, int length) {
            if (text.Length <= length) {
                return "";
            } else {
                return text.Substring(0, length);
            }
        }

        private string SkipWhitespace(string text) {
            return text.TrimStart();
        }

        private string GetId(string text) {
            return GetMultiCharValue(text, c => char.IsLetterOrDigit(c));
        }
        
        private string GetInteger(string text) {
            return GetMultiCharValue(text, c => "0123456789".Contains(c));
        }

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
            return "Lexer('" + Text + "', " + Token + ")";
        }
    }

}