using OpenTK.Graphics.OpenGL4;
using System;

namespace MeteorEngine
{
	public class FontRenderer : RenderBase
	{
		private FontVertex[] _vertices;
		private Texture2D _texture;
		private int _program;

		public FontRenderer(FontVertex[] vertices, int program, Texture2D texture) : base(vertices.Length)
		{
			_vertices = vertices;
			_program = program;
			Initialize();
			_texture = texture;
			_initialized = true;
		}

		public override void Bind()
		{
			GL.UseProgram(_program);
			GL.BindVertexArray(_vertexArray);
			_texture.Apply();
			GL.ActiveTexture(TextureUnit.Texture0);

			GL.Enable(EnableCap.Blend);
			GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
			GL.Disable(EnableCap.DepthTest);
		}

		public override void Render()
		{
			GL.DrawArrays(PrimitiveType.Triangles, 0, _vertexCount);
			GL.Enable(EnableCap.DepthTest);
		}

		protected override void Initialize()
		{
			GL.BindVertexArray(_vertexArray);
			GL.BindBuffer(BufferTarget.ArrayBuffer, _buffer);

			GL.NamedBufferStorage(_buffer, FontVertex.Size * _vertices.Length, _vertices, BufferStorageFlags.MapWriteBit);

			//Position
			GL.VertexArrayAttribBinding(_vertexArray, 0, 0);
			GL.EnableVertexArrayAttrib(_vertexArray, 0);
			GL.VertexArrayAttribFormat(_vertexArray, 0, 2, VertexAttribType.Float, false, 0);

			//Color
			GL.VertexArrayAttribBinding(_vertexArray, 1, 0);
			GL.EnableVertexArrayAttrib(_vertexArray, 1);
			GL.VertexArrayAttribFormat(_vertexArray, 1, 2, VertexAttribType.Float, false, 8);

			GL.VertexArrayVertexBuffer(_vertexArray, 0, _buffer, IntPtr.Zero, FontVertex.Size);
		}
	}
}
