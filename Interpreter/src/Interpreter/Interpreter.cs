using System;
using System.Collections.Generic;

namespace Rewired.Interpreter {

    public class Interpreter : IAbstractSyntaxTreeNodeVisitor {

        private AbstractSyntaxTree tree;
        private Dictionary<string, int> globalScope;

        public Interpreter(AbstractSyntaxTree tree) {
            this.tree = tree;
            globalScope = new Dictionary<string, int>();
        }

        public void Interpret() {
            tree.Accept(this);
        }

        public void PrintGlobalScope() {
            Console.WriteLine("{");
            foreach (KeyValuePair<string, int> pair in globalScope) {
                Console.WriteLine("  " + pair.Key + ": " + pair.Value);
            }
            Console.WriteLine("}");
        }

        public int GetGlobalVar(string name) {
            return globalScope[name];
        }

        #region IAbstractSyntaxTreeNodeVisitor
        public object Visit(NoOp op) {
            return null;
        }

        public object Visit(UnaryOp op) {
            switch (op.Op.Type) {
                case TokenType.Plus:
                    return +(int) op.Expr.Accept(this);
                case TokenType.Minus:
                    return -(int) op.Expr.Accept(this);
                default:
                    throw new Exception("Unary operator not implemented: " + op.Op);
            }
        }

        public object Visit(BinOp op) {
            switch (op.Op.Type) {
                case TokenType.Plus:
                    return (int) op.Left.Accept(this) + (int) op.Right.Accept(this);
                case TokenType.Minus:
                    return (int) op.Left.Accept(this) - (int) op.Right.Accept(this);
                case TokenType.Asterisk:
                    return (int) op.Left.Accept(this) * (int) op.Right.Accept(this);
                case TokenType.Slash:
                    return (int) op.Left.Accept(this) / (int) op.Right.Accept(this);
                default:
                    throw new Exception("Binary operator not implemented: " + op.Op);
            }
        }

        public object Visit(Num num) {
            return int.Parse(num.Value);
        }

        public object Visit(Assign assign) {
            // The left-hand side of an Assign statement is Var
            string varName = ((Var) assign.Left).Value;
            int varValue = (int) assign.Right.Accept(this);
            globalScope[varName] = varValue;
            return null;
        }

        public object Visit(Var var) {
            if (globalScope.ContainsKey(var.Value)) {
                return globalScope[var.Value];
            }
            throw new Exception("Variable '" + var.Value + "' does not exist!");
        }

        public object Visit(Compound comp) {
            foreach (AbstractSyntaxTree child in comp.Children) {
                child.Accept(this);
            }
            return null;
        }
        #endregion
    }

}