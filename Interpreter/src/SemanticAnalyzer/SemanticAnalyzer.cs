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
        /// The current scope's symbol table.
        /// </summary>
        private ScopedSymbolTable currentScope;

        /// <summary>
        /// Instantiates a new instance with built-in symbols added.
        /// </summary>
        /// <param name="tree">The AST to analyze</param>
        public SemanticAnalyzer(AbstractSyntaxTreeNode tree) {
            this.tree = tree;
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

        /// <summary>
        /// Looks up a symbol in the current scope.
        /// </summary>
        /// <param name="name">The name of the symbol</param>
        /// <returns>The symbol if it is found, otherwise null</returns>
        public Symbol LookupSymbol(string name) {
            return currentScope.Lookup(name);
        }

        /// <summary>
        /// Prints out the current scope.
        /// </summary>
        public void PrintScopeInfo() {
            Console.WriteLine("--== Semantic Analysis ==--");
            Console.WriteLine(currentScope);
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
            return currentScope.Lookup("INTEGER");
        }

        public object Visit(Assign assign) {
            Symbol rightType = (Symbol) assign.Right.VisitNode(this);

            string varName = ((Var) assign.Left).Value;
            Symbol varType = currentScope.Lookup(varName) ?? rightType;
            currentScope.Insert(new VarSymbol(varName, varType));
            return null;
        }

        public object Visit(Var var) {
            string varName = var.Value;
            Symbol varSymbol = currentScope.Lookup(varName);
            if (varSymbol == null) {
                throw new Exception(string.Format("Error: Identifier '{0}' not found", varName));
            }
            return varSymbol.Type;
        }

        public object Visit(Type type) {
            return null;
        }

        public object Visit(Parameter param) {
            param.Type.VisitNode(this);
            return null;
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
            ScopedSymbolTable globalScope = new ScopedSymbolTable("Global", 0, null);
            globalScope.InitBuiltInTypes();

            currentScope = globalScope;
            object retVal = program.Block.VisitNode(this);
            /* The current scope is not exited with:
             *   currentScope = currentScope.Parent
             * because it is needed for testing.
            */
            return retVal;
        }
        #endregion
    }

}