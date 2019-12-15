namespace InterpreterPractice {

    public class Num : AbstractSyntaxTree {

        private Token token;
        public string Value { get => token.Value; }

        public Num(Token token) {
            this.token = token;
        }

        public override object Accept(IAbstractSyntaxTreeVisitor visitor) {
            return visitor.Visit(this);
        }
    }

}