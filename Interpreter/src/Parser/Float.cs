namespace Rewired.Interpreter {

    /// <summary>
    /// Float represents a float AST node.
    /// </summary>
    public class Float : AbstractSyntaxTreeNode {

        /// <summary>
        /// The float token.
        /// </summary>
        public Token Token { get; }

        /// <summary>
        /// Gets the float value as a string.
        /// </summary>
        /// <value>The value of the token.</value>
        public string Value { get => Token.Value; }

        /// <summary>
        /// Instantiates a new node instance from the token.
        /// </summary>
        /// <param name="token">The float token</param>
        public Float(Token token) {
            Token = token;
        }

        public override object VisitNode(IAbstractSyntaxTreeNodeVisitor visitor) {
            return visitor.Visit(this);
        }
    }

}