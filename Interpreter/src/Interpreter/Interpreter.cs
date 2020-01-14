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
        /// Functions are stored in a separate dictionary and is not a part
        /// of the call stack.
        /// </summary>
        private Dictionary<string, FunctionDeclaration> functions;

        /// <summary>
        /// Instantiates a new instance with an empty global scope.
        /// </summary>
        /// <param name="tree">The AST to interpret</param>
        public Interpreter(AbstractSyntaxTreeNode tree) {
            this.tree = tree;
            stack = new CallStack();
            functions = new Dictionary<string, FunctionDeclaration>();
        }

        /// <summary>
        /// Walks the AST.
        /// </summary>
        public object Interpret() {
            return tree.VisitNode(this);
        }

        #region IAbstractSyntaxTreeNodeVisitor
        public object Visit(NoOp op) {
            return null;
        }

        public object Visit(UnaryOp op) {
            object expr = op.Expr.VisitNode(this);
            switch (op.Op.Type) {
                case TokenType.Plus:
                    if (expr is int) {
                        return +(int) expr;
                    } else if (expr is float) {
                        return +(float) expr;
                    }
                    break;
                case TokenType.Minus:
                    if (expr is int) {
                        return -(int) expr;
                    } else if (expr is float) {
                        return -(float) expr;
                    }
                    break;
            }
            throw new NotImplementedException(string.Format(
                "Unary operator '{0}' not implemented for the type '{1}'",
                op.Op, expr.GetType()
            ));
        }

        public object Visit(BinaryOp op) {
            object left = op.Left.VisitNode(this);
            object right = op.Right.VisitNode(this);

            switch (op.Op.Type) {
                case TokenType.Plus:
                    if (left is int) {
                        return (int) left + (int) right;
                    } else if (left is float) {
                        return (float) left + (float) right;
                    }
                    break;
                case TokenType.Minus:
                    if (left is int) {
                        return (int) left - (int) right;
                    } else if (left is float) {
                        return (float) left - (float) right;
                    }
                    break;
                case TokenType.Asterisk:
                    if (left is int) {
                        return (int) left * (int) right;
                    } else if (left is float) {
                        return (float) left * (float) right;
                    }
                    break;
                case TokenType.Slash:
                    if (left is int) {
                        return (int) left / (int) right;
                    } else if (left is float) {
                        return (float) left / (float) right;
                    }
                    break;
            }
            throw new NotImplementedException(string.Format(
                "Binary operator '{0}' not implemented for the type '{1}'",
                op.Op, left.GetType()
            ));
        }

        public object Visit(Bool boolean) {
            return bool.Parse(boolean.Value);
        }

        public object Visit(Float num) {
            return float.Parse(num.Value);
        }

        public object Visit(Int num) {
            return int.Parse(num.Value);
        }

        public object Visit(Assign assign) {
            // The left-hand side of an Assign statement is Var
            string varName = ((Var) assign.Left).Value;
            object varValue = assign.Right.VisitNode(this);
            stack.Peek().Set(varName, varValue);
            return null;
        }

        public object Visit(Return ret) {
            return ret.Expr.VisitNode(this);
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
            // returns the value returned from visiting the last element in
            // the compound statement.
            object ret = null;
            foreach (AbstractSyntaxTreeNode child in comp.Children) {
                ret = child.VisitNode(this);
            }
            return ret;
        }

        public object Visit(FunctionDeclaration func) {
            functions.Add(func.Name, func);
            return null;
        }

        public object Visit(FunctionCall call) {
            ActivationRecord record = new ActivationRecord(
                ActivationRecord.Type.Function,
                call.Name,
                stack.Count + 1
            );

            // Add arguments to the activation record
            FunctionDeclaration func = functions[call.Name];
            for (int i = 0; i < call.Arguments.Length; i++) {
                AbstractSyntaxTreeNode arg = call.Arguments[i];
                Parameter param = (Parameter) func.Parameters[i];
                string name = ((Var) param.Name).Value;
                int value = (int) arg.VisitNode(this);
                record.Set(name, value);
            }

            stack.Push(record);
            object ret = functions[call.Name].Block.VisitNode(this);
            stack.Pop();
            return ret;
        }

        public object Visit(Program program) {
            ActivationRecord record = new ActivationRecord(
                ActivationRecord.Type.Program,
                program.Name,
                1
            );

            stack.Push(record);
            object ret = program.Block.VisitNode(this);
            stack.Pop();

            return ret;
        }
        #endregion
    }

}