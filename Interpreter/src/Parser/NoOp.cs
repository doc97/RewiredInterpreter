namespace InterpreterPractice {
    public class NoOp : AbstractSyntaxTree {
        public override object Accept(IAbstractSyntaxTreeVisitor visitor) {
            return visitor.Visit(this);
        }
    }
}