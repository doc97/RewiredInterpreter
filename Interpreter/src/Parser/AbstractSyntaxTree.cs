
namespace InterpreterPractice {
    public abstract class AbstractSyntaxTree {
        public abstract int Accept(IAbstractSyntaxTreeVisitor visitor);
    }
}