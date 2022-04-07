﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace Meteor.Engine.Application
{
	public static class Settings
	{
		private static Dictionary<string, string> _settings = new Dictionary<string, string>();
		private static List<string> _changedSettings = new List<string>();

		private static bool _initialized = false;

		private static Dictionary<string, Action> _settingsChangedHandlers = new Dictionary<string, Action>();

		public static void Initialize()
		{

			StreamReader reader = new StreamReader(File.Open("settings.cfg", FileMode.OpenOrCreate));



			bool nextLine = true;
			string line;
			while(nextLine)
			{
				line = "";

				try
				{
					line = reader.ReadLine();
				}
				catch(Exception e)
				{
					Debug.WriteLine(e.ToString());
				}

				if(string.IsNullOrEmpty(line))
				{
					nextLine = false;
					continue;
				}

				string[] tokens = line.Split(':');
				if(!_settings.ContainsKey(tokens[0]))
				{
					_settings.Add(tokens[0].ToLower(), tokens[1]);
				}
				else
				{
					_settings[tokens[0]] = tokens[1];
				}
			}

			reader.Close();

			_initialized = true;
		}

		public static void Save()
		{
			StreamWriter writer = new StreamWriter(File.Open("settings.config", FileMode.Create));

			foreach(string key in _settings.Keys)
			{
				writer.WriteLine(key + ":" + _settings[key]);
			}

			writer.Close();
		}

		public static void AddSettingListener(string setting, Action action)
		{
			if (_settingsChangedHandlers.ContainsKey(setting.ToLower()))
			{
				//First subtract it, then add it to ensure only 1 of THIS action per listener
				_settingsChangedHandlers[setting.ToLower()] -= action;
				_settingsChangedHandlers[setting.ToLower()] += action;
			}
			else
			{
				_settingsChangedHandlers.Add(setting, action);
			}
		}

		public static void ApplyChanges()
		{
			foreach (string setting in _changedSettings)
			{
				if (_settingsChangedHandlers.ContainsKey(setting))
				{
					_settingsChangedHandlers[setting].Invoke();
				}
			}

			_changedSettings.Clear();

			Save();
		}

		private static void OnSettingChanged(string setting)
		{
			if (!_changedSettings.Contains(setting))
				_changedSettings.Add(setting);
		}

		public static bool GetString(string setting, out string val)
		{
			if (_settings.ContainsKey(setting.ToLower()) && _initialized)
			{
				try
				{
					val = _settings[setting.ToLower()];
					return true;
				}
				catch
				{
					val = null;
					return false;
				}
			}
			else
			{
				val = null;
				return false;
			}
		}

		public static bool GetBool(string setting, out bool val)
		{
			int intVal;
			bool result = GetInt(setting, out intVal);

			if(result)
			{
				if (intVal == 0)
				{
					val = false;
					return true;
				}
				else if(intVal == 1)
				{
					val = true;
					return true;
				}
			}
			
			val = false;
			return false;
		}

		public static bool GetFloat(string setting, out float val)
		{
			if(_settings.ContainsKey(setting.ToLower()) && _initialized)
			{
				try
				{
					val = float.Parse(_settings[setting.ToLower()]);
					return true;
				}
				catch(Exception e)
				{
					val = 0.0f;
					return false;
				}
			}
			else
			{
				val = 0.0f;
				return false;
			}
		}

		public static bool GetInt(string setting, out int val)
		{
			if (_settings.ContainsKey(setting.ToLower()) && _initialized)
			{
				try
				{
					val = int.Parse(_settings[setting.ToLower()]);
					return true;
				}
				catch (Exception e)
				{
					val = 0;
					return false;
				}
			}
			else
			{
				val = 0;
				return false;
			}
		}

		public static void SetString(string setting, string val)
		{
			if (!string.IsNullOrEmpty(val))
			{
				if (_settings.ContainsKey(setting))
				{
					_settings[setting] = val;
				}
				else
				{
					_settings.Add(setting, val);
				}
			}

			OnSettingChanged(setting.ToLower());
		}

		public static void SetInt(string setting, int val)
		{
			if(_settings.ContainsKey(setting.ToLower()))
			{
				_settings[setting.ToLower()] = "" + val;
			}
			else
			{
				_settings.Add(setting.ToLower(), "" + val);
			}

			OnSettingChanged(setting.ToLower());
		}

		public static void SetFloat(string setting, float val)
		{
			if (_settings.ContainsKey(setting.ToLower()))
			{
				_settings[setting.ToLower()] = "" + val;
			}
			else
			{
				_settings.Add(setting.ToLower(), "" + val);
			}

			OnSettingChanged(setting.ToLower());
		}

		public static void SetBool(string setting, bool val)
		{
			if (_settings.ContainsKey(setting.ToLower()))
			{
				_settings[setting.ToLower()] = "" + (val ? 1 : 0);
			}
			else
			{
				_settings.Add(setting.ToLower(), "" + (val ? 1 : 0));
			}

			OnSettingChanged(setting.ToLower());
		}
	}
}