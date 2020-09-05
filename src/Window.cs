// Copyright (c) 2019  Jean-Philippe Bruyère <jp_bruyere@hotmail.com>
//
// This code is licensed under the MIT license (MIT) (http://opensource.org/licenses/MIT)
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Glfw {
	public class Window : IDisposable {

		/** GLFW callback may return a custom pointer, this list makes the link between the GLFW window pointer and the
			manage VkWindow instance. */		
		static Dictionary<IntPtr,Window> windows = new Dictionary<IntPtr, Window>();
		/** GLFW window native pointer. */
		IntPtr hWin;
		IntPtr currentCursor;
		uint frameCount;
		Stopwatch frameChrono;

		protected uint fps { get; private set; }
		protected bool updateViewRequested = true;
		protected double lastMouseX { get; private set; }
		protected double lastMouseY { get; private set; }

		/// <summary>readonly GLFW window handle</summary>
		public IntPtr WindowHandle => hWin;
		public Modifier KeyModifiers = 0;

		/// <summary>
		/// Frequency in millisecond of the call to the Update method
		/// </summary>
		public long UpdateFrequency = 200;

		public uint Width { get; private set; }
		public uint Height { get; private set; }
		public bool VSync { get; private set; }
		public string Title {
			set => Glfw3.SetWindowTitle (hWin, value);
		}

		public static Dictionary<WindowAttribute, int> WindowAttributes = new Dictionary<WindowAttribute, int> ();

		/// <summary>
		/// This function is used to get the monitor for the call to 'CreateWindow'.
		/// Override it to customize monitor selection on window creation.
		/// </summary>
		/// <value>The selected monitor for creating the window.</value>
		protected virtual MonitorHandle GetMonitor => MonitorHandle.Zero;

		/// <summary>
		/// This method is used when calling 'CreateWindow' to get a shared context pointer. Default is IntPtr.Zero.
		/// Override this method to set a custom shared context pointer for creating the window.
		/// </summary>
		/// <value>Shared context pointer to use for creating the window.</value>
		protected virtual IntPtr GetSharedContext => IntPtr.Zero;

		/// <summary>
		/// Create a new native window with the glfw api. Attributes are configured through the static dictionnary 'WindowAttributes'
		/// of the window class.
		/// </summary>
		/// <param name="width">Width.</param>
		/// <param name="height">Height.</param>
		/// <param name="name">Caption of the window</param>
		/// <param name="vSync">Enable Vertical synchronisation, default is 'true'</param>
		public Window (uint width = 800, uint height = 600, string name = "VkWindow", bool vSync = true) {

			Width = width;
			Height = height;
			VSync = vSync;

			Glfw3.Init ();

			hWin = Glfw3.CreateWindow ((int)Width, (int)Height, name, GetMonitor, GetSharedContext);

			if (hWin == IntPtr.Zero)
				throw new Exception ("[GLFW3] Unable to create vulkan Window");

			Glfw3.SetKeyCallback (hWin, HandleKeyDelegate);
			Glfw3.SetMouseButtonPosCallback (hWin, HandleMouseButtonDelegate);
			Glfw3.SetCursorPosCallback (hWin, HandleCursorPosDelegate);
			Glfw3.SetScrollCallback (hWin, HandleScrollDelegate);
			Glfw3.SetCharCallback (hWin, HandleCharDelegate);

			windows.Add (hWin, this);
		}
		/// <summary>
		/// Set current mouse cursor in the GLFW window.
		/// </summary>
		/// <param name="cursor">New mouse cursor to set.</param>
		public void SetCursor (CursorShape cursor) {
			if (currentCursor != IntPtr.Zero)
				Glfw3.DestroyCursor (currentCursor);
			currentCursor = Glfw3.CreateStandardCursor (cursor);
			Glfw3.SetCursor (hWin, currentCursor);
		}
		/// <summary>
		/// Ask GLFW to close the native window.
		/// </summary>
		public void Close ()
		{
			Glfw3.SetWindowShouldClose (hWin, 1);
		}

		protected virtual void onScroll (double xOffset, double yOffset) { }
		protected virtual void onMouseMove (double xPos, double yPos) { }
		protected virtual void onMouseButtonDown (MouseButton button) { }
		protected virtual void onMouseButtonUp (MouseButton button) { }
		protected virtual void onKeyDown (Key key, int scanCode, Modifier modifiers) {
			switch (key) {
			case Key.F4:
				if (modifiers == Modifier.Alt)
					Close ();
				break;
			case Key.Escape:
				Close ();
				break;
			}
		}
		protected virtual void onKeyUp (Key key, int scanCode, Modifier modifiers) { }
		protected virtual void onChar (CodePoint cp) { }
		/// <summary>
		/// Get button state.
		/// </summary>
		/// <returns>The button status</returns>
		/// <param name="button">The Button to query the status for.</param>
		protected InputAction GetButton (MouseButton button) =>
			Glfw3.GetMouseButton (hWin, button);

		#region events delegates
		static CursorPosDelegate HandleCursorPosDelegate = (window, xPosition, yPosition) => {
			windows[window].onMouseMove (xPosition, yPosition);
			windows[window].lastMouseX = xPosition;
			windows[window].lastMouseY = yPosition;
		};
		static MouseButtonDelegate HandleMouseButtonDelegate = (IntPtr window, Glfw.MouseButton button, InputAction action, Modifier mods) => {
			if (action == InputAction.Press) 
				windows[window].onMouseButtonDown (button);
			 else
				windows[window].onMouseButtonUp (button);
		};
		static ScrollDelegate HandleScrollDelegate = (IntPtr window, double xOffset, double yOffset) => {
			windows[window].onScroll (xOffset, yOffset);
		};
		static KeyDelegate HandleKeyDelegate = (IntPtr window, Key key, int scanCode, InputAction action, Modifier modifiers) => {
			windows[window].KeyModifiers = modifiers;
			if (action == InputAction.Press || action == InputAction.Repeat) 
				windows[window].onKeyDown (key, scanCode, modifiers);
			else
				windows[window].onKeyUp (key, scanCode, modifiers);
		};
		static CharDelegate HandleCharDelegate = (IntPtr window, CodePoint codepoint) => {
			windows[window].onChar (codepoint);
		};
		#endregion

		#region IDisposable Support
		protected bool isDisposed;

		protected virtual void Dispose (bool disposing) {
			if (!isDisposed) {
				if (currentCursor != IntPtr.Zero)
					Glfw3.DestroyCursor (currentCursor);

				Glfw3.DestroyWindow (hWin);
				Glfw3.Terminate ();

				isDisposed = true;
			}
		}
		~Window () {
			Dispose (false);
		}
		public void Dispose () {
			Dispose (true);
			GC.SuppressFinalize (this);
		}
		#endregion
	}
}
