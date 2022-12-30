using OpenTK.Platform.Windows;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MeteorEngine
{
	public enum Flags
	{
		None = 0,
		HideInHierarchy = 1,
		HideInInspector = 2,
		DontSaveInEditor = 4,
		NotEditable = 8,
		DontSaveInBuild = 16,
		DontUnloadUnusedAsset = 32,
		DontSave = 64,
		HideAndDontSave = 128
	}

	public abstract class Object : IDisposable
	{
		protected Guid m_instanceId;
		public string Name;
		public Flags Flags;

		internal Object()
		{
			m_instanceId = Guid.NewGuid();

			AddObject(this);
		}

		internal Object(Guid instanceId)
		{
			m_instanceId = instanceId;

			AddObject(this);
		}

		public Guid GetInstanceID()
		{
			return m_instanceId;
		}

		public override string ToString()
		{
			return Name;
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(this, obj))
			{
				return true;
			}

			if (ReferenceEquals(obj, null))
			{
				return false;
			}

			return m_instanceId == ((Object)obj)?.m_instanceId;
		}

		public override int GetHashCode()
		{
			return m_instanceId.GetHashCode();
		}

		public abstract void Dispose();

		#region Statics
		private static Dictionary<Type, List<Object>> _instances = new Dictionary<Type, List<Object>>();
		private static List<Object> _objectsToRelease = new List<Object>();

		private static void AddObject(Object obj)
		{
			if(_instances.ContainsKey(obj.GetType()))
			{
				_instances[obj.GetType()].Add(obj);
			}
			else
			{
				_instances.Add(obj.GetType(), new List<Object>());
				_instances[obj.GetType()].Add(obj);
			}
		}

		public static bool operator ==(Object objA, Object objB)
		{
			if (objA is null)
				return objB is null;

			return objA.Equals(objB);
		}

		public static bool operator !=(Object objA, Object objB)
		{
			if (objA is null)
				return objB is not null;

			if (objB is null)
				return objA is not null;

			return !objA.Equals(objB);
		}

		public static void Destroy(Object obj)
		{
			if (_instances.ContainsKey(obj.GetType()))
			{
				_instances[obj.GetType()].Remove(obj);
				_objectsToRelease.Add(obj);
			}
		}

		public static void DestroyImmediate(Object obj)
		{
			if (_instances.ContainsKey(obj.GetType()))
			{
				_instances[obj.GetType()].Remove(obj);
				ReleaseObject(obj);
			}
		}

		private static void ReleaseObjects()
		{
			foreach(Object obj in _objectsToRelease)
			{
				ReleaseObject(obj);
			}

			_objectsToRelease.Clear();
		}

		private static void ReleaseObject(Object obj)
		{
			obj.Dispose();
		}

		public static Object FindObjectOfType(Type type)
		{
			if(_instances.ContainsKey(type) && _instances[type].Count > 0)
				return _instances[type][0];

			return null;
		}

		internal static Object FindObjectWithId<T>(Guid objectId) where T : Object
		{
			if (_instances.ContainsKey(typeof(T)))
			{
				return _instances[typeof(T)].Where(o => o.m_instanceId == objectId).First() as T;
			}
			else
				return null;
		}

		public static T FindObjectOfType<T>() where T : Object
		{
			return FindObjectOfType(typeof(T)) as T;
		}

		public static Object[] FindObjectsOfType(Type type)
		{
			if (_instances.ContainsKey(type) && _instances[type].Count > 0)
				return _instances[type].ToArray();

			return null;
		}

		public static T[] FindObjectsOfType<T>() where T : Object
		{
			return FindObjectsOfType(typeof(T)) as T[];
		}

		public static T Instantiate<T>() where T : Object, new()
		{
			T result = new T();
			result.Name = typeof(T).Name;
			result.m_instanceId = Guid.NewGuid();

			if (_instances.ContainsKey(typeof(T)))
			{
				_instances[typeof(T)].Add(result);
			}
			else
			{
				_instances.Add(typeof(T), new List<Object>());
				_instances[typeof(T)].Add(result);
			}

			return result;
		}
		#endregion
	}
}