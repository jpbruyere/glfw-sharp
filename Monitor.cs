using System.Collections.Generic;
using System.Linq;

namespace Glfw
{

    /// <summary>
    /// Represents an instance of a GLFW3 monitor.
    /// </summary>
    public class Monitor
    {
        internal readonly MonitorHandle handle;

        /// <summary>
        /// Creates a new instance of Monitor from a given monitor handle.
        /// </summary>
        /// <param name="handle">
        /// The monitor handle to use.
        /// </param>
        public Monitor(MonitorHandle handle)
        {
            this.handle = handle;
        }

        /// <summary>
        /// Gets a human-readable name for the monitor. The name typically
        /// reflects the make and model of the monitor and is not guaranteed
        /// to be unique among the connected monitors.
        /// </summary>
        public string Name => Glfw3.GetMonitorName(this.handle).Value;

        /// <summary>
        /// Gets the size, in millimetres, of the display area of the monitor.
        /// </summary>
        public (int Width, int Height) PhysicalSize
        {
            get
            {
                Glfw3.GetMonitorPhysicalSize(this.handle, out int width, out int height);

                return (width, height);
            }
        }

        /// <summary>
        /// Gets the position, in screen coordinates, of the upper-left corner
        /// of the specified monitor.
        /// </summary>
        public (int X, int Y) Position
        {
            get
            {
                Glfw3.GetMonitorPos(this.handle, out int x, out int y);

                return (x, y);
            }
        }

        /// <summary>
        /// Gets the current video mode of the specified monitor. If you have
        /// created a full screen window for that monitor, the return value
        /// will depend on whether that window is iconified.
        /// </summary>
        public VideoMode CurrentVideoMode => Glfw3.GetVideoMode(this.handle).Value;

        /// <summary>
        /// Returns an array of all video modes supported by the specified
        /// monitor. The returned array is sorted in ascending order, first by
        /// color bit depth (the sum of all channel depths) and then by
        /// resolution area (the product of width and height).
        /// </summary>
        /// <returns>
        /// An array of video modes.
        /// </returns>
        public VideoMode[] GetVideoModes()
        {
            try
            {
                ErrorUtility.Bind();

                var result = Glfw3.GetVideoModes(this.handle);

                ErrorUtility.ThrowOnError();

                return result;
            }
            finally
            {
                ErrorUtility.Unbind();
            }
        }

        /// <summary>
        /// Generates a 256-element gamma ramp from the specified exponent and
        /// then calls glfwSetGammaRamp with it. The value must be a finite
        /// number greater than zero.
        /// </summary>
        /// <param name="gamma">
        /// The desired exponent.
        /// </param>
        public void SetGamma(float gamma) => Glfw3.SetGamma(this.handle, gamma);

        /// <summary>
        /// Creates a fullscreen window and its associated OpenGL or OpenGL ES
        /// context.
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
        /// <returns>
        /// A new fullscreen window instance.
        /// </returns>
        public Window CreateFullscreenWindow(int width, int height, string title, Dictionary<WindowAttribute, int> windowHints = null)
        {
            return new Window(width, height, title, this.handle, windowHints);
        }

        /// <summary>
        /// Returns the primary monitor. This is usually the monitor where
        /// elements like the task bar or global menu bar are located.
        /// </summary>
        /// <returns>
        /// The primary monitor.
        /// </returns>
        public static Monitor GetPrimary()
        {
            try
            {
                ErrorUtility.Bind();

                var handle = Glfw3.GetPrimaryMonitor();

                ErrorUtility.ThrowOnError();

                return new Monitor(handle);
            }
            finally
            {
                ErrorUtility.Unbind();
            }
        }

        /// <summary>
        /// Returns an array of handles for all currently connected monitors.
        /// The primary monitor is always first in the returned array.
        /// </summary>
        /// <returns>
        /// An enumeration of monitors.
        /// </returns>
        public static Monitor[] GetAll()
        {
            try
            {
                ErrorUtility.Bind();

                var monitors = Glfw3.GetMonitors()
                                    .Select(x => new Monitor(x))
                                    .ToArray();

                ErrorUtility.ThrowOnError();

                return monitors;
            }
            finally
            {
                ErrorUtility.Unbind();
            }
        }
    }
}
