using System;
using NUnit.Framework;

namespace Rewired.Interpreter.Tests {

    [TestFixture]
    public class TestSemanticAnalyzer {

        [TestCase("a := a;")]
        [TestCase("a := b;")]
        [TestCase("a := 1; b := a + c;")]
        public void TestUndeclaredVariableThrowsException(string text) {
            AbstractSyntaxTree tree = new Parser(new Lexer(text)).Parse();
            SemanticAnalyzer analyzer = new SemanticAnalyzer(tree);
            Assert.Throws<Exception>(() => analyzer.Analyze());
        }

        [TestCase("a := 1;", ExpectedResult = "INTEGER")]
        [TestCase("b := 1; a := b;", ExpectedResult = "INTEGER")]
        public string TestSymbolType(string text) {
            AbstractSyntaxTree tree = new Parser(new Lexer(text)).Parse();
            SemanticAnalyzer analyzer = new SemanticAnalyzer(tree);
            analyzer.Analyze();
            Symbol s = analyzer.Symbols.Lookup("a");
            return s.TypeName;
        }
    }
}