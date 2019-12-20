namespace Rewired.Interpreter {

    public class Assign : AbstractSyntaxTree {

        public AbstractSyntaxTree Left { get; }
        public AbstractSyntaxTree Right { get; }
        public Token Op { get; }

        public Assign(AbstractSyntaxTree left,
                      Token op,
                      AbstractSyntaxTree right) {
            Left = left;
            Op = op;
            Right = right;
        }

        public override object Accept(IAbstractSyntaxTreeNodeVisitor visitor) {
            return visitor.Visit(this);
        }
    }

}