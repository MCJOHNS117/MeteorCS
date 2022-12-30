using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace MeteorEngine
{
	public class GameObject : Object
	{
		public string Tag { get; set; }
		public int Layer { get; set; }
		public Transform Transform { get; set; }

		private bool _enabled;
		public bool Enabled { get { return _enabled; } set { SetEnabled(value); } }

		private List<Component> _components;

		private protected GameObject() : base()
		{
			Name = "GameObject";
			Tag = "GameObject";
			Transform = new Transform();
			Transform.GameObject = this;
			_components = new List<Component>();
		}

		internal GameObject(Guid id, string name, string tag) : base(id)
		{
			Name = name;
			Tag = tag;
			Transform = new Transform();
			Transform.GameObject = this;
			_components = new List<Component>();
		}

		public T GetComponent<T>() where T : Component
		{
			return GetComponent(typeof(T)) as T;
		}

		public Component GetComponent(Type type)
		{
			return _components.Find(c => c.GetType() == type);
		}

		public Component[] GetComponents()
		{
			return _components.ToArray();
		}

		public T AddComponent<T>() where T : Component, new()
		{
			if(_components.Find(c => c.GetType() == typeof(T)) == null)
			{
				T c = new T();
				c.GameObject = this;
				_components.Add(c);

				return c;
			}

			return GetComponent<T>();
		}

		public Component AddComponent(Type type)
		{
			if (type.IsAssignableTo(typeof(Component)))
			{
				if (null == _components.Find(c => c.GetType() == type))
				{
					Component c = (Component)Activator.CreateInstance(type);
					c.GameObject = this;
					_components.Add(c);

					return c;
				}

				return GetComponent(type);
			}

			return null;
		}

		public void RemoveComponent<T>()
		{
			Component comp = _components.Find(c => c.GetType() == typeof(T));
			if (null == comp)
				return;

			_components.Remove(comp);
		}

		public void SetEnabled(bool enabled)
		{
			_enabled = enabled;
		}

		public override void Dispose()
		{
			foreach(Component comp in _components)
			{
				comp.OnDestroy();
			}
			_components.Clear();
		}

		#region Statics
		public static GameObject Instantiate(string name)
		{
			GameObject gameObject = new GameObject();
			gameObject.Name = name;
			gameObject.Tag = "GameObject";
			return gameObject;
		}

		public static GameObject Instantiate(string name, Transform transform)
		{
			GameObject gameObject = new GameObject();
			gameObject.Name = name;
			gameObject.Tag = "GameObject";
			gameObject.Transform = transform;
			gameObject.Transform.GameObject = gameObject;
			return gameObject;
		}

		public static GameObject CreateCopy(GameObject original, Vector3 postion, Quaternion rotation, Transform parent)
		{
			GameObject result = new GameObject();
			result.Name = original.Name;
			result.Transform.Position = postion;
			result.Transform.Rotation = rotation;
			result.Transform.Parent = parent;
			result.Transform.GameObject = result;

			foreach(Component component in original._components)
			{
				result.AddComponent(typeof(Component));
			}

			return result;
		}

		public static GameObject LoadGameObject(string filename)
		{
			string json = File.ReadAllText(filename);

			Transform result = JsonSerializer.Deserialize<Transform>(json, JsonConverters.SerializerOptions);
			return result.GameObject;
		}

		public static GameObject FindGameObjectWithTag(string tag)
		{
			GameObject[] gameObjects = FindObjectsOfType<GameObject>();
			return gameObjects.Where(k => k.Tag == tag).ToArray().FirstOrDefault();
		}

		public static GameObject[] FindGameObjectsWithTag(string tag)
		{
			GameObject[] gameObjects = FindObjectsOfType<GameObject>();
			return gameObjects.Where(k => k.Tag == tag).ToArray();
		}

		public static GameObject FindGameObjectWithName(string name)
		{
			GameObject[] gameObjects = FindObjectsOfType<GameObject>();
			return gameObjects.Where(k => k.Name == name).ToArray().FirstOrDefault();
		}

		public static GameObject[] FindGameObjectsWithName(string name)
		{
			GameObject[] gameObjects = FindObjectsOfType<GameObject>();
			return gameObjects.Where(k => k.Name == name).ToArray();
		}
		#endregion
	}
}