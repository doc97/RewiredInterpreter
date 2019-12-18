using System;
using System.Collections.Generic;

namespace Rewired.Interpreter {

    public class Parser {

        /// <summary>
        /// origLexer is the <c>Lexer</c> passed to the constructor.
        /// </summary>
        private Lexer origLexer;

        /// <summary>
        /// lexer is the <c>Lexer</c> used during parsing, it is stateful.
        /// </summary>
        private Lexer lexer;

        public Parser(Lexer lexer) {
            origLexer = lexer;
        }

        /// <summary>
        /// Parse constructs an Abstract Syntax Tree (AST) with the compound statement as the root node.
        /// </summary>
        /// <returns>The constructed AST</returns>
        /// <exception>Throws a <c>System.Exception</c> if there are syntax errors.</exception>
        public AbstractSyntaxTree Parse() {
            lexer = origLexer.Next();

            AbstractSyntaxTree node = Compound();
            if (lexer.Token.Type != TokenType.Eof) {
                throw new Exception("Invalid syntax");
            }

            return node;
        }

        /// <summary>
        /// Compound implements the COMPOUND grammar rule.
        ///
        /// COMPOUND -> STATEMENT+
        /// </summary>
        private AbstractSyntaxTree Compound() {
            List<AbstractSyntaxTree> nodes = new List<AbstractSyntaxTree>();
            nodes.Add(Statement());

            while (lexer.Token.Type != TokenType.Eof) {
                nodes.Add(Statement());
            }

            return new Compound(nodes.ToArray());
        }

        /// <summary>
        /// Statement implements the STATEMENT grammar rule.
        ///
        /// Rule: STATEMENT -> (ASSIGNMENT | EMPTY) ";"
        /// </summary>
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

        /// <summary>
        /// AssignmentStatement implements the ASSIGNMENT grammar rule.
        ///
        /// Rule: ASSIGNMENT -> VAR "=" EXPR
        /// </summary>
        private AbstractSyntaxTree AssignmentStatement() {
            AbstractSyntaxTree left = Variable();
            Token op = lexer.Token;
            lexer = Eat(lexer, TokenType.Assign);
            AbstractSyntaxTree right = Expression();
            return new Assign(left, op, right);
        }

        /// <summary>
        /// Variable implements the VAR grammar rule.
        ///
        /// Rule: VAR -> ID
        /// </summary>
        private AbstractSyntaxTree Variable() {
            Token token = lexer.Token;
            lexer = Eat(lexer, TokenType.Id);
            return new Var(token);
        }

        /// <summary>
        /// EmptyStatement implements the EMPTY grammar rule.
        ///
        /// Rule: EMPTY -> ""
        /// </summary>
        private AbstractSyntaxTree EmptyStatement() {
            return new NoOp();
        }

        /// <summary>
        /// Expression implements the EXPR grammar rule.
        ///
        /// Rule: EXPR -> TERM (("+" | "-") TERM)*
        /// </summary>
        private AbstractSyntaxTree Expression() {
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

        /// <summary>
        /// Term implements the TERM grammar rule.
        ///
        /// Rule: TERM -> FACTOR (("*" | "/") FACTOR)*
        /// </summary>
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

        /// <summary>
        /// Factor implements the FACTOR grammar rule.
        ///
        /// Rule: FACTOR -> "+" FACTOR
        ///               | "-" FACTOR
        ///               | "(" EXPR ")"
        ///               | INTEGER
        ///               | VAR
        /// </summary>
        private AbstractSyntaxTree Factor() {
            Token token = lexer.Token;
            if (token.Type == TokenType.Plus) {
                lexer = Eat(lexer, TokenType.Plus);
                return new UnaryOp(token, Factor());
            } else if (token.Type == TokenType.Minus) {
                lexer = Eat(lexer, TokenType.Minus);
                return new UnaryOp(token, Factor());
            } else if (token.Type == TokenType.LeftParenthesis) {
                lexer = Eat(lexer, TokenType.LeftParenthesis);
                AbstractSyntaxTree node = Expression();
                lexer = Eat(lexer, TokenType.RightParenthesis);
                return node;
            } else if (token.Type == TokenType.Integer) {
                lexer = Eat(lexer, TokenType.Integer);
                return new Num(token);
            } else {
                return Variable();
            }
        }

        /// <summary>
        /// Eat verifies that the next token is of the specified type, and if so consumes it.
        /// </summary>
        /// <param name="lexer">Contains the next token</param>
        /// <returns>A new lexer without the consumed token</returns>
        private Lexer Eat(Lexer lexer, TokenType type) {
            if (type == lexer.Token.Type) {
                return lexer.Next();
            } else {
                throw new Exception("Invalid syntax");
            }
        }
    }

}