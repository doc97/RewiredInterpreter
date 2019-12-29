using System;

namespace Rewired.Interpreter {

    public class TokenizerError : Exception {
        public TokenizerError(string message = "") : base(message) { }
    }

}