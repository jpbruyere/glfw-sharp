using System;

namespace Glfw
{
    /// <summary>
    /// Represents errors reported by GLFW.
    /// </summary>
    public class GlfwErrorException
        : Exception
    {
        /// <summary>
        /// Creates a new instance of the GlfwErrorsException class.
        /// </summary>
        /// <param name="code">
        /// The category of the error.
        /// </param>
        /// <param name="message">
        /// The error description.
        /// </param>
        public GlfwErrorException(ErrorCode code, string message)
            : base(message)
        {
            this.Code = code;
        }

        /// <summary>
        /// The category of the error.
        /// </summary>
        public ErrorCode Code
        {
            get;
        }
    }
}
