namespace Rewired.Interpreter {

    public class Type : AbstractSyntaxTreeNode {

        public Token Token { get; }

        public Type(Token token) {
            Token = token;
        }

        public override object VisitNode(IAbstractSyntaxTreeNodeVisitor visitor) {
            return visitor.Visit(this);
        }
    }
}