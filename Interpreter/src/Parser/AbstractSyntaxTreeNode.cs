
namespace Rewired.Interpreter {

    /// <summary>
    /// AbstractSyntaxTreeNode represents a node in an Abstract Syntax Tree.
    /// 
    /// The actual traversal of the tree is implemented by using the visitor
    /// pattern.
    /// 
    /// <seealso cref="IAbstractSyntaxTreeNodeVisitor" />
    /// </summary>
    public abstract class AbstractSyntaxTreeNode {
        public abstract object VisitNode(IAbstractSyntaxTreeNodeVisitor visitor);
    }

}