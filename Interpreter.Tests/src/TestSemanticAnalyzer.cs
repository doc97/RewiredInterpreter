using System;
using NUnit.Framework;

namespace Rewired.Interpreter.Tests {

    [TestFixture]
    public class TestSemanticAnalyzer {

        [TestCase("int a := a;", 1, 10)]
        [TestCase("int a := b;", 1, 10)]
        [TestCase("int a := 1;\nint b := a + c;", 2, 14)]
        [TestCase("func A() {} int a := A;", 1, 22)]
        public void UndeclaredVariableThrowsError(string text, int errLine, int errCol) {
            AbstractSyntaxTreeNode tree = new Parser(new Tokenizer(text)).Parse();
            SemanticAnalyzer analyzer = new SemanticAnalyzer(tree);
            try {
                analyzer.Analyze();
                Assert.Fail("Test case does not fail and did not throw an exception");
            } catch (SemanticError err) {
                Assert.AreEqual(SemanticError.ErrorCode.IdNotFound, err.Code);
                Assert.AreEqual(errLine, err.Token.Line);
                Assert.AreEqual(errCol, err.Token.Column);
            } catch (Exception ex) {
                Assert.Fail(string.Format(
                    "Unrecognized exception thrown: ({0}): {1}",
                    ex.GetType(), ex.Message
                ));
            }
        }

        [TestCase("func A() {} func A() {}")]
        public void DuplicateFunctionDeclarationThrowsError(string text) {
            AbstractSyntaxTreeNode tree = new Parser(new Tokenizer(text)).Parse();
            SemanticAnalyzer analyzer = new SemanticAnalyzer(tree);
            try {
                analyzer.Analyze();
                Assert.Fail("Test case does not fail and did not throw an exception");
            } catch (SemanticError err) {
                Assert.AreEqual(SemanticError.ErrorCode.DuplicateId, err.Code);
            } catch (Exception ex) {
                Assert.Fail(string.Format(
                    "Unrecognized exception thrown: ({0}): {1}",
                    ex.GetType(), ex.Message
                ));
            }
        }

        [TestCase("float a := 1f + 1;")]
        [TestCase("func Sum(float a, int b) { return a + b; }")]
        [TestCase("func Increment(float a) { return a + 1; }")]
        public void TypeMismatchThrowsError(string text) {
            AbstractSyntaxTreeNode tree = new Parser(new Tokenizer(text)).Parse();
            SemanticAnalyzer analyzer = new SemanticAnalyzer(tree);
            try {
                analyzer.Analyze();
                Assert.Fail("Test case does not fail and did not throw an exception");
            } catch (SemanticError err) {
                Assert.AreEqual(SemanticError.ErrorCode.TypeMismatch, err.Code);
            } catch (Exception ex) {
                Assert.Fail(string.Format(
                    "Unrecognized exception thrown: ({0}): {1}",
                    ex.GetType(), ex.Message
                ));
            }
        }

        [TestCase("int globA := 1; int globB := globA;", "Global variable is not accessible in global scope")]
        [TestCase("func A(int paramA) { int a := paramA; }", "Function parameter is not accessible in function scope")]
        public void ThrowsNoException(string text, string errMsg = "") {
            AbstractSyntaxTreeNode tree = new Parser(new Tokenizer(text)).Parse();
            SemanticAnalyzer analyzer = new SemanticAnalyzer(tree);
            try {
                analyzer.Analyze();
            } catch (Exception e) {
                Assert.Fail(errMsg + "\n" + e.Message);
            }
        }
    }

}
