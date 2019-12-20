namespace Rewired.Interpreter {

    public class UnaryOp : AbstractSyntaxTree {

        public AbstractSyntaxTree Expr { get; }
        public Token Op { get; }

        public UnaryOp(Token op, AbstractSyntaxTree expr) {
            Op = op;
            Expr = expr;
        }

        public override object Accept(IAbstractSyntaxTreeNodeVisitor visitor) {
            return visitor.Visit(this);
        }
    }
}