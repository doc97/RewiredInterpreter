using System;

namespace InterpreterPractice {

    public class Parser {

        private Lexer lexer;

        public Parser(Lexer lexer) {
            this.lexer = lexer;
        }

        public AbstractSyntaxTree Parse() {
            return Expr();
        }

        private AbstractSyntaxTree Expr() {
            AbstractSyntaxTree node = Term();
            while (lexer != null) {
                Token token = lexer.Token;
                if (token.Type == TokenType.Plus) {
                    lexer = Eat(lexer, TokenType.Plus);
                } else if (token.Type == TokenType.Minus) {
                    lexer = Eat(lexer, TokenType.Minus);
                } else {
                    break;
                }

                node = new BinOp(node, token, Term());
            }
            return node;
        }

        private AbstractSyntaxTree Term() {
            AbstractSyntaxTree node = Factor();
            while (lexer != null) {
                Token token = lexer.Token;
                if (token.Type == TokenType.Asterisk) {
                    lexer = Eat(lexer, TokenType.Asterisk);
                } else if (token.Type == TokenType.Slash) {
                    lexer = Eat(lexer, TokenType.Slash);
                } else {
                    break;
                }

                node = new BinOp(node, token, Factor());
            }
            return node;
        }

        private AbstractSyntaxTree Factor() {
            if (lexer.Token.Type == TokenType.Plus) {
                Token token = lexer.Token;
                lexer = Eat(lexer, TokenType.Plus);
                return new UnaryOp(token, Expr());
            } else if (lexer.Token.Type == TokenType.Minus) {
                Token token = lexer.Token;
                lexer = Eat(lexer, TokenType.Minus);
                return new UnaryOp(token, Expr());
            } else if (lexer.Token.Type == TokenType.Integer) {
                Token token = lexer.Token;
                lexer = Eat(lexer, TokenType.Integer);
                return new Num(token);
            } else if (lexer.Token.Type == TokenType.LeftParenthesis) {
                lexer = Eat(lexer, TokenType.LeftParenthesis);
                AbstractSyntaxTree node = Expr();
                lexer = Eat(lexer, TokenType.RightParenthesis);
                return node;
            } else {
                throw new Exception("Invalid syntax");
            }
        }

        private Lexer Eat(Lexer lexer, TokenType type) {
            if (type == lexer.Token.Type) {
                return lexer.Next();
            } else {
                throw new Exception("Invalid syntax");
            }
        }
    }

}