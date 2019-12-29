using System;

namespace Rewired.Interpreter {

    public class ParserError : Exception {

        public enum ErrorCode {
            UnexpectedToken
        }

        /// <summary>
        /// Gets the error code.
        /// </summary>
        public ErrorCode Code { get; }

        /// <summary>
        /// Gets the erronous token.
        /// </summary>
        public Token Token { get; }

        public ParserError(ErrorCode code, Token token, string message = "") : base(message) {
            Code = code;
            Token = token;
        }
    }
}