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
        /// PROGRAM -> (FUNCTION_DECLARATION | STATEMENT_LIST)*
        /// </summary>
        /// <returns></returns>
        private AbstractSyntaxTreeNode Program() {
            List<AbstractSyntaxTreeNode> nodes = new List<AbstractSyntaxTreeNode>();
            while (tokenizer.Token.Type != TokenType.Eof) {
                if (tokenizer.Token.Type == TokenType.Func) {
                    nodes.Add(FunctionDeclaration());
                } else {
                    nodes.Add(StatementList());
                }
            }
            AbstractSyntaxTreeNode block = new Compound(nodes.ToArray());
            return new Program("", block);
        }

        /// <summary>
        /// FunctionCall implements the FUNCTION_CALL grammar rule.
        /// 
        /// FUNCTION_CALL -> ID "(" ARGUMENTS ")"
        /// </summary>
        private AbstractSyntaxTreeNode FunctionCall() {
            Token name = tokenizer.Token;
            tokenizer = Eat(tokenizer, TokenType.Id);
            tokenizer = Eat(tokenizer, TokenType.LeftParenthesis);
            AbstractSyntaxTreeNode[] arguments = Arguments();

            tokenizer = Eat(tokenizer, TokenType.RightParenthesis);
            return new FunctionCall(name, arguments);
        }

        /// <summary>
        /// Arguments implements the ARGUMENTS grammar rule.
        /// ARGUMENTS -> (EXPR ("," EXPR)*)?
        /// </summary>
        private AbstractSyntaxTreeNode[] Arguments() {
            if (tokenizer.Token.Type == TokenType.RightParenthesis) {
                return new AbstractSyntaxTreeNode[0];
            }

            List<AbstractSyntaxTreeNode> arguments = new List<AbstractSyntaxTreeNode>();
            arguments.Add(Expression());
            while (tokenizer.Token.Type == TokenType.Comma) {
                tokenizer = Eat(tokenizer, TokenType.Comma);
                arguments.Add(Expression());
            }
            return arguments.ToArray();
        }

        /// <summary>
        /// FunctionDeclaration implements the FUNCTION_DECLARATION grammar rule.
        /// 
        /// FUNCTION_DECLARATION -> "func" ID "(" PARAMETERS ")" BLOCK
        /// </summary>
        private AbstractSyntaxTreeNode FunctionDeclaration() {
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
        /// PARAMETERS -> PARAMETER ("," PARAMETER)* | EMPTY
        /// </summary>
        private AbstractSyntaxTreeNode[] Parameters() {
            if (!IsOneOfTypes(tokenizer.Token,
                              TokenType.IntegerType,
                              TokenType.FloatType,
                              TokenType.BoolType)) {
                return new AbstractSyntaxTreeNode[0];
            }

            List<AbstractSyntaxTreeNode> parameters = new List<AbstractSyntaxTreeNode>();
            parameters.Add(Parameter());
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
        /// TYPE -> "int" | "float" | "bool"
        /// </summary>
        private AbstractSyntaxTreeNode Type() {
            Token token = tokenizer.Token;
            tokenizer = Eat(tokenizer, tokenizer.Token.Type);
            return new Type(token);
        }

        /// <summary>
        /// Block implements the BLOCK grammar rule.
        /// 
        /// BLOCK -> "{" STATEMENT_LIST "}"
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

            bool skip = false;
            while (true) {
                TokenType tokenType = tokenizer.Token.Type;
                if (tokenType == TokenType.Eof
                || tokenType == TokenType.RightCurlyBracket
                || tokenType == TokenType.Func) {
                    return new Compound(nodes.ToArray());
                }

                AbstractSyntaxTreeNode statement = Statement();
                if (skip) { continue; }
                skip = tokenType == TokenType.Return;

                nodes.Add(statement);
            }
        }

        /// <summary>
        /// Statement implements the STATEMENT grammar rule.
        ///
        /// Rule: STATEMENT -> (ASSIGNMENT | IF_STATEMENT | FUNCTION_CALL | RETURN) ;" | EMPTY
        /// </summary>
        private AbstractSyntaxTreeNode Statement() {
            AbstractSyntaxTreeNode node = null;
            TokenType type = tokenizer.Token.Type;
            if (type == TokenType.Id) {
                if (tokenizer.Next().Token.Type == TokenType.LeftParenthesis) {
                    node = FunctionCall();
                } else {
                    node = AssignmentStatement();
                }
                tokenizer = Eat(tokenizer, TokenType.SemiColon);
            } else if (type == TokenType.Return) {
                node = ReturnStatement();
                tokenizer = Eat(tokenizer, TokenType.SemiColon);
            } else if (type == TokenType.If) {
                node = IfStatement();
            } else if (type == TokenType.RightCurlyBracket || type == TokenType.Eof) {
                node = EmptyStatement();
            } else {
                ErrorUnexpectedToken(tokenizer.Token);
            }
            return node;
        }

        /// <summary>
        /// ReturnStatement implements the RETURN grammar rule.
        /// 
        /// Rule: RETURN -> "return" EXPR
        /// </summary>
        private AbstractSyntaxTreeNode ReturnStatement() {
            tokenizer = Eat(tokenizer, TokenType.Return);
            return new Return(Expression());
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
        /// IfStatement implements the IF_STATEMENT grammar rule.
        /// 
        /// Rule: IF_STATEMENT -> "if" BOOL_EXPR BLOCK ("else" BLOCK)?
        /// </summary>
        /// <returns></returns>
        private AbstractSyntaxTreeNode IfStatement() {
            tokenizer = Eat(tokenizer, TokenType.If);
            AbstractSyntaxTreeNode condition = BooleanExpression();
            AbstractSyntaxTreeNode trueBlock = Block();
            AbstractSyntaxTreeNode falseBlock = EmptyStatement();
            if (tokenizer.Token.Type == TokenType.Else) {
                tokenizer = Eat(tokenizer, TokenType.Else);
                falseBlock = Block();
            }
            return new If(condition, trueBlock, falseBlock);
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
        /// Rule: EXPR -> BOOL_EXPR | NUM_EXPR
        /// </summary>
        private AbstractSyntaxTreeNode Expression() {
            Token token = tokenizer.Token;
            Tokenizer origTokenizer = tokenizer;

            try {
                return BooleanExpression();
            } catch (ParserError) {
                // restore tokenizer state after failing to parse boolean expression
                tokenizer = origTokenizer;
                return NumericalExpression();
            }
        }

        /// <summary>
        /// BooleanExpression implements the BOOL_EXPR grammar rule.
        /// 
        /// Rule: BOOL_EXPR -> BOOL_TERM (("&&" | "||") BOOL_TERM)*
        /// </summary>
        private AbstractSyntaxTreeNode BooleanExpression() {
            AbstractSyntaxTreeNode term = BooleanTerm();
            while (tokenizer.Token.Type != TokenType.Eof) {
                Token token = tokenizer.Token;
                if (token.Type == TokenType.LogicalAnd) {
                    tokenizer = Eat(tokenizer, TokenType.LogicalAnd);
                } else if (token.Type == TokenType.LogicalOr) {
                    tokenizer = Eat(tokenizer, TokenType.LogicalOr);
                } else {
                    break;
                }

                term = new BinaryOp(term, token, BooleanTerm());
            }
            return term;
        }

        /// <summary>
        /// BooleanTerm implements the BOOL_TERM grammar rule.
        /// 
        /// Rule: BOOL_TERM -> "(" BOOL_EXPR ")"
        ///                  | "!" BOOL_TERM
        ///                  | BOOL_CONST
        ///                  | NUM_EXPR COND_OP NUM_EXPR
        ///                  | FUNCTION_CALL
        ///                  | VAR
        /// </summary>
        private AbstractSyntaxTreeNode BooleanTerm() {
            Token token = tokenizer.Token;
            if (token.Type == TokenType.LeftParenthesis) {
                tokenizer = Eat(tokenizer, TokenType.LeftParenthesis);
                AbstractSyntaxTreeNode expr = BooleanExpression();
                tokenizer = Eat(tokenizer, TokenType.RightParenthesis);
                return expr;
            } else if (token.Type == TokenType.ExclamationPoint) {
                tokenizer = Eat(tokenizer, TokenType.ExclamationPoint);
                return new UnaryOp(token, BooleanTerm());
            } else if (token.Type == TokenType.BoolConst) {
                tokenizer = Eat(tokenizer, TokenType.BoolConst);
                return new Bool(token);
            } else {
                AbstractSyntaxTreeNode left = NumericalExpression();
                if (IsOneOfTypes(tokenizer.Token,
                                 TokenType.LessThan,
                                 TokenType.GreaterThan,
                                 TokenType.LessThanOrEqual,
                                 TokenType.GreaterThanOrEqual,
                                 TokenType.Equal,
                                 TokenType.NotEqual)) {
                    Token op = tokenizer.Token;
                    tokenizer = Eat(tokenizer, tokenizer.Token.Type);
                    AbstractSyntaxTreeNode right = NumericalExpression();
                    return new BinaryOp(left, op, right);
                }

                if (left is FunctionCall || left is Var) {
                    return left;
                }

                ErrorUnexpectedToken(tokenizer.Token);
                return null;
            }
        }

        /// <summary>
        /// NumericalExpression implements the NUM_EXPR grammar rule.
        ///
        /// Rule: NUM_EXPR -> NUM_TERM (("+" | "-") NUM_TERM)*
        /// </summary>
        private AbstractSyntaxTreeNode NumericalExpression() {
            AbstractSyntaxTreeNode node = NumericalTerm();
            while (tokenizer.Token.Type != TokenType.Eof) {
                Token token = tokenizer.Token;
                if (token.Type == TokenType.Plus) {
                    tokenizer = Eat(tokenizer, TokenType.Plus);
                } else if (token.Type == TokenType.Minus) {
                    tokenizer = Eat(tokenizer, TokenType.Minus);
                } else {
                    break;
                }

                node = new BinaryOp(node, token, NumericalTerm());
            }
            return node;
        }

        /// <summary>
        /// Term implements the NUM_TERM grammar rule.
        ///
        /// Rule: NUM_TERM -> FACTOR (("*" | "/") FACTOR)*
        /// </summary>
        private AbstractSyntaxTreeNode NumericalTerm() {
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
        /// Rule: FACTOR -> "(" NUM_EXPR ")"
        ///               | "+" FACTOR
        ///               | "-" FACTOR
        ///               | FLOAT_CONST
        ///               | INTEGER_CONST
        ///               | FUNCTION_CALL
        ///               | VAR
        /// </summary>
        private AbstractSyntaxTreeNode Factor() {
            Token token = tokenizer.Token;
            if (token.Type == TokenType.LeftParenthesis) {
                tokenizer = Eat(tokenizer, TokenType.LeftParenthesis);
                AbstractSyntaxTreeNode node = NumericalExpression();
                tokenizer = Eat(tokenizer, TokenType.RightParenthesis);
                return node;
            } else if (token.Type == TokenType.Plus) {
                tokenizer = Eat(tokenizer, TokenType.Plus);
                return new UnaryOp(token, Factor());
            } else if (token.Type == TokenType.Minus) {
                tokenizer = Eat(tokenizer, TokenType.Minus);
                return new UnaryOp(token, Factor());
            } else if (token.Type == TokenType.FloatConst) {
                tokenizer = Eat(tokenizer, TokenType.FloatConst);
                return new Float(token);
            } else if (token.Type == TokenType.IntegerConst) {
                tokenizer = Eat(tokenizer, TokenType.IntegerConst);
                return new Int(token);
            } else if (token.Type == TokenType.Id && tokenizer.Next().Token.Type == TokenType.LeftParenthesis) {
                return FunctionCall();
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
                ErrorUnexpectedToken(tokenizer.Token);
                return null;
            }
        }

        private void ErrorUnexpectedToken(Token token) {
            throw new ParserError(
                ParserError.ErrorCode.UnexpectedToken,
                token,
                string.Format("Error: Unexpected token '{0}'", token.Value)
            );
        }

        private bool IsOneOfTypes(Token token, params TokenType[] types) {
            foreach (TokenType t in types) {
                if (token.Type == t) {
                    return true;
                }
            }
            return false;
        }
    }

}
