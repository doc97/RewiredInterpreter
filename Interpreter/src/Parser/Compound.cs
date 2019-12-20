namespace Rewired.Interpreter {

    public class Compound : AbstractSyntaxTreeNode {

        public AbstractSyntaxTreeNode[] Children { get; }

        public Compound(AbstractSyntaxTreeNode[] children) {
            Children = children;
        }

        public override object VisitNode(IAbstractSyntaxTreeNodeVisitor visitor) {
            return visitor.Visit(this);
        }
    }
}