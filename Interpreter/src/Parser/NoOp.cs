namespace Rewired.Interpreter {
    public class NoOp : AbstractSyntaxTreeNode {
        public override object VisitNode(IAbstractSyntaxTreeNodeVisitor visitor) {
            return visitor.Visit(this);
        }
    }
}