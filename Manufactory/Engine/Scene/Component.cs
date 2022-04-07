namespace Meteor.Engine.Scene
{
	public class Component : CObject
	{
		public GameObject GameObject { get; protected set; }

		public Transform Transform { get { return GameObject.Transform; } }

		public virtual void Awake() { }
		public virtual void Start() { }
		public virtual void FixedUpdate() { }
		public virtual void Update() { }
		public virtual void LateUpdate() { }

		public void BroadcastMessage()
		{

		}
	}
}