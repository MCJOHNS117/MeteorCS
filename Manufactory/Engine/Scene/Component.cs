using System;
using System.Text.Json;

namespace MeteorEngine
{
	public class Component : Object
	{
		public GameObject GameObject { get; internal set; }

		public Transform Transform { get { return GameObject.Transform; } }

		public virtual void Awake() { }
		public virtual void Start() { }
		public virtual void FixedUpdate() { }
		public virtual void Update(float deltaTime) { }
		public virtual void LateUpdate(float deltaTime) { }

		public virtual void OnDestroy() { }

		public virtual void OnDispose() { }

		public override void Dispose()
		{
			OnDispose();
		}

		public void BroadcastMessage()
		{

		}
	}
}