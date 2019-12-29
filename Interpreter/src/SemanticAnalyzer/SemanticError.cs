using System;

namespace Rewired.Interpreter {

    public class SemanticError : Exception {

        public enum ErrorCode {
            IdNotFound,
            DuplicateId,
            TypeMismatch,
        }

        /// <summary>
        /// Gets the error code.
        /// </summary>
        public ErrorCode Code { get; }

        /// <summary>
        /// Gets the token at which the error occurred.
        /// </summary>
        public Token Token { get; }

        public SemanticError(ErrorCode code, Token token, string message = "") : base(message) {
            Code = code;
            Token = token;
        }
    }

}