namespace Rewired.Interpreter {

    public class Num : AbstractSyntaxTreeNode {

        private Token token;
        public string Value { get => token.Value; }

        public Num(Token token) {
            this.token = token;
        }

        public override object VisitNode(IAbstractSyntaxTreeNodeVisitor visitor) {
            return visitor.Visit(this);
        }
    }

}