namespace Rewired.Interpreter {

    /// <summary>
    /// Compound represents a compound statement AST node.
    /// 
    /// It is simply a container for other AST nodes. Visiting this node should
    /// traverse the children.
    /// </summary>
    public class Compound : AbstractSyntaxTreeNode {

        /// <summary>
        /// Gets the child AST nodes.
        /// </summary>
        /// <value></value>
        public AbstractSyntaxTreeNode[] Children { get; }

        /// <summary>
        /// Instantiates a new node instance with the specified children.
        /// </summary>
        /// <param name="children"></param>
        public Compound(AbstractSyntaxTreeNode[] children) {
            Children = children;
        }

        public override object VisitNode(IAbstractSyntaxTreeNodeVisitor visitor) {
            return visitor.Visit(this);
        }
    }
}