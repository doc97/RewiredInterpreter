namespace InterpreterPractice {

    public interface IAbstractSyntaxTreeVisitor {
        int Visit(UnaryOp op);
        int Visit(BinOp op);
        int Visit(Num num);
    }

}