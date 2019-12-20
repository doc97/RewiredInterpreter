namespace Rewired.Interpreter {

    /// <summary>
    /// Var represents the variable AST node.
    /// </summary>
    public class Var : AbstractSyntaxTreeNode {

        /// <summary>
        /// Gets the variable token.
        /// </summary>
        /// <value>The token is an ID token.</value>
        public Token Token { get; }

        /// <summary>
        /// Gets the token value.
        /// </summary>
        /// <value>The name of the varible.</value>
        public string Value { get => Token.Value; }

        /// <summary>
        /// Instatiates a new instance
        /// </summary>
        /// <param name="token">The token containing the name of the variable.</param>
        public Var(Token token) {
            Token = token;
        }

        public override object VisitNode(IAbstractSyntaxTreeNodeVisitor visitor) {
            return visitor.Visit(this);
        }
    }
}