using System;
using System.Collections.Generic;

namespace Glfw
{
    /// <summary>
    /// Represents an instance of a GLFW3 Window.
    /// </summary>
    public class Window
        : IDisposable
    {
        internal readonly IntPtr handle;

        private bool isDisposed;

        /// <summary>
        /// Creates a window and its associated OpenGL or OpenGL ES context.
        /// </summary>
        /// <param name="width">
        /// The desired width, in screen coordinates, of the window. This must
        /// be greater than zero.
        /// </param>
        /// <param name="height">
        /// The desired height, in screen coordinates, of the window. This must
        /// be greater than zero.
        /// </param>
        /// <param name="title">
        /// The initial window title.
        /// </param>
        /// <param name="windowHints">
        /// A dictionary of hints to set before creating the window.
        /// </param>
        public Window(int width, int height, string title, Dictionary<WindowAttribute, int> windowHints = null)
            : this(width, height, title, MonitorHandle.Zero, windowHints)
        {
        }

        internal Window(int width, int height, string title, MonitorHandle monitor, Dictionary<WindowAttribute, int> windowHints)
        {
            try
            {
                ErrorUtility.Bind();

                if (windowHints != null)
                {
                    foreach (var hintPair in windowHints)
                    {
                        Glfw3.WindowHint(hintPair.Key, hintPair.Value);
                    }
                }

                this.handle = Glfw3.CreateWindow(width, height, title, monitor, IntPtr.Zero);

                ErrorUtility.ThrowOnError();
            }
            finally
            {
                ErrorUtility.Unbind();
            }
        }

        public InputAction GetKeyState(Key key)
        {
            try
            {
                ErrorUtility.Bind();

                InputAction result = Glfw3.GetKey(this.handle, key);

                ErrorUtility.ThrowOnError();

                return result;
            }
            finally
            {
                ErrorUtility.Unbind();
            }
        }

        public InputAction GetMouseButtonState(MouseButton mouseButton)
        {
            try
            {
                ErrorUtility.Bind();

                InputAction result = Glfw3.GetMouseButton(this.handle, mouseButton);

                ErrorUtility.ThrowOnError();

                return result;
            }
            finally
            {
                ErrorUtility.Unbind();
            }
        }
        
        /// <summary>
        /// Returns the value of the close flag for this window.
        /// </summary>
        public bool ShouldClose
        {
            get
            {
                this.ThrowOnDisposed();

                return Glfw3.WindowShouldClose(this.handle);
            }
        }

        private void ThrowOnDisposed()
        {
            if (this.isDisposed)
            {
                throw new ObjectDisposedException(this.ToString());
            }
        }

        /// <summary>
        /// Closes the window.
        /// </summary>
        public void Close()
        {
            if (!this.isDisposed)
            {
                try
                {
                    ErrorUtility.Bind();

                    Glfw3.DestroyWindow(this.handle);

                    this.isDisposed = true;

                    ErrorUtility.ThrowOnError();
                }
                finally
                {
                    ErrorUtility.Unbind();
                }
            }
        }

        /// <summary>
        /// Releases all unmanaged resources associated with this window.
        /// </summary>
        public void Dispose()
        {
            this.Close();
        }

        public IntPtr Handle => this.handle;
    }
}
