namespace Rewired.Interpreter {
    public class NoOp : AbstractSyntaxTree {
        public override object Accept(IAbstractSyntaxTreeNodeVisitor visitor) {
            return visitor.Visit(this);
        }
    }
}