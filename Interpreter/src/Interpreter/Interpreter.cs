using System;

namespace InterpreterPractice {

    public class Interpreter : IAbstractSyntaxTreeVisitor {

        private Parser parser;
        public Interpreter(Parser parser) {
            this.parser = parser;
        }

        public int Interpret() {
            AbstractSyntaxTree tree = parser.Parse();
            return tree.Accept(this);
        }

        public int Visit(UnaryOp op) {
            switch (op.Op.Type) {
                case TokenType.Plus:
                    return op.Expr.Accept(this);
                case TokenType.Minus:
                    return -op.Expr.Accept(this);
                default:
                    throw new Exception("Unary operator not implemented: " + op.Op);
            }
        }

        public int Visit(BinOp op) {
            switch (op.Op.Type) {
                case TokenType.Plus:
                    return op.Left.Accept(this) + op.Right.Accept(this);
                case TokenType.Minus:
                    return op.Left.Accept(this) - op.Right.Accept(this);
                case TokenType.Asterisk:
                    return op.Left.Accept(this) * op.Right.Accept(this);
                case TokenType.Slash:
                    return op.Left.Accept(this) / op.Right.Accept(this);
                default:
                    throw new Exception("Binary operator not implemented: " + op.Op);
            }
        }

        public int Visit(Num num) {
            return int.Parse(num.Value);
        }
    }

}