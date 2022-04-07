using OpenTK;
using OpenTK.Graphics.OpenGL4;
using System;

namespace Meteor.Engine.UI
{
	/// <summary>
	/// The basis for all UI elements.
	/// </summary>
	public abstract class UIElement
	{
		protected Vector2[] _verticies = new Vector2[4]
		{
			new Vector2(-1.0f, 1.0f),
			new Vector2(-1.0f, -1.0f),
			new Vector2(1.0f, 1.0f),

			new Vector2(1.0f, -1.0f),
			//new Vector4(-1.0f, -1.0f, 1.0f, 1.0f),
			//new Vector4(1.0f, -1.0f, 1.0f, 0.0f)
		};

		protected int _quadVAO;
		protected int _quadBuffer;

		protected UIElement()
		{
			_quadVAO = GL.GenVertexArray();
			_quadBuffer = GL.GenBuffer();

			GL.BindVertexArray(_quadVAO);
			GL.BindBuffer(BufferTarget.ArrayBuffer, _quadBuffer);

			GL.NamedBufferStorage(_quadBuffer, 4 * 2 * 4, _verticies, BufferStorageFlags.MapWriteBit);

			GL.VertexArrayAttribBinding(_quadVAO, 0, 0);
			GL.EnableVertexArrayAttrib(_quadVAO, 0);
			GL.VertexArrayAttribFormat(_quadVAO, 0, 2, VertexAttribType.Float, false, 0);

			GL.VertexArrayVertexBuffer(_quadVAO, 0, _quadBuffer, IntPtr.Zero, 8);
			
			GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
			GL.BindVertexArray(0);
		}

		public RectTransform Transform { get; protected set; }
		
		public abstract void Update();
		public abstract void Render();
	}
}