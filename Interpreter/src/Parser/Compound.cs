namespace Rewired.Interpreter {

    public class Compound : AbstractSyntaxTree {

        public AbstractSyntaxTree[] Children { get; }

        public Compound(AbstractSyntaxTree[] children) {
            Children = children;
        }

        public override object Accept(IAbstractSyntaxTreeNodeVisitor visitor) {
            return visitor.Visit(this);
        }
    }
}