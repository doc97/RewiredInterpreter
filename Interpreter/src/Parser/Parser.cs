using System;
using System.Collections.Generic;

namespace Rewired.Interpreter {

    public class Parser {

        private Lexer lexer;

        public Parser(Lexer lexer) {
            this.lexer = lexer;
        }

        public AbstractSyntaxTree Parse() {
            AbstractSyntaxTree node = Compound();
            if (lexer.Token.Type != TokenType.Eof) {
                throw new Exception("Invalid syntax");
            }

            return node;
        }

        private AbstractSyntaxTree Compound() {
            List<AbstractSyntaxTree> nodes = new List<AbstractSyntaxTree>();
            nodes.Add(Statement());

            while (lexer.Token.Type != TokenType.Eof) {
                nodes.Add(Statement());
            }

            return new Compound(nodes.ToArray());
        }

        private AbstractSyntaxTree Statement() {
            AbstractSyntaxTree node;
            if (lexer.Token.Type == TokenType.Id) {
                node = AssignmentStatement();
            } else {
                node = EmptyStatement();
            }
            lexer = Eat(lexer, TokenType.SemiColon);
            return node;
        }

        private AbstractSyntaxTree AssignmentStatement() {
            AbstractSyntaxTree left = Variable();
            Token op = lexer.Token;
            lexer = Eat(lexer, TokenType.Assign);
            AbstractSyntaxTree right = Expr();
            return new Assign(left, op, right);
        }

        private AbstractSyntaxTree Variable() {
            Token token = lexer.Token;
            lexer = Eat(lexer, TokenType.Id);
            return new Var(token);
        }

        private AbstractSyntaxTree EmptyStatement() {
            return new NoOp();
        }

        private AbstractSyntaxTree Expr() {
            AbstractSyntaxTree node = Term();
            while (lexer.Token.Type != TokenType.Eof) {
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
            while (lexer.Token.Type != TokenType.Eof) {
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
            Token token = lexer.Token;
            if (token.Type == TokenType.Plus) {
                lexer = Eat(lexer, TokenType.Plus);
                return new UnaryOp(token, Expr());
            } else if (token.Type == TokenType.Minus) {
                lexer = Eat(lexer, TokenType.Minus);
                return new UnaryOp(token, Expr());
            } else if (token.Type == TokenType.Integer) {
                lexer = Eat(lexer, TokenType.Integer);
                return new Num(token);
            } else if (token.Type == TokenType.LeftParenthesis) {
                lexer = Eat(lexer, TokenType.LeftParenthesis);
                AbstractSyntaxTree node = Expr();
                lexer = Eat(lexer, TokenType.RightParenthesis);
                return node;
            } else {
                return Variable();
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