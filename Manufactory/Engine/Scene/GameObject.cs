using System;
using System.Collections.Generic;

namespace Meteor.Engine.Scene
{
	public class GameObject : CObject
	{
		public string Name { get; set; }
		public Transform Transform { get; set; }

		private List<Component> _components;

		public GameObject() : base()
		{
			Name = "GameObject";
			Transform = new Transform();
			_components = new List<Component>();
		}

		public Component GetComponent<T>()
		{
			return GetComponent(typeof(T));
		}

		public Component GetComponent(Type type)
		{
			return _components.Find(c => c.GetType() == type);
		}

		public void AddComponent<T>() where T : Component, new()
		{
			if (null == _components.Find(c => c.GetType() == typeof(T)))
			{
				T c = new ComponentFactory(this) as T;
				_components.Add(c);
			}
		}

		public void RemoveComponent<T>()
		{
			Component comp = _components.Find(c => c.GetType() == typeof(T));
			if (null == comp)
				return;

			_components.Remove(comp);
		}

		private class ComponentFactory : Component
		{
			public ComponentFactory(GameObject gameObject) : base()
			{
				GameObject = gameObject;
			}
		}
	}
}