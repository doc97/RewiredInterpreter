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
        /// Gets the node containing the statements.
        /// </summary>
        public AbstractSyntaxTreeNode Block { get; }

        /// <summary>
        /// Instantiates a new node instance with the specified function name
        /// and code block.
        /// </summary>
        /// <param name="name">The name of the function</param>
        /// <param name="block">A `Compound` node containing the statements</param>
        public FuncDecl(string name, AbstractSyntaxTreeNode block) {
            Name = name;
            Block = block;
        }

        public override object VisitNode(IAbstractSyntaxTreeNodeVisitor visitor) {
            return visitor.Visit(this);
        }
    }

}