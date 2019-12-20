namespace Rewired.Interpreter {

    /// <summary>
    /// BinOp represents the binary operator AST node.
    /// </summary>
    public class BinOp : AbstractSyntaxTreeNode {

        /// <summary>
        /// Gets the left-hand operand.
        /// </summary>
        /// <value>Can only be a `Num` AST node.</value>
        public AbstractSyntaxTreeNode Left { get; }

        /// <summary>
        /// Gets the right-hand operand.
        /// </summary>
        /// <value>Can only be a `Num` AST node.</value>
        public AbstractSyntaxTreeNode Right { get; }

        /// <summary>
        /// Gets the binary operator token.
        /// </summary>
        /// <value>Is a binary operator token e.g. "+" or "-".</value>
        public Token Op { get; }

        /// <summary>
        /// Instantiates a new node instance comprising of the parameters.
        /// </summary>
        /// <param name="left">The left-hand side</param>
        /// <param name="op">The operator token</param>
        /// <param name="right">The right-hand side</param>
        public BinOp(AbstractSyntaxTreeNode left,
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