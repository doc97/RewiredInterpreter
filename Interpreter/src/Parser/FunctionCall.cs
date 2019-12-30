namespace Rewired.Interpreter {

    /// <summary>
    /// FunctionCall represents a function call statement.
    /// </summary>
    public class FunctionCall : AbstractSyntaxTreeNode {

        /// <summary>
        /// Gets the name of the function.
        /// </summary>
        public string Name { get => Token.Value; }

        /// <summary>
        /// Gets the nodes containing the values of the function parameters.
        /// </summary>
        public AbstractSyntaxTreeNode[] Arguments { get; }

        /// <summary>
        /// Gets the identifier token (containing the name of the function).
        /// </summary>
        public Token Token { get; }

        /// <summary>
        /// Instantiates a new node instance with the specified function name
        /// and arguments.
        /// </summary>
        /// <param name="name">The token containing of the function</param>
        /// <param name="arguments">The var nodes containing the arguments</param>
        public FunctionCall(Token name, AbstractSyntaxTreeNode[] arguments) {
            Token = name;
            Arguments = arguments;
        }

        public override object VisitNode(IAbstractSyntaxTreeNodeVisitor visitor) {
            return visitor.Visit(this);
        }
    }

}