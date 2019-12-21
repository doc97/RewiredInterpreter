using System;
using System.Collections.Generic;

namespace Rewired.Interpreter {

    /// <summary>
    /// Interpreter implements the AST node visitor and will walk the tree and
    /// evaluate the node values.
    /// </summary>
    public class Interpreter : IAbstractSyntaxTreeNodeVisitor {

        /// <summary>
        /// The AST to interpret.
        /// </summary>
        private AbstractSyntaxTreeNode tree;

        /// <summary>
        /// The dictionary works as the global memory of the program during runtime.
        /// </summary>
        private Dictionary<string, int> globalScope;

        /// <summary>
        /// Instantiates a new instance with an empty global scope.
        /// </summary>
        /// <param name="tree">The AST to interpret</param>
        public Interpreter(AbstractSyntaxTreeNode tree) {
            this.tree = tree;
            globalScope = new Dictionary<string, int>();
        }

        /// <summary>
        /// Walks the AST.
        /// </summary>
        public void Interpret() {
            tree.VisitNode(this);
        }

        /// <summary>
        /// Prints the current state of the global scope.
        ///
        /// This is mostly for debugging purposes.
        /// </summary>
        public void PrintGlobalScope() {
            Console.WriteLine("{");
            foreach (KeyValuePair<string, int> pair in globalScope) {
                Console.WriteLine("  " + pair.Key + ": " + pair.Value);
            }
            Console.WriteLine("}");
        }

        /// <summary>
        /// Looks up the value of a variable stored in the global scope.
        /// 
        /// Should be called after `Interpret()` has been called. The function
        /// will throw a `System.KeyNotFoundException` exception if a variable
        /// with the name cannot be found.
        /// </summary>
        /// <param name="name">The name of the variable</param>
        /// <returns>The stored value</returns>
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
                    return +(int) op.Expr.VisitNode(this);
                case TokenType.Minus:
                    return -(int) op.Expr.VisitNode(this);
                default:
                    throw new NotImplementedException("Unary operator not implemented: " + op.Op);
            }
        }

        public object Visit(BinOp op) {
            switch (op.Op.Type) {
                case TokenType.Plus:
                    return (int) op.Left.VisitNode(this) + (int) op.Right.VisitNode(this);
                case TokenType.Minus:
                    return (int) op.Left.VisitNode(this) - (int) op.Right.VisitNode(this);
                case TokenType.Asterisk:
                    return (int) op.Left.VisitNode(this) * (int) op.Right.VisitNode(this);
                case TokenType.Slash:
                    return (int) op.Left.VisitNode(this) / (int) op.Right.VisitNode(this);
                default:
                    throw new NotImplementedException("Binary operator not implemented: " + op.Op);
            }
        }

        public object Visit(Int num) {
            return int.Parse(num.Value);
        }

        public object Visit(Assign assign) {
            // The left-hand side of an Assign statement is Var
            string varName = ((Var) assign.Left).Value;
            int varValue = (int) assign.Right.VisitNode(this);
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
            foreach (AbstractSyntaxTreeNode child in comp.Children) {
                child.VisitNode(this);
            }
            return null;
        }

        public object Visit(FuncDecl func) {
            return null;
        }
        #endregion
    }

}