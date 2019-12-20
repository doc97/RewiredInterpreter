namespace Rewired.Interpreter {

    /// <summary>
    /// Num represents an integer AST node.
    /// </summary>
    public class Num : AbstractSyntaxTreeNode {

        /// <summary>
        /// The integer token.
        /// </summary>
        private Token token;

        /// <summary>
        /// Gets the integer value as a string.
        /// </summary>
        /// <value>The value of the token.</value>
        public string Value { get => token.Value; }

        /// <summary>
        /// Instantiates a new node instance from the token.
        /// </summary>
        /// <param name="token">The integer token</param>
        public Num(Token token) {
            this.token = token;
        }

        public override object VisitNode(IAbstractSyntaxTreeNodeVisitor visitor) {
            return visitor.Visit(this);
        }
    }

}