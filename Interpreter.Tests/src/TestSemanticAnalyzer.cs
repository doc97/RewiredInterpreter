using System;
using NUnit.Framework;

namespace Rewired.Interpreter.Tests {

    [TestFixture]
    public class TestSemanticAnalyzer {

        [TestCase("a := a;")]
        [TestCase("a := b;")]
        [TestCase("a := 1; b := a + c;")]
        public void TestUndeclaredVariableThrowsException(string text) {
            AbstractSyntaxTreeNode tree = new Parser(new Tokenizer(text)).Parse();
            SemanticAnalyzer analyzer = new SemanticAnalyzer(tree);
            Assert.Throws<Exception>(() => analyzer.Analyze());
        }

        [TestCase("a := 1;", ExpectedResult = "INTEGER")]
        [TestCase("b := 1; a := b;", ExpectedResult = "INTEGER")]
        public string TestSymbolType(string text) {
            AbstractSyntaxTreeNode tree = new Parser(new Tokenizer(text)).Parse();
            SemanticAnalyzer analyzer = new SemanticAnalyzer(tree);
            analyzer.Analyze();
            Symbol s = analyzer.LookupSymbol("a");
            return s.TypeName;
        }

        [Test]
        public void FunctionSymbolIsInScope() {
            string text = "func A() {}";
            AbstractSyntaxTreeNode tree = new Parser(new Tokenizer(text)).Parse();
            SemanticAnalyzer analyzer = new SemanticAnalyzer(tree);
            analyzer.Analyze();
            Symbol s = analyzer.LookupSymbol("A");
            Assert.IsInstanceOf(typeof(FunctionSymbol), s);
       }

        [Test]
        public void SymbolsInFunctionAddedToInnerScope() {
            string text = "globA := 0; func A() { a := 1; globA := a; }";
            AbstractSyntaxTreeNode tree = new Parser(new Tokenizer(text)).Parse();
            SemanticAnalyzer analyzer = new SemanticAnalyzer(tree);
            analyzer.Analyze();
            Assert.IsNull(analyzer.LookupSymbol("a"));
        }
    }
}
