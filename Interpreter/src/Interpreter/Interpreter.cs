using System;

namespace InterpreterPractice {

    public class Interpreter : IAbstractSyntaxTreeVisitor {

        private Parser parser;
        public Interpreter(Parser parser) {
            this.parser = parser;
        }

        public object Interpret() {
            AbstractSyntaxTree tree = parser.Parse();
            return tree.Accept(this);
        }

        public object Visit(NoOp op) {
            return null;
        }

        public object Visit(UnaryOp op) {
            switch (op.Op.Type) {
                case TokenType.Plus:
                    return +(int)op.Expr.Accept(this);
                case TokenType.Minus:
                    return -(int)op.Expr.Accept(this);
                default:
                    throw new Exception("Unary operator not implemented: " + op.Op);
            }
        }

        public object Visit(BinOp op) {
            switch (op.Op.Type) {
                case TokenType.Plus:
                    return (int)op.Left.Accept(this) + (int)op.Right.Accept(this);
                case TokenType.Minus:
                    return (int)op.Left.Accept(this) - (int)op.Right.Accept(this);
                case TokenType.Asterisk:
                    return (int)op.Left.Accept(this) * (int)op.Right.Accept(this);
                case TokenType.Slash:
                    return (int)op.Left.Accept(this) / (int)op.Right.Accept(this);
                default:
                    throw new Exception("Binary operator not implemented: " + op.Op);
            }
        }

        public object Visit(Num num) {
            return int.Parse(num.Value);
        }
    }

}