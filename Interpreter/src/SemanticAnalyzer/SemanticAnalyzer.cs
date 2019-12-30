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
        /// Whether to exit the root scope after analyzing the AST.
        /// To perform testing or debugging, please set this to false.
        /// </summary>
        public bool ShouldExitRootScope { get; set; } = true;

        /// <summary>
        /// Whether logging is enabled / disabled.
        /// </summary>
        public bool IsLoggingEnabled { get; set; } = false;

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
        /// <exception cref="SemanticError">
        /// Thrown when there is a semantic error.
        /// </exception>
        public void Analyze() {
            tree.VisitNode(this);
        }

        /// <summary>
        /// Prints out the current scope.
        /// Won't print unless <see cref="IsLoggingEnabled"/> is set to `true`.
        /// </summary>
        public void PrintScopeInfo() {
            if (IsLoggingEnabled) {
                Console.WriteLine("--== Semantic Analysis ==--");
                Console.WriteLine(currentScope);
            }
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
                throw new SemanticError(
                    SemanticError.ErrorCode.TypeMismatch,
                    op.Op,
                    string.Format("Type mismatch: Cannot perform '{0} {1} {2}",
                        leftSymbol.TypeName, op.Op.Value, rightSymbol.TypeName)
                );
            }
            return leftSymbol.Type;
        }

        public object Visit(Int num) {
            return currentScope.LookupSymbol("INTEGER");
        }

        public object Visit(Assign assign) {
            Symbol rightType = (Symbol) assign.Right.VisitNode(this);

            string varName = ((Var) assign.Left).Value;
            // If the variable does not exist, infer type from the right-hand side
            Symbol varType = currentScope.LookupVariable(varName)?.Type ?? rightType;
            currentScope.InsertVariable(new VarSymbol(varName, varType));
            return null;
        }

        public object Visit(Var var) {
            string varName = var.Value;
            Symbol varSymbol = currentScope.LookupVariable(varName);
            if (varSymbol == null) {
                throw new SemanticError(
                    SemanticError.ErrorCode.IdNotFound,
                    var.Token,
                    string.Format("Error: Variable '{0}' not found", varName)
                );
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
            if (currentScope.HasSymbol(func.Name)) {
                throw new SemanticError(
                    SemanticError.ErrorCode.DuplicateId,
                    func.Token,
                    string.Format("Error: Identifier {0} already exists in the scope", func.Name)
                );
            }

            VarSymbol[] paramSymbols = new VarSymbol[func.Parameters.Length];
            for (int i = 0; i < paramSymbols.Length; i++) {
                Parameter p = (Parameter) func.Parameters[i];
                string name = ((Var) p.Name).Value;
                string type = ((Type) p.Type).Value;
                Symbol typeSymbol = currentScope.LookupSymbol(type);
                paramSymbols[i] = new VarSymbol(name, typeSymbol);
            }

            currentScope.InsertSymbol(new FunctionSymbol(func.Name, paramSymbols));
            currentScope = new ScopedSymbolTable("", 1, currentScope);

            foreach (VarSymbol param in paramSymbols) {
                currentScope.InsertVariable(param);
            }

            func.Block.VisitNode(this);
            currentScope = currentScope.Parent;

            return null;
        }

        public object Visit(Program program) {
            ScopedSymbolTable globalScope = new ScopedSymbolTable("Global", 0, null);
            globalScope.InitBuiltInTypes();

            currentScope = globalScope;
            object retVal = program.Block.VisitNode(this);
            if (ShouldExitRootScope) {
                currentScope = currentScope.Parent;
            }
            return retVal;
        }
        #endregion
    }

}
