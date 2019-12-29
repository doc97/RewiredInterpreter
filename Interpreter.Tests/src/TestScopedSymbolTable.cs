using NUnit.Framework;

namespace Rewired.Interpreter.Tests {

    [TestFixture]
    public class TestScopedSymbolTable {

        [Test]
        public void Test_InitBuiltInTypes() {
            ScopedSymbolTable scope = new ScopedSymbolTable("", 1, null);
            scope.InitBuiltInTypes();
            Assert.NotNull(scope.LookupSymbol("INTEGER"));
            Assert.NotNull(scope.LookupSymbol("REAL"));
        }

        [Test]
        public void Lookup_ParentScope() {
            ScopedSymbolTable parent = new ScopedSymbolTable("", 1, null);
            ScopedSymbolTable child = new ScopedSymbolTable("", 2, parent);

            parent.InsertSymbol(new Symbol("test", null));
            Assert.NotNull(child.LookupSymbol("test"));
        }

        [Test]
        public void Lookup_ParentNull() {
            ScopedSymbolTable scope = new ScopedSymbolTable("", 2, null);
            Assert.Null(scope.LookupSymbol("test"));
        }

        [Test]
        public void Lookup_VariableNotCheckedInParentScope() {
            ScopedSymbolTable parent = new ScopedSymbolTable("", 1, null);
            ScopedSymbolTable child = new ScopedSymbolTable("", 2, parent);

            parent.InsertVariable(new Symbol("a", null));
            Assert.Null(child.LookupVariable("a"));
        }
    }

}