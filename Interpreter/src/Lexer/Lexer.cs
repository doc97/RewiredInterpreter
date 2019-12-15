using System;

namespace InterpreterPractice {

    public class Lexer {

        public string Text { get; }
        public Token Token { get; }

        private Lexer(string text, Token token) {
            Text = text;
            Token = token;
        }

        public Lexer(string text) {
            Text = text;
            Token = new Token(TokenType.Eof, null);
        }

        public static Lexer New(string text) {
            return new Lexer(text, null).Next();
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
                }

                if (token.Type == TokenType.Eof) {
                    throw new Exception("Invalid syntax");
                }

                return new Lexer(text.Substring(token.Value.Length), token);
            }

            return null;
        }
        
        private char NextChar(string text) {
            if (text.Length == 0) {
                return '\0';
            } else {
                return text[0];
            }
        }

        private string SkipWhitespace(string text) {
            return text.TrimStart();
        }

        private string GetInteger(string text) {
            string result = "";
            int pos = 0;
            while (pos < text.Length && "0123456789".Contains(text[pos])) {
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