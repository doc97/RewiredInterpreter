namespace Rewired.Interpreter {

    public class Assign : AbstractSyntaxTreeNode {

        public AbstractSyntaxTreeNode Left { get; }
        public AbstractSyntaxTreeNode Right { get; }
        public Token Op { get; }

        public Assign(AbstractSyntaxTreeNode left,
                      Token op,
                      AbstractSyntaxTreeNode right) {
            Left = left;
            Op = op;
            Right = right;
        }

        public override object VisitNode(IAbstractSyntaxTreeNodeVisitor visitor) {
            return visitor.Visit(this);
        }
    }

}