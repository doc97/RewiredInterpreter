using NUnit.Framework;

namespace Rewired.Interpreter.Tests {

    [TestFixture]
    public class TestParser {

        [Test]
        public void Parse_EmptyStatement() {
            Parser parser = new Parser(new Tokenizer(""));
            Assert.True(parser.Parse() is Program);
        }

        [Test]
        public void Parse_HasNoState() {
            Parser parser = new Parser(new Tokenizer("a := 1;"));
            Assert.True(parser.Parse() is Program);
            Assert.True(parser.Parse() is Program);
        }

        [Test]
        public void Parse_MultipleStatements() {
            Parser parser = new Parser(new Tokenizer("a := 1; b := 2;"));
            Assert.True(parser.Parse() is Program);
        }

        [TestCase("func A() { }", ExpectedResult=true)]
        [TestCase("a := 1;", ExpectedResult=false)]
        public bool Parse_FunctionDecl(string text) {
            Parser parser = new Parser(new Tokenizer(text));
            AbstractSyntaxTreeNode root = parser.Parse();
            TestASTNodeVisitor visitor = new TestASTNodeVisitor();
            root.VisitNode(visitor);
            return visitor.FuncDeclVisited;
        }

        private class TestASTNodeVisitor : IAbstractSyntaxTreeNodeVisitor {

            public bool NoOpVisited { get; private set; }
            public bool UnaryOpVisited { get; private set; }
            public bool BinOpVisited { get; private set; }
            public bool IntVisited { get; private set; }
            public bool AssignVisited { get; private set; }
            public bool VarVisited { get; private set; }
            public bool FuncDeclVisited { get; private set; }
            public bool CompoundVisited { get; private set; }

            public object Visit(NoOp op) {
                NoOpVisited = true;
                return null;
            }

            public object Visit(UnaryOp op) {
                UnaryOpVisited = true;
                return null;
            }

            public object Visit(BinOp op) {
                BinOpVisited = true;
                return null;
            }

            public object Visit(Int num) {
                IntVisited = true;
                return null;
            }

            public object Visit(Assign assign) {
                AssignVisited = true;
                return null;
            }

            public object Visit(Var var) {
                VarVisited = true;
                return null;
            }

            public object Visit(Compound comp) {
                foreach (AbstractSyntaxTreeNode child in comp.Children) {
                    child.VisitNode(this);
                }
                CompoundVisited = true;
                return null;
            }

            public object Visit(FuncDecl func) {
                FuncDeclVisited = true;
                return null;
            }

            public object Visit(Program program) {
                return program.Block.VisitNode(this);
            }
        }
    }

}