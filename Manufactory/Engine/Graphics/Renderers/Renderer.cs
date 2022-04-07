using Meteor.Engine.Scene;

namespace Meteor.Engine.Graphics.Renderers
{
	/// <summary>
	/// Provides a basis for MeshRenderer, SkinnedMeshRenderer, and CanvasRenderer
	/// </summary>
	public class Renderer : Component
	{
		public Bounds Bounds { get; protected set; }

		public bool Enabled { get; set; }

		public bool StaticBatched { get; set; }
	}
}