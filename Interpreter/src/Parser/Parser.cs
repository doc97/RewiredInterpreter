using System;
using System.Collections.Generic;

namespace Rewired.Interpreter {

    /// <summary>
    /// Parser takes a lexer (tokenizer) and constructs an intermediate form
    /// called AST (Abstract Syntax Tree) that can be used e.g. by an
    /// interpreter to evaluate statements.
    /// </summary>
    public class Parser {

        /// <summary>
        /// The `Lexer` that is copied before parsing.
        /// </summary>
        private Lexer origLexer;

        /// <summary>
        /// The `Lexer` used during parsing, it is stateful.
        /// </summary>
        private Lexer lexer;

        /// <summary>
        /// Instantiates a new instance.
        /// </summary>
        /// <param name="lexer"></param>
        public Parser(Lexer lexer) {
            origLexer = lexer;
        }

        /// <summary>
        /// Parse constructs an Abstract Syntax Tree (AST) with the compound statement as the root node.
        /// </summary>
        /// <returns>The constructed AST</returns>
        /// <exception>Throws a <c>System.Exception</c> if there are syntax errors.</exception>
        public AbstractSyntaxTreeNode Parse() {
            lexer = origLexer.Next();

            AbstractSyntaxTreeNode node = Compound();
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
        private AbstractSyntaxTreeNode Compound() {
            List<AbstractSyntaxTreeNode> nodes = new List<AbstractSyntaxTreeNode>();
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
        private AbstractSyntaxTreeNode Statement() {
            AbstractSyntaxTreeNode node;
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
        private AbstractSyntaxTreeNode AssignmentStatement() {
            AbstractSyntaxTreeNode left = Variable();
            Token op = lexer.Token;
            lexer = Eat(lexer, TokenType.Assign);
            AbstractSyntaxTreeNode right = Expression();
            return new Assign(left, op, right);
        }

        /// <summary>
        /// Variable implements the VAR grammar rule.
        ///
        /// Rule: VAR -> ID
        /// </summary>
        private AbstractSyntaxTreeNode Variable() {
            Token token = lexer.Token;
            lexer = Eat(lexer, TokenType.Id);
            return new Var(token);
        }

        /// <summary>
        /// EmptyStatement implements the EMPTY grammar rule.
        ///
        /// Rule: EMPTY -> ""
        /// </summary>
        private AbstractSyntaxTreeNode EmptyStatement() {
            return new NoOp();
        }

        /// <summary>
        /// Expression implements the EXPR grammar rule.
        ///
        /// Rule: EXPR -> TERM (("+" | "-") TERM)*
        /// </summary>
        private AbstractSyntaxTreeNode Expression() {
            AbstractSyntaxTreeNode node = Term();
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
        private AbstractSyntaxTreeNode Term() {
            AbstractSyntaxTreeNode node = Factor();
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
        private AbstractSyntaxTreeNode Factor() {
            Token token = lexer.Token;
            if (token.Type == TokenType.Plus) {
                lexer = Eat(lexer, TokenType.Plus);
                return new UnaryOp(token, Factor());
            } else if (token.Type == TokenType.Minus) {
                lexer = Eat(lexer, TokenType.Minus);
                return new UnaryOp(token, Factor());
            } else if (token.Type == TokenType.LeftParenthesis) {
                lexer = Eat(lexer, TokenType.LeftParenthesis);
                AbstractSyntaxTreeNode node = Expression();
                lexer = Eat(lexer, TokenType.RightParenthesis);
                return node;
            } else if (token.Type == TokenType.Integer) {
                lexer = Eat(lexer, TokenType.Integer);
                return new Int(token);
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