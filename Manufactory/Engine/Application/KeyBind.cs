using OpenTK.Input;
using System;

namespace Meteor.Engine.Application
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

		public Key Key { get; protected set; }

		public KeyBind(Key key, byte mods = 0)
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