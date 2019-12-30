namespace Rewired.Interpreter {

    /// <summary>
    /// FunctionDeclaration represents a function declaration.
    /// </summary>
    public class FunctionDeclaration : AbstractSyntaxTreeNode {

        /// <summary>
        /// Gets the name of the function.
        /// </summary>
        public string Name { get => Token.Value; }

        /// <summary>
        /// Gets the identifier token (containing the name of the function).
        /// </summary>
        public Token Token { get; }

        /// <summary>
        /// Gets the nodes containing the parameters.
        /// </summary>
        public AbstractSyntaxTreeNode[] Parameters { get; }

        /// <summary>
        /// Gets the node containing the statements.
        /// </summary>
        public AbstractSyntaxTreeNode Block { get; }

        /// <summary>
        /// Instantiates a new node instance with the specified function name
        /// and code block.
        /// </summary>
        /// <param name="name">The token containing of the function</param>
        /// <param name="parameters">The var nodes containing the
        /// parameters</param>
        /// <param name="block">A `Compound` node containing the statements</param>
        public FunctionDeclaration(Token name, AbstractSyntaxTreeNode[] parameters, AbstractSyntaxTreeNode block) {
            Token = name;
            Parameters = parameters;
            Block = block;
        }

        public override object VisitNode(IAbstractSyntaxTreeNodeVisitor visitor) {
            return visitor.Visit(this);
        }
    }

}
