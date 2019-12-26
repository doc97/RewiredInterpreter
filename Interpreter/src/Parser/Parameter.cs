namespace Rewired.Interpreter {

    public class Parameter : AbstractSyntaxTreeNode {

        /// <summary>
        /// Gets the parameter type
        /// </summary>
        /// <value>A Type AST node</value>
        public AbstractSyntaxTreeNode Type { get; }

        /// <summary>
        /// Gets the name of the parameter.
        /// </summary>
        /// <value>A Var AST node containing the name</value>
        public AbstractSyntaxTreeNode Name { get; }

        public Parameter(AbstractSyntaxTreeNode type, AbstractSyntaxTreeNode name) {
            Type = type;
            Name = name;
        }

        public override object VisitNode(IAbstractSyntaxTreeNodeVisitor visitor) {
            return visitor.Visit(this);
        }
    }
}
