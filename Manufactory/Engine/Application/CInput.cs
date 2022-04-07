using OpenTK;
using OpenTK.Input;
using System;
using System.Collections.Generic;

namespace Meteor.Engine.Application
{
	[Flags]
	public enum BindType
	{
		OnKey = 0,
		OnKeyDown = 1,
		OnKeyUp = 2
	}

	public class CInput
	{
		private static Dictionary<KeyBind, Action> onKeyDownEvents = new Dictionary<KeyBind, Action>();
		private static Dictionary<KeyBind, Action> onKeyUpEvents = new Dictionary<KeyBind, Action>();
		private static Dictionary<KeyBind, Action> onKeyEvents = new Dictionary<KeyBind, Action>();

		//use this list to ignore multiple key presses until the key is released
		private static List<KeyBind> _heldKeys = new List<KeyBind>();

		private static bool[] _mouseButtons = new bool[13]{
			false, false, false, false, false, false,
			false, false, false, false, false, false, false,
		};

		private static Vector2 _mouseDelta = new Vector2();
		private static Vector2 _mousePosition = new Vector2();

		public static Vector2 MouseDelta {  get { return _mouseDelta; } }
		public static Vector2 MousePosition { get { return _mousePosition; } }

		public static void OnMouseMove(int x, int y, int dx, int dy)
		{
			_mouseDelta = new Vector2(dx, dy);
			_mousePosition = new Vector2(x, y);
		}

		public static void OnMouseDown(MouseButton mouseButton)
		{
			_mouseButtons[(int)mouseButton] = true;
		}

		public static void OnMouseUp(MouseButton mouseButton)
		{
			_mouseButtons[(int)mouseButton] = false;
		}

		public static bool IsMousePressed(MouseButton mouseButton)
		{
			return _mouseButtons[(int)mouseButton];
		}

		public static void OnKeyDown(Key key)
		{
			KeyModifiers mods = 0;
			if (key == Key.LShift || key == Key.RShift)
			{
				mods |= KeyModifiers.Shift;
			}

			if(key == Key.LControl || key == Key.RControl)
			{
				mods |= KeyModifiers.Ctrl;
			}

			if(key == Key.LAlt || key == Key.RAlt)
			{
				mods |= KeyModifiers.Alt;
			}

			KeyBind bind = new KeyBind(key, (byte)mods);

			//Invoke the action for this bind if it exists
			if (onKeyDownEvents.ContainsKey(bind))
				onKeyDownEvents[bind]?.Invoke();

			//invoke the action for this bind if it exists
			if(onKeyEvents.ContainsKey(bind))
			{
				onKeyEvents[bind]?.Invoke();

				if (!_heldKeys.Contains(bind))
					_heldKeys.Add(bind);
			}
		}

		public static void OnKeyUp(Key key)
		{
			KeyModifiers mods = 0;
			if (key == Key.LShift || key == Key.RShift)
			{
				mods |= KeyModifiers.Shift;
			}

			if (key == Key.LControl || key == Key.RControl)
			{
				mods |= KeyModifiers.Ctrl;
			}

			if (key == Key.LAlt || key == Key.RAlt)
			{
				mods |= KeyModifiers.Alt;
			}

			KeyBind bind = new KeyBind(key, (byte)mods);

			//Fire an event if theres a bind for it
			if (onKeyUpEvents.ContainsKey(bind))
				onKeyUpEvents[bind]?.Invoke();

			//Clear this bind from the held keys
			if (_heldKeys.Contains(bind))
				_heldKeys.Remove(bind);
		}

		public static void AddKeybind(KeyBind keyBind, BindType bindType, Action handler)
		{
			switch (bindType)
			{
				case BindType.OnKeyDown:
					if (onKeyDownEvents.ContainsKey(keyBind))
						onKeyDownEvents[keyBind] = handler;
					else
					{
						onKeyDownEvents.Add(keyBind, handler);
					}
					break;
				case BindType.OnKeyUp:
					if (onKeyUpEvents.ContainsKey(keyBind))
						onKeyUpEvents[keyBind] = handler;
					else
					{
						onKeyUpEvents.Add(keyBind, handler);
					}
					break;
				case BindType.OnKey:
					if (onKeyEvents.ContainsKey(keyBind))
						onKeyEvents[keyBind] = handler;
					else
					{
						onKeyEvents.Add(keyBind, handler);
					}
					break;
			}
		}

		public static void Update()
		{
			_mouseDelta = Vector2.Zero;
		}
	}
}