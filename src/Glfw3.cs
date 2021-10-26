// Copyright (c) 2019 Andrew Armstrong/FacticiusVir
// Copyright (c) 2019 Jean-Philippe Bruyère <jp_bruyere@hotmail.com>
//
// This code is licensed under the MIT license (MIT) (http://opensource.org/licenses/MIT)
using System;
using System.Runtime.InteropServices;
using System.Security;

using GLFWwindow = System.IntPtr;
using GLFWmonitor = System.IntPtr;
using GLFWgammaramp = System.IntPtr;
using System.Reflection;

namespace Glfw {
	public enum CursorShape
	{
		Arrow		= 0x00036001,
		IBeam		= 0x00036002,
		Crosshair	= 0x00036003,
		Hand		= 0x00036004,
		HResize		= 0x00036005,
		VResize		= 0x00036006,
        NWSEResize  = 0x00036007,
        NESWResize  = 0x00036008,
        ResizeAll   = 0x00036009,
        NotAllowed  = 0x0003600A
    }
    public enum JoystickEvent {
		Connected 		= 0x00040001,
		Disconnected 	= 0x00040002
	}
	public struct GamepadStatePtr {
		readonly unsafe byte* handle;
		public InputAction[] Buttons {
			get {
				unsafe {
					return new Span<InputAction> (handle, 15 * sizeof(int)).ToArray ();
				}
			}
		}
		public float[] axes {
			get {
				unsafe {
					return new Span<float> (handle + sizeof(int) * 15, 6).ToArray ();
				}
			}
		}
	}

	/// <summary>
	/// Interop functions for the GLFW3 API.
	/// </summary>
	public static class Glfw3
    {
		static IntPtr resolveUnmanaged(Assembly assembly, String libraryName)
		{

			switch (libraryName)
			{
				case "glfw3":
					return NativeLibrary.Load("glfw", assembly, null);
			}
			Console.WriteLine($"[UNRESOLVE] {assembly} {libraryName}");
			return IntPtr.Zero;
		}
        static Glfw3 () {
            System.Runtime.Loader.AssemblyLoadContext.GetLoadContext(
                Assembly.GetExecutingAssembly()).ResolvingUnmanagedDll += resolveUnmanaged;
        }
		/// <summary>
		/// The base name for the GLFW3 library.
		/// </summary>
		public const string GlfwDll = "glfw3";

