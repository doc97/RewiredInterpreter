namespace Rewired.Interpreter {

    public class UnaryOp : AbstractSyntaxTreeNode {

        public AbstractSyntaxTreeNode Expr { get; }
        public Token Op { get; }

        public UnaryOp(Token op, AbstractSyntaxTreeNode expr) {
            Op = op;
            Expr = expr;
        }

        public override object VisitNode(IAbstractSyntaxTreeNodeVisitor visitor) {
            return visitor.Visit(this);
        }
    }
}