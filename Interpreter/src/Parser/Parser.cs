using System;
using System.Collections.Generic;

namespace Rewired.Interpreter {

    /// <summary>
    /// Parser takes a tokenizer and constructs an intermediate form
    /// called AST (Abstract Syntax Tree) that can be used e.g. by an
    /// interpreter to evaluate statements.
    /// </summary>
    public class Parser {

        /// <summary>
        /// The `Tokenizer` that is copied before parsing.
        /// </summary>
        private Tokenizer origTokenizer;

        /// <summary>
        /// The `Tokenizer` used during parsing, it is stateful.
        /// </summary>
        private Tokenizer tokenizer;

        /// <summary>
        /// Instantiates a new instance.
        /// </summary>
        /// <param name="tokenizer"></param>
        public Parser(Tokenizer tokenizer) {
            origTokenizer = tokenizer;
        }

        /// <summary>
        /// Parse constructs an Abstract Syntax Tree (AST) with the Program as the root node.
        /// </summary>
        /// <returns>The constructed AST</returns>
        /// <exception cref="ParserError">
        /// Throws if there are any errors.
        /// </exception>
        public AbstractSyntaxTreeNode Parse() {
            tokenizer = origTokenizer.Next();
            return Program();
        }

        /// <summary>
        /// Program implements the PROGRAM grammar rule.
        /// 
        /// PROGRAM -> (DECLARATION | STATEMENT_LIST)*
        /// </summary>
        /// <returns></returns>
        private AbstractSyntaxTreeNode Program() {
            List<AbstractSyntaxTreeNode> nodes = new List<AbstractSyntaxTreeNode>();
            while (tokenizer.Token.Type != TokenType.Eof) {
                if (tokenizer.Token.Type == TokenType.Func) {
                    nodes.Add(Declaration());
                } else {
                    nodes.Add(StatementList());
                }
            }
            AbstractSyntaxTreeNode block = new Compound(nodes.ToArray());
            return new Program("", block);
        }

        /// <summary>
        /// Declaration implements the DECLARATION grammar rule.
        /// 
        /// DECLARATION -> "func" ID "(" PARAMS ")" BLOCK
        /// </summary>
        private AbstractSyntaxTreeNode Declaration() {
            tokenizer = Eat(tokenizer, TokenType.Func);
            Token funcName = tokenizer.Token;
            tokenizer = Eat(tokenizer, TokenType.Id);
            tokenizer = Eat(tokenizer, TokenType.LeftParenthesis);
            AbstractSyntaxTreeNode[] parameters = Parameters();
            tokenizer = Eat(tokenizer, TokenType.RightParenthesis);
            return new FunctionDeclaration(funcName, parameters, Block());
        }

        /// <summary>
        /// Parameters implements the PARAMETERS grammar rule.
        /// 
        /// PARAMETERS -> PARAMETER ("," PARAMETERS)* | EMPTY
        /// </summary>
        private AbstractSyntaxTreeNode[] Parameters() {
            List<AbstractSyntaxTreeNode> parameters = new List<AbstractSyntaxTreeNode>();
            if (tokenizer.Token.Type == TokenType.IntegerType || tokenizer.Token.Type == TokenType.FloatType) {
                parameters.Add(Parameter());
            } else {
                return parameters.ToArray();
            }

            while (tokenizer.Token.Type == TokenType.Comma) {
                tokenizer = Eat(tokenizer, TokenType.Comma);
                parameters.Add(Parameter());
            }
            return parameters.ToArray();
        }

        /// <summary>
        /// Parameter implements the PARAMETER grammar rule.
        /// 
        /// PARAMETER -> TYPE ID
        /// </summary>
        private AbstractSyntaxTreeNode Parameter() {
            AbstractSyntaxTreeNode type = Type();
            Token name = tokenizer.Token;
            tokenizer = Eat(tokenizer, TokenType.Id);
            return new Parameter(type, new Var(name));
        }

        /// <summary>
        /// Type implements the TYPE grammar rule.
        /// 
        /// TYPE -> "int" | "float"
        /// </summary>
        private AbstractSyntaxTreeNode Type() {
            Token token = tokenizer.Token;
            tokenizer = Eat(tokenizer, tokenizer.Token.Type);
            return new Type(token);
        }

        /// <summary>
        /// Block implements the BLOCK grammar rule.
        /// 
        /// BLOCK -> "{" COMPOUND "}"
        /// </summary>
        private AbstractSyntaxTreeNode Block() {
            tokenizer = Eat(tokenizer, TokenType.LeftCurlyBracket);
            AbstractSyntaxTreeNode statements = StatementList();
            tokenizer = Eat(tokenizer, TokenType.RightCurlyBracket);
            return statements;
        }

