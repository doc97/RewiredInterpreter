namespace Rewired.Interpreter {

    /// <summary>
    /// If represents an if statement
    /// </summary>
    public class If : AbstractSyntaxTreeNode {

        /// <summary>
        /// Gets the node containing the if condition. 
        /// </summary>
        public AbstractSyntaxTreeNode Condition { get; }

        /// <summary>
        /// Gets the node containing statements to run if condition is
        /// evaluated to true.
        /// </summary>
        public AbstractSyntaxTreeNode TrueBlock { get; }

        /// <summary>
        /// Gets the node containing statements to run if condition is
        /// evaluated to false.
        /// </summary>
        public AbstractSyntaxTreeNode FalseBlock { get; }

        /// <summary>
        /// Instantiates a new node instance with the specified condition,
        /// true block and false block.
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="trueBlock"></param>
        /// <param name="falseBlock"></param>
        public If(AbstractSyntaxTreeNode condition,
                  AbstractSyntaxTreeNode trueBlock,
                  AbstractSyntaxTreeNode falseBlock) {
            Condition = condition;
            TrueBlock = trueBlock;
            FalseBlock = falseBlock;
        }

        public override object VisitNode(IAbstractSyntaxTreeNodeVisitor visitor) {
            return visitor.Visit(this);
        }
    }
}