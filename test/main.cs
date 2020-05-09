﻿// Copyright (c) 2013-2020  Jean-Philippe Bruyère <jp_bruyere@hotmail.com>
//
// This code is licensed under the MIT license (MIT) (http://opensource.org/licenses/MIT)
using System;
using Glfw;

namespace test {
	public class test {
		static void Main () {
			Glfw3.Init ();
			Console.WriteLine ($"GLFW3 version: {Glfw3.GetVersion ()}");
			Glfw3.Terminate ();
		}
	}
}
