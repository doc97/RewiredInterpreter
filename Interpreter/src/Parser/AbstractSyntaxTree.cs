
namespace InterpreterPractice {
    public abstract class AbstractSyntaxTree {
        public abstract object Accept(IAbstractSyntaxTreeVisitor visitor);
    }
}