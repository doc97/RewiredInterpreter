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

        public ParserError(ErrorCode code, string message = "") : base(message) {
            Code = code;
        }
    }
}