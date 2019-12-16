namespace Rewired.Interpreter {
    public class NoOp : AbstractSyntaxTree {
        public override object Accept(IAbstractSyntaxTreeVisitor visitor) {
            return visitor.Visit(this);
        }
    }
}