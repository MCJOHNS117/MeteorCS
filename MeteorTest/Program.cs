﻿using MeteorEngine;
using System;

namespace Meteor
{
	static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{
			new CWindow().Run();
		}
	}
}