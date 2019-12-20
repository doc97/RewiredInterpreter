namespace Rewired.Interpreter {

    public class BinOp : AbstractSyntaxTree {

        public AbstractSyntaxTree Left { get; }
        public AbstractSyntaxTree Right { get; }
        public Token Op { get; }

        public BinOp(AbstractSyntaxTree left,
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