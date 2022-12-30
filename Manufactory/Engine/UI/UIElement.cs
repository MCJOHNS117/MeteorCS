using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using System;

namespace MeteorEngine
{
	/// <summary>
	/// The basis for all UI elements.
	/// </summary>
	public abstract class UIElement : Component
	{
		private Vector4[] quadVerticies = new Vector4[6]
		{
						//pos		//tex
			new Vector4(0.0f, 1.0f, 0.0f, 1.0f),
			new Vector4(1.0f, 0.0f, 1.0f, 0.0f),
			new Vector4(0.0f, 0.0f, 0.0f, 0.0f),

			new Vector4(0.0f, 1.0f, 0.0f, 1.0f),
			new Vector4(1.0f, 1.0f, 1.0f, 1.0f),
			new Vector4(1.0f, 0.0f, 1.0f, 0.0f)
		};

		private int quadVAO;
		private int quadBuffer;

		protected UIElement()
		{
			quadVAO = GL.GenVertexArray();
			quadBuffer = GL.GenBuffer();

			GL.BindBuffer(BufferTarget.ArrayBuffer, quadBuffer);
			GL.BufferData(BufferTarget.ArrayBuffer, sizeof(float) * 4 * 6, quadVerticies, BufferUsageHint.StaticDraw);

			GL.BindVertexArray(quadVAO);
			GL.EnableVertexArrayAttrib(quadVAO, 0);
			GL.VertexAttribPointer(0, 4, VertexAttribPointerType.Float, false, 4 * sizeof(float), 0);

			GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
			GL.BindVertexArray(0);
		}

		public RectTransform Transform { get; protected set; }

		protected virtual void OnRender() { }

		public void Render()
		{
			OnRender();

			GL.BindVertexArray(quadVAO);
			GL.DrawArrays(PrimitiveType.Triangles, 0, 6);
			GL.BindVertexArray(0);
		}
	}
}