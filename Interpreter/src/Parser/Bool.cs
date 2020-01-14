namespace Rewired.Interpreter {

    /// <summary>
    /// Bool represents a boolean AST node.
    /// </summary>
    public class Bool : AbstractSyntaxTreeNode {

        /// <summary>
        /// The boolean token.
        /// </summary>
        public Token Token { get; }

        /// <summary>
        /// Gets the boolean value as a string.
        /// </summary>
        /// <value>The value of the token.</value>
        public string Value { get => Token.Value; }

        /// <summary>
        /// Instantiates a new node instance from the token.
        /// </summary>
        /// <param name="token">The float token</param>
        public Bool(Token token) {
            Token = token;
        }

        public override object VisitNode(IAbstractSyntaxTreeNodeVisitor visitor) {
            return visitor.Visit(this);
        }
    }
}