using System;
using NUnit.Framework;

namespace Rewired.Interpreter.Tests {

    [TestFixture]
    public class TestSemanticAnalyzer {

        [TestCase("a := a;")]
        [TestCase("a := b;")]
        [TestCase("a := 1; b := a + c;")]
        [TestCase("func A() {} a := A;")]
        public void UndeclaredVariableThrowsError(string text) {
            AbstractSyntaxTreeNode tree = new Parser(new Tokenizer(text)).Parse();
            SemanticAnalyzer analyzer = new SemanticAnalyzer(tree);
            try {
                analyzer.Analyze();
                Assert.Fail("Test case does not fail and did not throw an exception");
            } catch (SemanticError e) {
                Assert.AreEqual(SemanticError.ErrorCode.IdNotFound, e.Code);
            } catch (Exception) {
                Assert.Fail("Unrecognized exception thrown");
            }
        }

        [TestCase("func A() {} func A() {}")]
        public void DuplicateDeclarationThrowsError(string text) {
            AbstractSyntaxTreeNode tree = new Parser(new Tokenizer(text)).Parse();
            SemanticAnalyzer analyzer = new SemanticAnalyzer(tree);
            try {
                analyzer.Analyze();
                Assert.Fail("Test case does not fail and did not throw an exception");
            } catch (SemanticError e) {
                Assert.AreEqual(SemanticError.ErrorCode.DuplicateId, e.Code);
            } catch (Exception) {
                Assert.Fail("Unrecognized exception thrown");
            }
        }

        [TestCase("globA := 1; globB := globA;", "Global variable is not accessible in global scope")]
        [TestCase("globA := 0; func A() { a := 1; globA := a; }", "Global variable is not accessible in function scope")]
        [TestCase("globA := 0; func A(int a) { globA := a; }", "Function parameter is not accessible in function scope")]
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
