namespace Rewired.Interpreter {

    /// <summary>
    /// Assign represents the assignment statement AST node.
    /// </summary>
    public class Assign : AbstractSyntaxTreeNode {

        /// <summary>
        /// Gets the left-hand side AST node.
        /// </summary>
        /// <value>Can only be a `Var` AST node.</value>
        public AbstractSyntaxTreeNode Left { get; }

        /// <summary>
        /// Gets the right-hand side AST node.
        /// </summary>
        /// <value>Can be any AST node resulting in a value.</value>
        public AbstractSyntaxTreeNode Right { get; }

        /// <summary>
        /// Gets the token used in the assignment statement.
        /// </summary>
        /// <value>Can only be a ":=" token.</value>
        public Token Op { get; }

        /// <summary>
        /// Instantiates a new node instance comprising of the parameters.
        /// </summary>
        /// <param name="left">The left-hand side</param>
        /// <param name="op">The operation token</param>
        /// <param name="right">The right-hand side</param>
        public Assign(AbstractSyntaxTreeNode left,
                      Token op,
                      AbstractSyntaxTreeNode right) {
            Left = left;
            Op = op;
            Right = right;
        }

        public override object VisitNode(IAbstractSyntaxTreeNodeVisitor visitor) {
            return visitor.Visit(this);
        }
    }

}