        /// <summary>
        /// Initializes the GLFW library.
        /// </summary>
        /// <returns>
        /// True if successful, otherwise false.
        /// </returns>
        [DllImport(GlfwDll, CallingConvention = CallingConvention.Cdecl, EntryPoint = "glfwInit")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool Init();

        /// <summary>
        /// This function destroys all remaining windows and cursors, restores
        /// any modified gamma ramps and frees any other allocated resources.
        /// </summary>
        [DllImport(GlfwDll, CallingConvention = CallingConvention.Cdecl, EntryPoint = "glfwTerminate")]
        public static extern void Terminate();

        /// <summary>
        /// This function retrieves the major, minor and revision numbers of
        /// the GLFW library.
        /// </summary>
        /// <param name="major">
        /// The major version number.
        /// </param>
        /// <param name="minor">
        /// The minor version number.
        /// </param>
        /// <param name="rev">
        /// The revision number.
        /// </param>
        [DllImport(GlfwDll, CallingConvention = CallingConvention.Cdecl, EntryPoint = "glfwGetVersion")]
        public static extern void GetVersion(out int major, out int minor, out int rev);


        /// <summary>
        /// Returns the compile-time generated version string of the GLFW
        /// library binary. It describes the version, platform, compiler and
        /// any platform-specific compile-time options.
        /// </summary>
        /// <returns>
        /// The compile-time generated version string of the GLFW library
        /// binary.
        /// </returns>
        [DllImport(GlfwDll, CallingConvention = CallingConvention.Cdecl, EntryPoint = "glfwGetVersionString")]
        static unsafe extern NativeUtf8String GetVersionString();
		public static string VersionString {
			get => GetVersionString ().ToString ();
		}


		/// <summary>
		/// Creates a window and its associated OpenGL or OpenGL ES context.
		/// Most of the options controlling how the window and its context
		/// should be created are specified with window hints.
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
		/// <param name="monitor">
		/// The monitor to use for full screen mode, or Null for windowed mode.
		/// </param>
		/// <param name="share">
		/// The window whose context to share resources with, or Null to not share resources.
		/// </param>
		/// <returns>
		/// The handle of the created window, or Null if an error occurred.
		/// </returns>
		[DllImport(GlfwDll, CallingConvention = CallingConvention.Cdecl, EntryPoint = "glfwCreateWindow")]
        public static extern GLFWwindow CreateWindow(int width, int height, [MarshalAs(UnmanagedType.LPStr)] string title, MonitorHandle monitor, IntPtr share);

        /// <summary>
        /// Destroys the specified window and its context. On calling this
        /// function, no further callbacks will be called for that window.
        /// </summary>
        /// <param name="window">
        /// The window to destroy.
        /// </param>
        [DllImport(GlfwDll, CallingConvention = CallingConvention.Cdecl, EntryPoint = "glfwDestroyWindow")]
        public static extern void DestroyWindow(GLFWwindow window);

        /// <summary>
        /// Processes events in the event queue and then returns immediately.
        /// Processing events will cause the window and input callbacks
        /// associated with those events to be called.
        /// </summary>
        [DllImport(GlfwDll, CallingConvention = CallingConvention.Cdecl, EntryPoint = "glfwPollEvents")]
        public static extern void PollEvents();

        /// <summary>
        /// Sets hints for the next call to CreateWindow. The hints, once set,
        /// retain their values until changed by a call to WindowHint or
        /// DefaultWindowHints, or until the library is terminated.
        /// </summary>
        /// <param name="hint">
        /// The window hint to set.
        /// </param>
        /// <param name="value">
        /// The new value of the window hint.
        /// </param>
        [DllImport(GlfwDll, CallingConvention = CallingConvention.Cdecl, EntryPoint = "glfwWindowHint")]
        public static extern void WindowHint(WindowAttribute hint, int value);

		[DllImport (GlfwDll, CallingConvention = CallingConvention.Cdecl, EntryPoint = "glfwGetWindowAttrib")]
		public static extern int GetWindowAttrib (GLFWwindow window, WindowAttribute attribute);
		[DllImport (GlfwDll, CallingConvention = CallingConvention.Cdecl, EntryPoint = "glfwSetWindowAttrib")]
		public static extern void SetWindowAttrib (GLFWwindow window, WindowAttribute attribute, int value);


		[DllImport (GlfwDll, CallingConvention = CallingConvention.Cdecl, EntryPoint = "glfwGetWindowPos")]
		public static extern void GetWindowPos (GLFWwindow window, out int x, out int y);
		[DllImport (GlfwDll, CallingConvention = CallingConvention.Cdecl, EntryPoint = "glfwSetWindowPos")]
		public static extern void SetWindowPos (GLFWwindow window, int x, int y);

		[DllImport (GlfwDll, CallingConvention = CallingConvention.Cdecl, EntryPoint = "glfwGetWindowSize")]
		public static extern void GetWindowSize (GLFWwindow window, out int width, out int height);
		[DllImport (GlfwDll, CallingConvention = CallingConvention.Cdecl, EntryPoint = "glfwSetWindowSize")]
		public static extern void SetWindowSize (GLFWwindow window, int width, int height);

		[DllImport (GlfwDll, CallingConvention = CallingConvention.Cdecl, EntryPoint = "glfwGetWindowOpacity")]
		public static extern float GetWindowOpacity (GLFWwindow window);
		[DllImport (GlfwDll, CallingConvention = CallingConvention.Cdecl, EntryPoint = "glfwSetWindowOpacity")]
		public static extern void SetWindowOpacity (GLFWwindow window, float opacity);
		[DllImport (GlfwDll, CallingConvention = CallingConvention.Cdecl, EntryPoint = "glfwIconifyWindow")]
		public static extern void IconifyWindow (GLFWwindow window);
		[DllImport (GlfwDll, CallingConvention = CallingConvention.Cdecl, EntryPoint = "glfwRestoreWindow")]
		public static extern void RestoreWindow (GLFWwindow window);
		[DllImport (GlfwDll, CallingConvention = CallingConvention.Cdecl, EntryPoint = "glfwMaximizeWindow")]
		public static extern void MaximizeWindow (GLFWwindow window);
		[DllImport (GlfwDll, CallingConvention = CallingConvention.Cdecl, EntryPoint = "glfwShowWindow")]
		public static extern void ShowWindow (GLFWwindow window);
		[DllImport (GlfwDll, CallingConvention = CallingConvention.Cdecl, EntryPoint = "glfwHideWindow")]
		public static extern void HideWindow (GLFWwindow window);


		/// <summary>
		/// Returns the value of the close flag of the specified window.
		/// </summary>
		/// <param name="window">
		/// The window to query.
		/// </param>
		/// <returns>
		/// The value of the close flag.
		/// </returns>
		[DllImport (GlfwDll, CallingConvention = CallingConvention.Cdecl, EntryPoint = "glfwWindowShouldClose")]
        public static extern bool WindowShouldClose(GLFWwindow window);

        [DllImport (GlfwDll, CallingConvention = CallingConvention.Cdecl, EntryPoint = "glfwSetWindowShouldClose")]
        public static extern void SetWindowShouldClose (GLFWwindow window, int value);

        [DllImport (GlfwDll, CallingConvention = CallingConvention.Cdecl, EntryPoint = "glfwSetWindowTitle")]
		static extern void SetWindowTitle (GLFWwindow window, ref byte utf8);
		public static void SetWindowTitle (GLFWwindow window, string title)
			=> SetWindowTitle (window, ref MemoryMarshal.GetReference (new Span<byte> (System.Text.Encoding.UTF8.GetBytes (title + "\0"))));
		/// <summary>
		/// Creates a Vulkan surface for the specified window.
		/// </summary>
		/// <param name="instance">
		/// The Vulkan instance to create the surface in.
		/// </param>
		/// <param name="window">
		/// The window to create the surface for.
		/// </param>
		/// <param name="pAllocator">
		/// The allocator to use, or NULL to use the default allocator.
		/// </param>
		/// <param name="surface">
		/// Where to store the handle of the surface. This is set to
		/// VK_NULL_HANDLE if an error occurred.
		/// </param>
		/// <returns>
		/// Result.Success if successful, or a Vulkan error code if an error
		/// occurred.
		/// </returns>
		[DllImport (GlfwDll, CallingConvention = CallingConvention.Cdecl, EntryPoint = "glfwCreateWindowSurface")]
        public static extern int CreateWindowSurface(IntPtr instance, IntPtr window, IntPtr pAllocator, out ulong surface);

        /// <summary>
        /// Returns an array of names of Vulkan instance extensions required by
        /// GLFW for creating Vulkan surfaces for GLFW windows. If successful,
        /// the list will always contains VK_KHR_surface, so if you don't
        /// require any additional extensions you can pass this list directly
        /// to the InstanceCreateInfo struct.
        /// </summary>
        /// <param name="count">
        /// Where to store the number of extensions in the returned array. This
        /// is set to zero if an error occurred.
        /// </param>
        /// <returns>
        /// An array of extension names, or Null if an error occurred.
        /// </returns>
        [DllImport(GlfwDll, CallingConvention = CallingConvention.Cdecl, EntryPoint = "glfwGetRequiredInstanceExtensions")]
        public static extern IntPtr GetRequiredInstanceExtensions(out int count);

        /// <summary>
        /// Sets the size callback of the specified window, which is called
        /// when the window is resized. The callback is provided with the size,
        /// in screen coordinates, of the client area of the window.
        /// </summary>
        /// <param name="window">
        /// The window whose callback to set.
        /// </param>
        /// <param name="callback">
        /// The new callback, or Null to remove the currently set callback.
        /// </param>
        /// <returns></returns>
        [DllImport(GlfwDll, CallingConvention = CallingConvention.Cdecl, EntryPoint = "glfwSetWindowSizeCallback")]
        public static extern WindowSizeDelegate SetWindowSizeCallback(GLFWwindow window, WindowSizeDelegate callback);

        /// <summary>
        /// Sets the error callback, which is called with an error code and a
        /// human-readable description each time a GLFW error occurs.
        /// </summary>
        /// <param name="callback">
        /// The new callback, or Null to remove the currently set callback.
        /// </param>
        /// <returns>
        /// The previously set callback, or Null if no callback was set.
        /// </returns>
        [DllImport(GlfwDll, CallingConvention = CallingConvention.Cdecl, EntryPoint = "glfwSetErrorCallback")]
        public static extern ErrorDelegate SetErrorCallback(ErrorDelegate callback);

		[DllImport (GlfwDll, CallingConvention = CallingConvention.Cdecl, EntryPoint = "glfwSetWindowRefreshCallback")]
		public static extern WindowDelegate SetWindowRefreshCallback (GLFWwindow window, WindowDelegate callback);


		/// <summary>
		/// Returns an array of handles for all currently connected monitors.
		/// The primary monitor is always first in the returned array. If no
		/// monitors were found, this function returns Null.
		/// </summary>
		/// <param name="count">
		/// Where to store the number of monitors in the returned array. This
		/// is set to zero if an error occurred.
		/// </param>
		/// <returns>
		/// An array of monitor handles, or Null if no monitors were found or
		/// if an error occurred.
		/// </returns>
		[DllImport(GlfwDll, CallingConvention = CallingConvention.Cdecl, EntryPoint = "glfwGetMonitors")]
        public static extern IntPtr GetMonitors(out int count);

        /// <summary>
        /// Returns the primary monitor. This is usually the monitor where
        /// elements like the task bar or global menu bar are located.
        /// </summary>
        /// <returns>
        /// The primary monitor, or Null if no monitors were found or if an
        /// error occurred.
        /// </returns>
        [DllImport(GlfwDll, CallingConvention = CallingConvention.Cdecl, EntryPoint = "glfwGetPrimaryMonitor")]
        public static extern MonitorHandle GetPrimaryMonitor();

        /// <summary>
        /// Returns the position, in screen coordinates, of the upper-left
        /// corner of the specified monitor.
        /// </summary>
        /// <param name="monitor">
        /// The monitor to query.
        /// </param>
        /// <param name="xPos">
        /// Returns the monitor x-coordinate.
        /// </param>
        /// <param name="yPos">
        /// Returns the monitor y-coordinate.
        /// </param>
        [DllImport(GlfwDll, CallingConvention = CallingConvention.Cdecl, EntryPoint = "glfwGetMonitorPos")]
        public static extern void GetMonitorPos(MonitorHandle monitor, out int xPos, out int yPos);

        /// <summary>
        /// Returns the size, in millimetres, of the display area of the
        /// specified monitor.
        /// </summary>
        /// <param name="monitor">
        /// The monitor to query.
        /// </param>
        /// <param name="widthMm">
        /// The width, in millimetres, of the monitor's display area.
        /// </param>
        /// <param name="heightMm">
        /// The width, in millimetres, of the monitor's display area.
        /// </param>
        [DllImport(GlfwDll, CallingConvention = CallingConvention.Cdecl, EntryPoint = "glfwGetMonitorPhysicalSize")]
        public static extern void GetMonitorPhysicalSize(MonitorHandle monitor, out int widthMm, out int heightMm);

        /// <summary>
        /// Returns a human-readable name, of the specified monitor. The name
        /// typically reflects the make and model of the monitor and is not
        /// guaranteed to be unique among the connected monitors.
        /// </summary>
        /// <param name="monitor">
        /// The monitor to query.
        /// </param>
        /// <returns>
        /// The name of the monitor, or Null if an error occurred.
        /// </returns>
        [DllImport(GlfwDll, CallingConvention = CallingConvention.Cdecl, EntryPoint = "glfwGetMonitorName")]
        public static extern NativeString GetMonitorName(MonitorHandle monitor);

        /// <summary>
        /// Sets the monitor configuration callback, or removes the currently
        /// set callback. This is called when a monitor is connected to or
        /// disconnected from the system.
        /// </summary>
        /// <param name="callback">
        /// The new callback, or Null to remove the currently set callback.
        /// </param>
        /// <returns>
        /// The previously set callback, or NULL if no callback was set or the
        /// library had not been initialized.
        /// </returns>
        [DllImport(GlfwDll, CallingConvention = CallingConvention.Cdecl, EntryPoint = "glfwSetMonitorCallback")]
        public static extern MonitorEventDelegate SetMonitorCallback(MonitorEventDelegate callback);

        /// <summary>
        /// Returns an array of all video modes supported by the specified
        /// monitor. The returned array is sorted in ascending order, first by
        /// color bit depth (the sum of all channel depths) and then by
        /// resolution area (the product of width and height).
        /// </summary>
        /// <param name="monitor">
        /// The monitor to query.
        /// </param>
        /// <param name="count">
        /// Tthe number of video modes in the returned array. This is set to
        /// zero if an error occurred.
        /// </param>
        /// <returns>
        /// An array of video modes, or Null if an error occurred.
        /// </returns>
        [DllImport(GlfwDll, CallingConvention = CallingConvention.Cdecl, EntryPoint = "glfwGetVideoModes")]
        public static extern IntPtr GetVideoModes(MonitorHandle monitor, out int count);

        /// <summary>
        /// Returns the current video mode of the specified monitor. If you
        /// have created a full screen window for that monitor, the return
        /// value will depend on whether that window is iconified.
        /// </summary>
        /// <param name="monitor">
        /// The monitor to query.
        /// </param>
        /// <returns>
        /// A wrapped pointer to the current mode of the monitor, or Null if
        /// an error occurred.
        /// </returns>
        [DllImport(GlfwDll, CallingConvention = CallingConvention.Cdecl, EntryPoint = "glfwGetVideoMode")]
        public static extern VideoModePointer GetVideoMode(MonitorHandle monitor);

        /// <summary>
        /// Generates a 256-element gamma ramp from the specified exponent and
        /// then calls glfwSetGammaRamp with it. The value must be a finite
        /// number greater than zero.
        /// </summary>
        /// <param name="monitor">
        /// The monitor whose gamma ramp to set.
        /// </param>
        /// <param name="gamma">
        /// The desired exponent.
        /// </param>
        [DllImport(GlfwDll, CallingConvention = CallingConvention.Cdecl, EntryPoint = "glfwSetGamma")]
        public static extern void SetGamma(MonitorHandle monitor, float gamma);

        [DllImport(GlfwDll, CallingConvention = CallingConvention.Cdecl, EntryPoint = "glfwGetInputMode")]
        public static extern int GetInputMode(GLFWwindow window, InputMode mode);

        [DllImport(GlfwDll, CallingConvention = CallingConvention.Cdecl, EntryPoint = "glfwSetInputMode")]
        public static extern void SetInputMode(GLFWwindow window, InputMode mode, int value);

        /// <summary>
        /// Returns the localized name of the specified printable key. This is
        /// intended for displaying key bindings to the user.
        /// </summary>
        /// <param name="key">
        /// The key to query, or Key.Unknown.
        /// </param>
        /// <param name="scancode">
        /// The scancode of the key to query, if key is Key.Unknown.
        /// </param>
        /// <returns>
        /// The localized name of the key, or Null.
        /// </returns>
        [DllImport(GlfwDll, CallingConvention = CallingConvention.Cdecl, EntryPoint = "glfwGetKeyName")]
        public static extern NativeUtf8String GetKeyName(Key key, int scancode);

        [DllImport(GlfwDll, CallingConvention = CallingConvention.Cdecl, EntryPoint = "glfwGetKey")]
        public static extern InputAction GetKey(GLFWwindow window, Key key);

        [DllImport(GlfwDll, CallingConvention = CallingConvention.Cdecl, EntryPoint = "glfwGetMouseButton")]
        public static extern InputAction GetMouseButton(GLFWwindow window, MouseButton button);

        [DllImport(GlfwDll, CallingConvention = CallingConvention.Cdecl, EntryPoint = "glfwGetCursorPos")]
        public static extern void GetCursorPosition(GLFWwindow window, out double xPosition, out double yPosition);

        [DllImport(GlfwDll, CallingConvention = CallingConvention.Cdecl, EntryPoint = "glfwSetCursorPos")]
        public static extern void SetCursorPosition(GLFWwindow window, double xPosition, double yPosition);

        [DllImport(GlfwDll, CallingConvention = CallingConvention.Cdecl, EntryPoint = "glfwSetKeyCallback")]
        public static extern KeyDelegate SetKeyCallback(GLFWwindow window, KeyDelegate callback);

        [DllImport(GlfwDll, CallingConvention = CallingConvention.Cdecl, EntryPoint = "glfwSetCharCallback")]
        public static extern KeyDelegate SetCharCallback(GLFWwindow window, CharDelegate callback);

        /// <summary>
        /// <para>Sets a callback for Mouse movement events. Use this for full
        /// mouse path resolution between PollEvents() calls.</para>
        /// <para>From GLFW Documentation: The callback functions receives the
        /// cursor position, measured in screen coordinates but relative to the
        /// top-left corner of the window client area. On platforms that
        /// provide it, the full sub-pixel cursor position is passed on.</para>
        /// </summary>
        /// <returns>
        /// The previously set callback, or NULL if no callback was set or the
        /// library had not been initialized.
        /// </returns>
        [DllImport(GlfwDll, CallingConvention = CallingConvention.Cdecl, EntryPoint = "glfwSetCursorPosCallback")]
        public static extern CursorPosDelegate SetCursorPosCallback(GLFWwindow window, CursorPosDelegate callback);

        /// <summary>
        /// <para>Sets a Callback for Button Events (i.e. clicks). This also
        /// detects mouse press and release events done between PollEvents()
        /// calls.</para>
        /// <para>From GLFW Documentation: Whenever you poll state, you risk
        /// missing the state change you are looking for. If a pressed mouse
        /// button is released again before you poll its state, you will have
        /// missed the button press. The recommended solution for this is to
        /// use a mouse button callback, but there is also the
        /// GLFW_STICKY_MOUSE_BUTTONS input mode.</para>
        /// </summary>
        /// <returns>
        /// The previously set callback, or NULL if no callback was set or the
        /// library had not been initialized.
        /// </returns>
        [DllImport(GlfwDll, CallingConvention = CallingConvention.Cdecl, EntryPoint = "glfwSetMouseButtonCallback")]
        public static extern MouseButtonDelegate SetMouseButtonPosCallback(GLFWwindow window, MouseButtonDelegate callback);

        /// <summary>
        /// Sets a Callback for Mouse Scrolling Events. (i.e. scroll wheel)
        /// There is no polling support for this, so if youre interested in the wheel, you have to set this callback
        /// NOTE: your normal desktop mouse variant likely only reports Y-Coordinate
        /// </summary>
        /// <returns>
        /// The previously set callback, or NULL if no callback was set or the
        /// library had not been initialized.
        /// </returns>
        [DllImport(GlfwDll, CallingConvention = CallingConvention.Cdecl, EntryPoint = "glfwSetScrollCallback")]
        public static extern ScrollDelegate SetScrollCallback(GLFWwindow window, ScrollDelegate callback);

        /// <summary>
        /// Returns an array of names of Vulkan instance extensions required by
        /// GLFW for creating Vulkan surfaces for GLFW windows. If successful,
        /// the list will always contains VK_KHR_surface, so if you don't
        /// require any additional extensions you can pass this list directly
        /// to the InstanceCreateInfo struct.
        /// </summary>
        /// <returns>
        /// An array of extension names, or Null if an error occurred.
        /// </returns>
        public static string[] GetRequiredInstanceExtensions()
        {
            IntPtr names = GetRequiredInstanceExtensions(out int count);

            string[] result = new string[count];

            for (int nameIndex = 0; nameIndex < count; nameIndex++)
            {
				IntPtr name = Marshal.ReadIntPtr (names, nameIndex * Marshal.SizeOf<IntPtr>());
                result[nameIndex] = Marshal.PtrToStringAnsi(name);
            }

            return result;
        }

        /// <summary>
        /// Returns an array of handles for all currently connected monitors.
        /// The primary monitor is always first in the returned array. If no
        /// monitors were found, this function returns Null.
        /// </summary>
        /// <returns>
        /// An array of monitor handles, or Null if no monitors were found or
        /// if an error occurred.
        /// </returns>
        public static MonitorHandle[] GetMonitors()
        {
            IntPtr monitors = GetMonitors(out int count);

            var result = new MonitorHandle[count];

            for (int i = 0; i < count; i++)
                result[i] = new MonitorHandle(Marshal.ReadIntPtr(monitors, i));

            return result;
        }

        /// <summary>
        /// Returns an array of all video modes supported by the specified
        /// monitor. The returned array is sorted in ascending order, first by
        /// color bit depth (the sum of all channel depths) and then by
        /// resolution area (the product of width and height).
        /// </summary>
        /// <param name="monitor">
        /// The monitor to query.
        /// </param>
        /// <returns>
        /// An array of video modes, or Null if an error occurred.
        /// </returns>
        public static VideoMode[] GetVideoModes(MonitorHandle monitor)
        {
            IntPtr videoModes = GetVideoModes(monitor, out int count);

            var result = new VideoMode[count];

            for (int i = 0; i < count; i++)
                result[i] = Marshal.PtrToStructure<VideoMode>(Marshal.ReadIntPtr(videoModes, i));

            return result;
        }

        /// <summary>
        /// This function retrieves the version number of the GLFW library.
        /// </summary>
        /// <returns>
        /// The version number of the GLFW library.
        /// </returns>
        public static Version GetVersion()
        {
            GetVersion(out int major, out int minor, out int revision);

            return new Version(major, minor, revision);
        }

		[DllImport (GlfwDll, CallingConvention = CallingConvention.Cdecl, EntryPoint = "glfwCreateStandardCursor")]
		public static extern IntPtr CreateStandardCursor (CursorShape shape);

		[DllImport (GlfwDll, CallingConvention = CallingConvention.Cdecl, EntryPoint = "glfwCreateCursor")]
		public static extern IntPtr CreateCursor (IntPtr image, int xhot, int yhot);

		[DllImport (GlfwDll, CallingConvention = CallingConvention.Cdecl, EntryPoint = "glfwDestroyCursor")]
		public static extern void DestroyCursor (IntPtr cursor);

		[DllImport (GlfwDll, CallingConvention = CallingConvention.Cdecl, EntryPoint = "glfwSetCursor")]
		public static extern void SetCursor (GLFWwindow window, IntPtr cursor);

		[DllImport (GlfwDll, CallingConvention = CallingConvention.Cdecl, EntryPoint = "glfwSetClipboardString")]
		static extern void SetClipboardString (GLFWwindow window, ref byte utf8);
		public static void SetClipboardString (GLFWwindow window, string cbString)
			=> SetClipboardString (window, ref MemoryMarshal.GetReference (new Span<byte> (System.Text.Encoding.UTF8.GetBytes (cbString + "\0"))));



		[DllImport (GlfwDll, CallingConvention = CallingConvention.Cdecl, EntryPoint = "glfwGetClipboardString")]
		public static extern NativeUtf8String GetClipboardString (GLFWwindow window);

		#region platform native window handle

		[DllImport (GlfwDll, CallingConvention = CallingConvention.Cdecl, EntryPoint = "glfwInitHint")]
		public static extern void InitHint (int a, int b);


		[DllImport (GlfwDll, CallingConvention = CallingConvention.Cdecl, EntryPoint = "glfwGetError")]
		public static extern int GetError (out NativeUtf8String utf8String);




		[DllImport (GlfwDll, CallingConvention = CallingConvention.Cdecl, EntryPoint = "glfwGetMonitorWorkarea")]
		public static extern void GetMonitorWorkarea (GLFWmonitor monitor, out int x, out int y, out int width, out int height);


		[DllImport (GlfwDll, CallingConvention = CallingConvention.Cdecl, EntryPoint = "glfwGetMonitorContentScale")]
		public static extern void GetMonitorContentScale (GLFWmonitor monitor, out float xscale, out float yscale);


		[DllImport (GlfwDll, CallingConvention = CallingConvention.Cdecl, EntryPoint = "glfwSetMonitorUserPointer")]
		public static extern void SetMonitorUserPointer (GLFWmonitor monitor, IntPtr userPointer);

		[DllImport (GlfwDll, CallingConvention = CallingConvention.Cdecl, EntryPoint = "glfwGetMonitorUserPointer")]
		public static extern IntPtr GetMonitorUserPointer (GLFWmonitor monitor);





		[DllImport (GlfwDll, CallingConvention = CallingConvention.Cdecl, EntryPoint = "glfwGetGammaRamp")]
		public static extern GLFWgammaramp GetGammaRamp (GLFWmonitor monitor);

		[DllImport (GlfwDll, CallingConvention = CallingConvention.Cdecl, EntryPoint = "glfwSetGammaRamp")]
		public static extern void SetGammaRamp (GLFWmonitor monitor, GLFWgammaramp gammaramp);

		[DllImport (GlfwDll, CallingConvention = CallingConvention.Cdecl, EntryPoint = "glfwDefaultWindowHints")]
		public static extern void DefaultWindowHints ();


		[DllImport (GlfwDll, CallingConvention = CallingConvention.Cdecl, EntryPoint = "glfwWindowHintString")]
		public static extern void WindowHintString (WindowAttribute hint, ref byte value);

		[DllImport (GlfwDll, CallingConvention = CallingConvention.Cdecl, EntryPoint = "glfwSetWindowIcon")]
		public static extern void SetWindowIcon (GLFWwindow window, int count, ref Image image);

		[DllImport (GlfwDll, CallingConvention = CallingConvention.Cdecl, EntryPoint = "glfwSetWindowSizeLimits")]
		public static extern void SetWindowSizeLimits (GLFWwindow window, int minW, int minH, int maxW, int maxH);

		[DllImport (GlfwDll, CallingConvention = CallingConvention.Cdecl, EntryPoint = "glfwSetWindowAspectRatio")]
		public static extern void SetWindowAspectRatio (GLFWwindow window, int numerator, int denominator);

		[DllImport (GlfwDll, CallingConvention = CallingConvention.Cdecl, EntryPoint = "glfwGetFramebufferSize")]
		public static extern void GetFramebufferSize (GLFWwindow window, out int width, out int height);

		[DllImport (GlfwDll, CallingConvention = CallingConvention.Cdecl, EntryPoint = "glfwGetWindowFrameSize")]
		public static extern void GetWindowFrameSize (GLFWwindow window, out int left, out int top, out int width, out int height);

		[DllImport (GlfwDll, CallingConvention = CallingConvention.Cdecl, EntryPoint = "glfwGetWindowContentScale")]
		public static extern void GetWindowContentScale (GLFWwindow window, out float scalex, out float scaly);

		[DllImport (GlfwDll, CallingConvention = CallingConvention.Cdecl, EntryPoint = "glfwFocusWindow")]
		public static extern void FocusWindow (GLFWwindow window);

		[DllImport (GlfwDll, CallingConvention = CallingConvention.Cdecl, EntryPoint = "glfwRequestWindowAttention")]
		public static extern void RequestWindowAttention (GLFWwindow window);

		[DllImport (GlfwDll, CallingConvention = CallingConvention.Cdecl, EntryPoint = "glfwGetWindowMonitor")]
		public static extern GLFWmonitor GetWindowMonitor (GLFWwindow window);

		[DllImport (GlfwDll, CallingConvention = CallingConvention.Cdecl, EntryPoint = "glfwSetWindowMonitor")]
		public static extern void SetWindowMonitor (GLFWwindow window, GLFWmonitor monitor,int x, int y, int width, int height, int refreshRate);



		[DllImport (GlfwDll, CallingConvention = CallingConvention.Cdecl, EntryPoint = "glfwSetWindowUserPointer")]
		public static extern void SetWindowUserPointer (GLFWwindow window, IntPtr userPointer);

		[DllImport (GlfwDll, CallingConvention = CallingConvention.Cdecl, EntryPoint = "glfwGetWindowUserPointer")]
		public static extern IntPtr GetWindowUserPointer (GLFWwindow window);



		[DllImport (GlfwDll, CallingConvention = CallingConvention.Cdecl, EntryPoint = "glfwSetWindowCloseCallback")]
		public static extern WindowDelegate SetWindowCloseCallback (GLFWwindow window, WindowDelegate winDel);


		[DllImport (GlfwDll, CallingConvention = CallingConvention.Cdecl, EntryPoint = "glfwSetWindowFocusCallback")]
		public static extern WindowBooleanDelegate SetWindowFocusCallback (GLFWwindow window, WindowBooleanDelegate winDel);

		[DllImport (GlfwDll, CallingConvention = CallingConvention.Cdecl, EntryPoint = "glfwSetWindowIconifyCallback")]
		public static extern WindowBooleanDelegate SetWindowIconifyCallback (GLFWwindow window, WindowBooleanDelegate winDel);

		[DllImport (GlfwDll, CallingConvention = CallingConvention.Cdecl, EntryPoint = "glfwSetWindowMaximizeCallback")]
		public static extern WindowBooleanDelegate SetWindowMaximizeCallback (GLFWwindow window, WindowBooleanDelegate winDel);

		[DllImport (GlfwDll, CallingConvention = CallingConvention.Cdecl, EntryPoint = "glfwSetFramebufferSizeCallback")]
		public static extern WindowSizeDelegate SetFramebufferSizeCallback (GLFWwindow window, WindowSizeDelegate winDel);

		[DllImport (GlfwDll, CallingConvention = CallingConvention.Cdecl, EntryPoint = "glfwSetWindowContentScaleCallback")]
		public static extern WindowScaleDelegate SetWindowContentScaleCallback (GLFWwindow window, WindowScaleDelegate winDel);


		[DllImport (GlfwDll, CallingConvention = CallingConvention.Cdecl, EntryPoint = "glfwWaitEvents")]
		public static extern void WaitEvents ();

		[DllImport (GlfwDll, CallingConvention = CallingConvention.Cdecl, EntryPoint = "glfwWaitEventsTimeout")]
		public static extern void WaitEventsTimeout (double timeOut);

		[DllImport (GlfwDll, CallingConvention = CallingConvention.Cdecl, EntryPoint = "glfwPostEmptyEvent")]
		public static extern void PostEmptyEvent ();

		[DllImport (GlfwDll, CallingConvention = CallingConvention.Cdecl, EntryPoint = "glfwRawMouseMotionSupported")]
		public static extern bool RawMouseMotionSupported ();


		[DllImport (GlfwDll, CallingConvention = CallingConvention.Cdecl, EntryPoint = "glfwGetKeyScancode")]
		public static extern int GetKeyScancode (Key key);

		[DllImport (GlfwDll, CallingConvention = CallingConvention.Cdecl, EntryPoint = "glfwSetCharModsCallback")]
		public static extern CharModsDelegate SetCharModsCallback (GLFWwindow window, CharModsDelegate del);


		[DllImport (GlfwDll, CallingConvention = CallingConvention.Cdecl, EntryPoint = "glfwSetCursorEnterCallback")]
		public static extern WindowBooleanDelegate SetCursorEnterCallback (GLFWwindow window, WindowBooleanDelegate del);


		[DllImport (GlfwDll, CallingConvention = CallingConvention.Cdecl, EntryPoint = "glfwSetDropCallback")]
		public static extern WindowDropDelegate SetDropCallback (GLFWwindow window, WindowDropDelegate del);

		[DllImport (GlfwDll, CallingConvention = CallingConvention.Cdecl, EntryPoint = "glfwJoystickPresent")]
		public static extern bool JoystickPresent (int joystickID);

		[DllImport (GlfwDll, CallingConvention = CallingConvention.Cdecl, EntryPoint = "glfwGetJoystickAxes")]
		public static extern IntPtr GetJoystickAxes (int joystickID, out int count);

		[DllImport (GlfwDll, CallingConvention = CallingConvention.Cdecl, EntryPoint = "glfwGetJoystickButtons")]
		public static extern IntPtr GetJoystickButtons (int joystickID, out int count);

		[DllImport (GlfwDll, CallingConvention = CallingConvention.Cdecl, EntryPoint = "glfwGetJoystickHats")]
		public static extern IntPtr GetJoystickHats (int joystickID, out int count);

		[DllImport (GlfwDll, CallingConvention = CallingConvention.Cdecl, EntryPoint = "glfwGetJoystickName")]
		public static extern NativeUtf8String GetJoystickName (int joystickID);

		[DllImport (GlfwDll, CallingConvention = CallingConvention.Cdecl, EntryPoint = "glfwGetJoystickGUID")]
		public static extern NativeUtf8String GetJoystickGUID (int joystickID);

		[DllImport (GlfwDll, CallingConvention = CallingConvention.Cdecl, EntryPoint = "glfwSetJoystickUserPointer")]
		public static extern void SetJoystickUserPointer (int joystickID, IntPtr userPtr);

		[DllImport (GlfwDll, CallingConvention = CallingConvention.Cdecl, EntryPoint = "glfwGetJoystickUserPointer")]
		public static extern IntPtr GetJoystickUserPointer (int joystickID);

		[DllImport (GlfwDll, CallingConvention = CallingConvention.Cdecl, EntryPoint = "glfwJoystickIsGamepad")]
		public static extern int JoystickIsGamepad (int joystickID);

		[DllImport (GlfwDll, CallingConvention = CallingConvention.Cdecl, EntryPoint = "glfwSetJoystickCallback")]
		public static extern JoystickDelegate SetJoystickCallback (JoystickDelegate del);

		[DllImport (GlfwDll, CallingConvention = CallingConvention.Cdecl, EntryPoint = "glfwUpdateGamepadMappings")]
		public static extern bool UpdateGamepadMappings (NativeUtf8String mapping);

		[DllImport (GlfwDll, CallingConvention = CallingConvention.Cdecl, EntryPoint = "glfwGetGamepadName")]
		public static extern NativeUtf8String GetGamepadName (int gamepadID);

		[DllImport (GlfwDll, CallingConvention = CallingConvention.Cdecl, EntryPoint = "glfwGetGamepadState")]
		public static extern bool GetGamepadState (int gamepadID, GamepadStatePtr gamepadState);


		[DllImport (GlfwDll, CallingConvention = CallingConvention.Cdecl, EntryPoint = "glfwGetTime")]
		public static extern double GetTime ();

		[DllImport (GlfwDll, CallingConvention = CallingConvention.Cdecl, EntryPoint = "glfwSetTime")]
		public static extern void SetTime (double time);

		[DllImport (GlfwDll, CallingConvention = CallingConvention.Cdecl, EntryPoint = "glfwGetTimerValue")]
		public static extern UInt64 GetTimerValue ();

		[DllImport (GlfwDll, CallingConvention = CallingConvention.Cdecl, EntryPoint = "glfwGetTimerFrequency")]
		public static extern UInt64 GetTimerFrequency ();

		[DllImport (GlfwDll, CallingConvention = CallingConvention.Cdecl, EntryPoint = "glfwMakeContextCurrent")]
		public static extern void MakeContextCurrent (GLFWwindow window);

		[DllImport (GlfwDll, CallingConvention = CallingConvention.Cdecl, EntryPoint = "glfwGetCurrentContext")]
		public static extern GLFWwindow GetCurrentContext ();

		[DllImport (GlfwDll, CallingConvention = CallingConvention.Cdecl, EntryPoint = "glfwSwapBuffers")]
		public static extern void SwapBuffers (GLFWwindow window);

		[DllImport (GlfwDll, CallingConvention = CallingConvention.Cdecl, EntryPoint = "glfwSwapInterval")]
		public static extern void SwapInterval (int interval);

		[DllImport (GlfwDll, CallingConvention = CallingConvention.Cdecl, EntryPoint = "glfwExtensionSupported")]
		public static extern int ExtensionSupported (NativeString extname);

		[DllImport (GlfwDll, CallingConvention = CallingConvention.Cdecl, EntryPoint = "glfwGetProcAddress")]
		public static extern IntPtr GetProcAddress (NativeString procName);

		[DllImport (GlfwDll, CallingConvention = CallingConvention.Cdecl, EntryPoint = "glfwVulkanSupported")]
		public static extern bool VulkanSupported ();

		[DllImport (GlfwDll, CallingConvention = CallingConvention.Cdecl, EntryPoint = "glfwGetRequiredInstanceExtensions")]
		public static extern IntPtr GetRequiredInstanceExtensions (out UInt32 count);

		[DllImport (GlfwDll, CallingConvention = CallingConvention.Cdecl, EntryPoint = "glfwGetInstanceProcAddress")]
		public static extern IntPtr GetInstanceProcAddress (IntPtr vkInstance, NativeString procName);

		[DllImport (GlfwDll, CallingConvention = CallingConvention.Cdecl, EntryPoint = "glfwGetPhysicalDevicePresentationSupport")]
		public static extern bool GetPhysicalDevicePresentationSupport (IntPtr instance, IntPtr phy, UInt32 queuefamily);

		#endregion
		[DllImport (GlfwDll, CallingConvention = CallingConvention.Cdecl, EntryPoint = "glfwGetX11Display")]
		public static extern IntPtr GetX11Display ();
		[DllImport (GlfwDll, CallingConvention = CallingConvention.Cdecl, EntryPoint = "glfwGetX11Window")]
		public static extern IntPtr GetX11Window (GLFWwindow window);

		[DllImport (GlfwDll, CallingConvention = CallingConvention.Cdecl, EntryPoint = "glfwGetWin32Window")]
		public static extern IntPtr GetWin32Window (GLFWwindow window);


		[DllImport ("X11", CallingConvention = CallingConvention.Cdecl, EntryPoint = "XDefaultVisual")]
		public static extern IntPtr GetX11DefaultVisual (IntPtr disp, Int32 screen);
		[DllImport ("X11", CallingConvention = CallingConvention.Cdecl, EntryPoint = "XDefaultScreen")]
		public static extern Int32 GetX11DefaultScreen (IntPtr disp);

		[DllImport ("user32.dll", SetLastError = true, EntryPoint = "GetDC")]
		[SuppressUnmanagedCodeSecurity]
		public static extern IntPtr GetWin32DC (IntPtr hWnd);

		//Egl
		[DllImport (GlfwDll, CallingConvention = CallingConvention.Cdecl, EntryPoint = "glfwGetEGLDisplay")]
		public static extern IntPtr GetEGLDisplay ();
		[DllImport (GlfwDll, CallingConvention = CallingConvention.Cdecl, EntryPoint = "glfwGetEGLContext")]
		public static extern IntPtr GetEGLContext (IntPtr window);
		[DllImport (GlfwDll, CallingConvention = CallingConvention.Cdecl, EntryPoint = "glfwGetEGLSurface")]
		public static extern IntPtr GetEGLSurface (IntPtr window);

	}
}
