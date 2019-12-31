namespace Rewired.Interpreter {

    /// <summary>
    /// Return represents a return statement.
    /// </summary>
    public class Return : AbstractSyntaxTreeNode {

        /// <summary>
        /// Gets the return expression.
        /// </summary>
        public AbstractSyntaxTreeNode Expr { get; }

        /// <summary>
        /// Instantiates a new node with the specified expression.
        /// </summary>
        /// <param name="expr">The expression evaluating the return value</param>
        public Return(AbstractSyntaxTreeNode expr) {
            Expr = expr;
        }

        public override object VisitNode(IAbstractSyntaxTreeNodeVisitor visitor) {
            return visitor.Visit(this);
        }
    }
}