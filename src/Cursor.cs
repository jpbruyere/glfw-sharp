// Copyright (c) 2013-2020  Jean-Philippe Bruyère <jp_bruyere@hotmail.com>
//
// This code is licensed under the MIT license (MIT) (http://opensource.org/licenses/MIT)
using System;
using System.Runtime.InteropServices;

namespace Glfw
{
	public class CustomCursor : Cursor
	{
		Image img;
		IntPtr imgPtr;

		public CustomCursor (Image img, uint xHot, uint yHot)
		{
			imgPtr = Marshal.AllocHGlobal (Marshal.SizeOf<Image> ());
			Marshal.StructureToPtr (img, imgPtr, false);
			this.img = img;
			hnd = Glfw3.CreateCursor (imgPtr, (int)xHot, (int)yHot);
		}
		public CustomCursor (uint width, uint height, byte [] data, uint xHot, uint yHot) :
			this (new Image (width, height, data), xHot, yHot)
		{ }

		protected override void Dispose (bool disposing)
		{
			base.Dispose (disposing);
			Marshal.FreeHGlobal (imgPtr);
			img.Dispose ();
		}
	}

	public class Cursor : IDisposable
	{
		protected IntPtr hnd;

		protected Cursor () { }
		public Cursor (CursorShape cursor)
		{
			hnd = Glfw3.CreateStandardCursor (cursor);
		}

		public void Set (IntPtr hWin)
		{
			Glfw3.SetCursor (hWin, hnd);
		}

		protected virtual void Dispose (bool disposing)
		{
			Glfw3.DestroyCursor (hnd);
		}
		public void Dispose ()
		{
			Dispose (true);
		}

	}
}
