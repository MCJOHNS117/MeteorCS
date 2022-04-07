using System;
using Meteor.Engine.Application;

namespace Manufactory
{
	static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{
			new CWindow().Run(60);
		}
	}
}