namespace Rewired.Interpreter {

    public class Var : AbstractSyntaxTree {

        public Token Token { get; }
        public string Value { get => Token.Value; }

        public Var(Token token) {
            Token = token;
        }

        public override object Accept(IAbstractSyntaxTreeNodeVisitor visitor) {
            return visitor.Visit(this);
        }
    }
}