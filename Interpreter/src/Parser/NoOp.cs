namespace Rewired.Interpreter {

    /// <summary>
    /// NoOp represents an empty statement AST node.
    /// </summary>
    public class NoOp : AbstractSyntaxTreeNode {
        public override object VisitNode(IAbstractSyntaxTreeNodeVisitor visitor) {
            return visitor.Visit(this);
        }
    }

}