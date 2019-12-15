namespace InterpreterPractice {

    public interface IAbstractSyntaxTreeVisitor {
        object Visit(NoOp op);
        object Visit(UnaryOp op);
        object Visit(BinOp op);
        object Visit(Num num);
    }

}