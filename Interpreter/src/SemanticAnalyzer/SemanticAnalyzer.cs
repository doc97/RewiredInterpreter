using System;

namespace Rewired.Interpreter {

    /// <summary>
    /// SemanticAnalyzer implements the AST node visitor.
    /// 
    /// It will walk the tree and check for semantic errors like the use of
    /// unknown variables.
    /// </summary>
    public class SemanticAnalyzer : IAbstractSyntaxTreeNodeVisitor {

        /// <summary>
        /// The AST to analyze.
        /// </summary>
        private AbstractSyntaxTreeNode tree;

        /// <summary>
        /// Gets the symbol table.
        /// </summary>
        /// <value></value>
        public SymbolTable Symbols { get; }

        /// <summary>
        /// Instantiates a new instance with built-in symbols added.
        /// </summary>
        /// <param name="tree">The AST to analyze</param>
        public SemanticAnalyzer(AbstractSyntaxTreeNode tree) {
            this.tree = tree;
            Symbols = new SymbolTable();
            InitBuiltInTypes();
        }

        /// <summary>
        /// Adds built-in types to the symbol table.
        /// </summary>
        private void InitBuiltInTypes() {
            Symbols.Insert(new BuiltInTypeSymbol("INTEGER"));
            Symbols.Insert(new BuiltInTypeSymbol("REAL"));
        }

        /// <summary>
        /// Walks the AST.
        /// </summary>
        /// <exception cref="System.Exception">
        /// Thrown when there is a semantic error.
        /// </exception>
        public void Analyze() {
            tree.VisitNode(this);
        }

        #region IAbstractSyntaxTreeNodeVisitor
        public object Visit(NoOp op) {
            return null;
        }

        public object Visit(UnaryOp op) {
            return op.Expr.VisitNode(this);
        }

        public object Visit(BinOp op) {
            Symbol leftSymbol = (Symbol) op.Left.VisitNode(this);
            Symbol rightSymbol = (Symbol) op.Right.VisitNode(this);
            if (leftSymbol != rightSymbol) {
                throw new Exception(string.Format("Type mismatch: Cannot perform '{0} {1} {2}",
                    leftSymbol.TypeName, op.Op.Value, rightSymbol.TypeName));
            }
            return leftSymbol.Type;
        }

        public object Visit(Int num) {
            return Symbols.Lookup("INTEGER");
        }

        public object Visit(Assign assign) {
            Symbol rightType = (Symbol) assign.Right.VisitNode(this);

            string varName = ((Var) assign.Left).Value;
            Symbol varType = Symbols.Lookup(varName) ?? rightType;
            Symbols.Insert(new VarSymbol(varName, varType));
            return null;
        }

        public object Visit(Var var) {
            string varName = var.Value;
            Symbol varSymbol = Symbols.Lookup(varName);
            if (varSymbol == null) {
                throw new Exception(string.Format("Error: Identifier '{0}' not found", varName));
            }
            return varSymbol.Type;
        }

        public object Visit(Compound comp) {
            foreach (AbstractSyntaxTreeNode child in comp.Children) {
                child.VisitNode(this);
            }
            return null;
        }

        public object Visit(FuncDecl func) {
            return null;
        }

        public object Visit(Program program) {
            return program.Block.VisitNode(this);
        }
        #endregion
    }

}