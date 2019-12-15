namespace InterpreterPractice {

    public class Compound : AbstractSyntaxTree {

        public AbstractSyntaxTree[] Children { get; }

        public Compound(AbstractSyntaxTree[] children) {
            Children = children;
        }

        public override object Accept(IAbstractSyntaxTreeVisitor visitor) {
            return visitor.Visit(this);
        }
    }
}