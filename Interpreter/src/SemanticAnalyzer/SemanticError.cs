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

        public SemanticError(ErrorCode code, string message = "") : base(message) {
            Code = code;
        }
    }

}