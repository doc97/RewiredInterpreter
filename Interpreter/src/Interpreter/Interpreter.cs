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
        /// The call stack containing the stack frames/activation records.
        /// </summary>
        private CallStack stack;

        /// <summary>
        /// Whether to pop the stack after interpreting the AST.
        /// To perform testing or debugging, please set this to false.
        /// </summary>
        public bool ShouldPopStackAtEnd { get; set; } = true;

        /// <summary>
        /// Whether logging is enabled / disabled.
        /// </summary>
        public bool IsLoggingEnabled { get; set; } = false;

        /// <summary>
        /// Instantiates a new instance with an empty global scope.
        /// </summary>
        /// <param name="tree">The AST to interpret</param>
        public Interpreter(AbstractSyntaxTreeNode tree) {
            this.tree = tree;
            stack = new CallStack();
        }

        /// <summary>
        /// Walks the AST.
        /// </summary>
        public void Interpret() {
            tree.VisitNode(this);
        }

        /// <summary>
        /// Looks up the value of a variable stored in the global scope.
        /// 
        /// Should be called after `Interpret()` has been called. 
        /// </summary>
        /// <param name="name">The name of the variable</param>
        /// <returns>The stored value</returns>
        /// <exception cref="KeyNotFoundException">
        /// Throws if a variable with the name cannot be found.
        /// </exception>
        public int GetGlobalVar(string name) {
            int value;
            if (stack.Peek().TryGet(name, out value)) {
                return value;
            }
            throw new KeyNotFoundException();
        }

        public void PrintCallStack() {
            if (IsLoggingEnabled) {
                Console.WriteLine("--== Call Stack ==--");
                Console.WriteLine(stack);
            }
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

        public object Visit(BinaryOp op) {
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
            stack.Peek().Set(varName, varValue);
            return null;
        }

        public object Visit(Var var) {
            // The semantic analyzer checks for unused variables
            return stack.Peek().Get(var.Value);
        }

        public object Visit(Type type) {
            return null;
        }

        public object Visit(Parameter param) {
            return null;
        }

        public object Visit(Compound comp) {
            foreach (AbstractSyntaxTreeNode child in comp.Children) {
                child.VisitNode(this);
            }
            return null;
        }

        public object Visit(FunctionDeclaration func) {
            return null;
        }

        public object Visit(FunctionCall call) {
            return null;
        }

        public object Visit(Program program) {
            ActivationRecord record = new ActivationRecord(
                ActivationRecord.Type.Program,
                program.Name,
                1
            );

            stack.Push(record);
            object ret = program.Block.VisitNode(this);
            if (ShouldPopStackAtEnd) {
                stack.Pop();
            }
            return ret;
        }
        #endregion
    }

}