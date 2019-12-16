namespace Rewired.Interpreter {

    public interface IAbstractSyntaxTreeVisitor {
        object Visit(NoOp op);
        object Visit(UnaryOp op);
        object Visit(BinOp op);
        object Visit(Num num);
        object Visit(Assign assign);
        object Visit(Var var);
        object Visit(Compound comp);
    }

}