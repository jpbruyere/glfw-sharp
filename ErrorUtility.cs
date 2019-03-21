using System;

namespace Glfw
{
    internal static class ErrorUtility
    {
        [ThreadStatic]
        private static ErrorCode? code;

        [ThreadStatic]
        private static string description;

        [ThreadStatic]
        private static ErrorDelegate wrappedDelegate;

        [ThreadStatic]
        private static ErrorDelegate callback;

        public static ErrorCode? Code
        {
            get => code;
            private set => code = value;
        }

        public static string Description
        {
            get => description;
            private set => description = value;
        }

        public static void Bind()
        {
            code = null;
            description = null;

            if (callback == null)
            {
                callback = (errorCode, errorDescription) =>
                {
                    code = errorCode;
                    description = errorDescription;

                    wrappedDelegate?.Invoke(errorCode, errorDescription);
                };
            }

            wrappedDelegate = Glfw3.SetErrorCallback(callback);
        }

        public static void ThrowOnError()
        {
            if (Code.HasValue)
            {
                throw new GlfwErrorException(Code.Value, Description);
            }
        }

        public static void Unbind()
        {
            Glfw3.SetErrorCallback(wrappedDelegate);
        }
    }
}
