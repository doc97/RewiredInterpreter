namespace Rewired.Interpreter {

    /// <summary>
    /// UnaryOp represents the unary operator AST node.
    /// </summary>
    public class UnaryOp : AbstractSyntaxTreeNode {

        /// <summary>
        /// Gets the expression AST node.
        /// </summary>
        /// <value>When visited, the node will return a value to apply the
        /// operator on</value>
        public AbstractSyntaxTreeNode Expr { get; }

        /// <summary>
        /// Gets the operator token.
        /// </summary>
        /// <value>Either a "+", "-" or "!" token.</value>
        public Token Op { get; }

        /// <summary>
        /// Instantiates a new node instance.
        /// </summary>
        /// <param name="op">The operator token</param>
        /// <param name="expr">The expression to apply the operator to</param>
        public UnaryOp(Token op, AbstractSyntaxTreeNode expr) {
            Op = op;
            Expr = expr;
        }

        public override object VisitNode(IAbstractSyntaxTreeNodeVisitor visitor) {
            return visitor.Visit(this);
        }
    }
}