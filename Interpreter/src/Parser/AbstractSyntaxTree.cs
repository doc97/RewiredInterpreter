
namespace Rewired.Interpreter {
    public abstract class AbstractSyntaxTree {
        public abstract object Accept(IAbstractSyntaxTreeNodeVisitor visitor);
    }
}