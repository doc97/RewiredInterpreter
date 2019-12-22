namespace Rewired.Interpreter {

    /// <summary>
    /// Program represents a program AST node. 
    /// </summary>
    public class Program : AbstractSyntaxTreeNode {

        /// <summary>
        /// Gets the name of the program.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the program code block.
        /// </summary>
        /// <value>The block is a compound statement</value>
        public AbstractSyntaxTreeNode Block { get; }

        /// <summary>
        /// Instantiates a new node instance with the specified name and code block.
        /// </summary>
        /// <param name="name">The name of the program</param>
        /// <param name="block">A `Compound` node containing the statements and declarations</param>
        public Program(string name, AbstractSyntaxTreeNode block) {
            Name = name;
            Block = block;
        }

        public override object VisitNode(IAbstractSyntaxTreeNodeVisitor visitor) {
            return visitor.Visit(this);
        }
    }
}