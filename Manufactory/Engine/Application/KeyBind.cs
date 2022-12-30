using OpenTK.Windowing.GraphicsLibraryFramework;
using System;

namespace MeteorEngine
{
	[Flags]
	public enum KeyModifiers
	{
		Alt = 1,
		Ctrl = 2,
		Shift = 4
	}

	public class KeyBind
	{
		public byte Modifiers { get; protected set; }

		public Keys Key { get; protected set; }

		public KeyBind(Keys key, byte mods = 0)
		{
			Modifiers = mods;
			Key = key;
		}

		public override int GetHashCode()
		{
			return (Key.ToString() + ":" + Modifiers).GetHashCode();
		}

		public override bool Equals(object obj)
		{
			KeyBind other = obj as KeyBind;
			return other != null && other.Key == this.Key && other.Modifiers == this.Modifiers;
		}
	}
}