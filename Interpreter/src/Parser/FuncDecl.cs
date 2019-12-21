namespace Rewired.Interpreter {

    public class FuncDecl : AbstractSyntaxTreeNode {

        public string Name { get; }

        public AbstractSyntaxTreeNode Block;

        public FuncDecl(string name, AbstractSyntaxTreeNode block) {
            Name = name;
            Block = block;
        }

        public override object VisitNode(IAbstractSyntaxTreeNodeVisitor visitor) {
            return visitor.Visit(this);
        }
    }
}