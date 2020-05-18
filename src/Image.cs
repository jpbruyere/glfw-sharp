// Copyright (c) 2013-2020  Jean-Philippe Bruyère <jp_bruyere@hotmail.com>
//
// This code is licensed under the MIT license (MIT) (http://opensource.org/licenses/MIT)
using System;
using System.Runtime.InteropServices;

namespace Glfw
{
	[StructLayout(LayoutKind.Sequential)]
	public struct Image : IDisposable
	{
		readonly int width;
		readonly int height;
		readonly IntPtr data;

		public Image (uint width, uint height, byte[] data)
		{
			this.width = (int)width;
			this.height = (int)height;
			this.data = Marshal.AllocHGlobal (data.Length);
			Marshal.Copy (data, 0, this.data, data.Length);
		}

		public void Dispose ()
		{
			if (data != IntPtr.Zero)
				Marshal.FreeHGlobal (data);
		}
	}
}
