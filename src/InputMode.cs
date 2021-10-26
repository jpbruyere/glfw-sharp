// Copyright (c) 2019 Jean-Philippe Bruyère <jp_bruyere@hotmail.com>
//
// This code is licensed under the MIT license (MIT) (http://opensource.org/licenses/MIT)

namespace Glfw
{
	/// <summary>
	/// First parameter on the GetInputMode and SetInputMode.
	/// </summary>
	public enum InputMode : int
	{
		Cursor				= 0x00033001,
		StickyKeys			= 0x00033002,
		StickyMouseButtons	= 0x00033003,
		LockKeyMods			= 0x00033004,
		RawMouseMotion		= 0x00033005
	}
	public enum CursorMode : int
	{
		Normal				= 0x00034001,
		Hidden				= 0x00034002,
		Disable				= 0x00034003
	}
}