        /// <summary>
        /// StatementList implements the STATEMENT_LIST grammar rule.
        ///
        /// STATEMENT_LIST -> STATEMENT+
        /// </summary>
        private AbstractSyntaxTreeNode StatementList() {
            List<AbstractSyntaxTreeNode> nodes = new List<AbstractSyntaxTreeNode>();
            nodes.Add(Statement());

            while (tokenizer.Token.Type != TokenType.Eof
                && tokenizer.Token.Type != TokenType.RightCurlyBracket
                && tokenizer.Token.Type != TokenType.Func) {
                nodes.Add(Statement());
            }

            return new Compound(nodes.ToArray());
        }

        /// <summary>
        /// Statement implements the STATEMENT grammar rule.
        ///
        /// Rule: STATEMENT -> ASSIGNMENT ";" | EMPTY
        /// </summary>
        private AbstractSyntaxTreeNode Statement() {
            AbstractSyntaxTreeNode node;
            if (tokenizer.Token.Type == TokenType.Id) {
                node = AssignmentStatement();
                tokenizer = Eat(tokenizer, TokenType.SemiColon);
            } else {
                node = EmptyStatement();
            }
            return node;
        }

        /// <summary>
        /// AssignmentStatement implements the ASSIGNMENT grammar rule.
        ///
        /// Rule: ASSIGNMENT -> VAR "=" EXPR
        /// </summary>
        private AbstractSyntaxTreeNode AssignmentStatement() {
            AbstractSyntaxTreeNode left = Variable();
            Token op = tokenizer.Token;
            tokenizer = Eat(tokenizer, TokenType.Assign);
            AbstractSyntaxTreeNode right = Expression();
            return new Assign(left, op, right);
        }

        /// <summary>
        /// Variable implements the VAR grammar rule.
        ///
        /// Rule: VAR -> ID
        /// </summary>
        private AbstractSyntaxTreeNode Variable() {
            Token token = tokenizer.Token;
            tokenizer = Eat(tokenizer, TokenType.Id);
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
            while (tokenizer.Token.Type != TokenType.Eof) {
                Token token = tokenizer.Token;
                if (token.Type == TokenType.Plus) {
                    tokenizer = Eat(tokenizer, TokenType.Plus);
                } else if (token.Type == TokenType.Minus) {
                    tokenizer = Eat(tokenizer, TokenType.Minus);
                } else {
                    break;
                }

                node = new BinaryOp(node, token, Term());
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
            while (tokenizer.Token.Type != TokenType.Eof) {
                Token token = tokenizer.Token;
                if (token.Type == TokenType.Asterisk) {
                    tokenizer = Eat(tokenizer, TokenType.Asterisk);
                } else if (token.Type == TokenType.Slash) {
                    tokenizer = Eat(tokenizer, TokenType.Slash);
                } else {
                    break;
                }

                node = new BinaryOp(node, token, Factor());
            }
            return node;
        }

        /// <summary>
        /// Factor implements the FACTOR grammar rule.
        ///
        /// Rule: FACTOR -> "+" FACTOR
        ///               | "-" FACTOR
        ///               | "(" EXPR ")"
        ///               | INTEGER_CONST
        ///               | VAR
        /// </summary>
        private AbstractSyntaxTreeNode Factor() {
            Token token = tokenizer.Token;
            if (token.Type == TokenType.Plus) {
                tokenizer = Eat(tokenizer, TokenType.Plus);
                return new UnaryOp(token, Factor());
            } else if (token.Type == TokenType.Minus) {
                tokenizer = Eat(tokenizer, TokenType.Minus);
                return new UnaryOp(token, Factor());
            } else if (token.Type == TokenType.LeftParenthesis) {
                tokenizer = Eat(tokenizer, TokenType.LeftParenthesis);
                AbstractSyntaxTreeNode node = Expression();
                tokenizer = Eat(tokenizer, TokenType.RightParenthesis);
                return node;
            } else if (token.Type == TokenType.IntegerConst) {
                tokenizer = Eat(tokenizer, TokenType.IntegerConst);
                return new Int(token);
            } else {
                return Variable();
            }
        }

        /// <summary>
        /// Eat verifies that the next token is of the specified type, and if so consumes it.
        /// </summary>
        /// <param name="tokenizer">Contains the next token</param>
        /// <returns>A new tokenizer without the consumed token</returns>
        private Tokenizer Eat(Tokenizer tokenizer, TokenType type) {
            if (type == tokenizer.Token.Type) {
                return tokenizer.Next();
            } else {
                throw new ParserError(
                    ParserError.ErrorCode.UnexpectedToken,
                    tokenizer.Token,
                    string.Format("Error: Unexpected token '{0}'", tokenizer.Token.Value)
                );
            }
        }
    }

}
