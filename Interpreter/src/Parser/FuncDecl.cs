namespace Rewired.Interpreter {

    /// <summary>
    /// FuncDecl represents a function declaration.
    /// </summary>
    public class FuncDecl : AbstractSyntaxTreeNode {

        /// <summary>
        /// Gets the name of the function.
        /// </summary>
        public string Name { get; }

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
        /// <param name="name">The name of the function</param>
        /// <param name="parameters">The var nodes containing the
        /// parameters</param>
        /// <param name="block">A `Compound` node containing the statements</param>
        public FuncDecl(string name, AbstractSyntaxTreeNode[] parameters, AbstractSyntaxTreeNode block) {
            Name = name;
            Parameters = parameters;
            Block = block;
        }

        public override object VisitNode(IAbstractSyntaxTreeNodeVisitor visitor) {
            return visitor.Visit(this);
        }
    }

}
