using NUnit.Framework;

namespace Rewired.Interpreter.Tests {

    [TestFixture]
    public class TestScopedSymbolTable {

        [Test]
        public void Test_InitBuiltInTypes() {
            ScopedSymbolTable scope = new ScopedSymbolTable("", 1, null);
            scope.InitBuiltInTypes();
            Assert.NotNull(scope.Lookup("INTEGER"));
            Assert.NotNull(scope.Lookup("REAL"));
        }

        [Test]
        public void Lookup_ParentScope() {
            ScopedSymbolTable parent = new ScopedSymbolTable("", 1, null);
            ScopedSymbolTable child = new ScopedSymbolTable("", 2, parent);

            parent.Insert(new Symbol("test", null));
            Assert.NotNull(child.Lookup("test"));
        }

        [Test]
        public void Lookup_ParentNull() {
            ScopedSymbolTable scope = new ScopedSymbolTable("", 2, null);
            Assert.Null(scope.Lookup("test"));
        }
    }

}