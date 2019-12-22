namespace Rewired.Interpreter {

    /// <summary>
    /// IAbstractSyntaxTreeNodeVisitor is implemented by those who will walk the
    /// Abstract Syntax Tree.
    ///
    /// Each function corresponds to a node type. The decision of which function
    /// is called is determined during runtime through overloading.
    /// </summary>
    public interface IAbstractSyntaxTreeNodeVisitor {
        object Visit(NoOp op);
        object Visit(UnaryOp op);
        object Visit(BinOp op);
        object Visit(Int num);
        object Visit(Assign assign);
        object Visit(Var var);
        object Visit(Type type);
        object Visit(Parameter param);
        object Visit(Compound comp);
        object Visit(FuncDecl func);
        object Visit(Program program);
    }

}