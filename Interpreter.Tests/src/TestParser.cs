using System;
using NUnit.Framework;

namespace Rewired.Interpreter.Tests {

    [TestFixture]
    public class TestParser {

        [Test]
        public void Parse_EmptyStatement() {
            Parser parser = new Parser(new Tokenizer(""));
            Assert.IsTrue(parser.Parse() is Program);
        }

        [Test]
        public void Parse_HasNoState() {
            Parser parser = new Parser(new Tokenizer("a := 1;"));
            Assert.IsTrue(parser.Parse() is Program);
            Assert.IsTrue(parser.Parse() is Program);
        }

        [Test]
        public void Parse_MultipleStatements() {
            Parser parser = new Parser(new Tokenizer("a := 1; b := 2;"));
            Assert.IsTrue(parser.Parse() is Program);
        }

        [TestCase("func A() { }", ExpectedResult = true)]
        [TestCase("a := 1;", ExpectedResult = false)]
        public bool Parse_FunctionDeclaration(string text) {
            Parser parser = new Parser(new Tokenizer(text));
            AbstractSyntaxTreeNode root = parser.Parse();
            TestASTNodeVisitor visitor = new TestASTNodeVisitor();
            root.VisitNode(visitor);
            return visitor.FunctionDeclarationVisited;
        }

        [Test]
        public void Parse_FunctionDeclaration_WithOneParameter() {
            Parser parser = new Parser(new Tokenizer("func A(int a) {}"));
            AbstractSyntaxTreeNode root = parser.Parse();
            TestASTNodeVisitor visitor = new TestASTNodeVisitor();
            root.VisitNode(visitor);
            Assert.IsTrue(visitor.TypeVisited);
            Assert.IsTrue(visitor.ParameterVisited);
        }

        [Test]
        public void Parse_FunctionDeclaration_MultipleParameters() {
            Parser parser = new Parser(new Tokenizer("func A(int a, int b) {}"));
            AbstractSyntaxTreeNode root = parser.Parse();
            TestASTNodeVisitor visitor = new TestASTNodeVisitor();
            root.VisitNode(visitor);
            Assert.IsTrue(visitor.TypeVisited);
            Assert.AreEqual(visitor.ParameterVisitedCount, 2);
        }

        [Test]
        public void Parse_FunctionDeclaration_ReturnStatement() {
            Parser parser = new Parser(new Tokenizer("func A() { return 0; }"));
            AbstractSyntaxTreeNode root = parser.Parse();
            TestASTNodeVisitor visitor = new TestASTNodeVisitor();
            root.VisitNode(visitor);
            // Return statement only visited during interpretation
            Assert.IsFalse(visitor.ReturnVisited);
        }

        [TestCase("func A() {} A();")]
        [TestCase("func B(int b) {} B(0);")]
        [TestCase("func C(int c1, int c2) {} C(1, 2);")]
        public void Parse_FunctionCall(string text) {
            Parser parser = new Parser(new Tokenizer(text));
            AbstractSyntaxTreeNode root = parser.Parse();
            TestASTNodeVisitor visitor = new TestASTNodeVisitor();
            root.VisitNode(visitor);
            Assert.IsTrue(visitor.FunctionCallVisited);
        }

        [TestCase("a/ := 1;")]
        public void Parse_InvalidTokenThrowsException(string text) {
            Parser parser = new Parser(new Tokenizer(text));
            try {
                parser.Parse();
                Assert.Fail("Test case does not fail and did not throw an exception");
            } catch (ParserError err) {
                Assert.AreEqual(ParserError.ErrorCode.UnexpectedToken, err.Code);
                Assert.AreEqual("/", err.Token.Value);
            } catch (Exception ex) {
                Assert.Fail(string.Format(
                    "Unrecognized exception thrown: ({0}): {1}",
                    ex.GetType(), ex.Message)
                );
            }
        }

        private class TestASTNodeVisitor : IAbstractSyntaxTreeNodeVisitor {

            public bool NoOpVisited { get; private set; }
            public bool UnaryOpVisited { get; private set; }
            public bool BinaryOpVisited { get; private set; }
            public bool IntVisited { get; private set; }
            public bool AssignVisited { get; private set; }
            public bool ReturnVisited { get; private set; }
            public bool VarVisited { get; private set; }
            public bool TypeVisited { get; private set; }
            public int ParameterVisitedCount { get; private set; }
            public bool ParameterVisited { get => ParameterVisitedCount > 0; }
            public bool FunctionDeclarationVisited { get; private set; }
            public bool FunctionCallVisited { get; private set; }
            public bool CompoundVisited { get; private set; }

            public object Visit(NoOp op) {
                NoOpVisited = true;
                return null;
            }

            public object Visit(UnaryOp op) {
                UnaryOpVisited = true;
                return null;
            }

            public object Visit(BinaryOp op) {
                BinaryOpVisited = true;
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

            public object Visit(Return ret) {
                ReturnVisited = true;
                return null;
            }

            public object Visit(Var var) {
                VarVisited = true;
                return null;
            }

            public object Visit(Type type) {
                TypeVisited = true;
                return null;
            }

            public object Visit(Parameter param) {
                ParameterVisitedCount++;
                param.Type.VisitNode(this);
                return null;
            }

            public object Visit(Compound comp) {
                foreach (AbstractSyntaxTreeNode child in comp.Children) {
                    child.VisitNode(this);
                }
                CompoundVisited = true;
                return null;
            }

            public object Visit(FunctionDeclaration func) {
                FunctionDeclarationVisited = true;
                foreach (AbstractSyntaxTreeNode param in func.Parameters) {
                    param.VisitNode(this);
                }
                return null;
            }

            public object Visit(FunctionCall call) {
                FunctionCallVisited = true;
                return null;
            }

            public object Visit(Program program) {
                return program.Block.VisitNode(this);
            }
        }
    }

}