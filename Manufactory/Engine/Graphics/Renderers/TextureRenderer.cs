using OpenTK.Graphics.OpenGL4;
using System;

namespace MeteorEngine
{
	public class TextureRenderer : RenderBase
	{
		private VertexPUV[] _vertices;
		private Texture2D _texture;
		private int _program;
		public TextureRenderer(VertexPUV[] verts, int program, Texture2D texture) : base(verts.Length)
		{
			_program = program;
			_vertices = verts;
			Initialize();
			_texture = texture;
			_initialized = true;
		}

		protected override void Initialize()
		{
			GL.BindVertexArray(_vertexArray);
			GL.BindBuffer(BufferTarget.ArrayBuffer, _buffer);

			GL.NamedBufferStorage(_buffer, VertexPUV.Size * _vertices.Length, _vertices, BufferStorageFlags.MapWriteBit);

			//Position
			GL.VertexArrayAttribBinding(_vertexArray, 0, 0);
			GL.EnableVertexArrayAttrib(_vertexArray, 0);
			GL.VertexArrayAttribFormat(_vertexArray, 0, 4, VertexAttribType.Float, false, 0);

			//Color
			GL.VertexArrayAttribBinding(_vertexArray, 1, 0);
			GL.EnableVertexArrayAttrib(_vertexArray, 1);
			GL.VertexArrayAttribFormat(_vertexArray, 1, 2, VertexAttribType.Float, false, 16);

			GL.VertexArrayVertexBuffer(_vertexArray, 0, _buffer, IntPtr.Zero, VertexPUV.Size);
		}

		public override void Bind()
		{
			GL.UseProgram(_program);
			GL.BindVertexArray(_vertexArray);
			_texture.Apply();
		}

		public override void Render()
		{
			GL.DrawArrays(PrimitiveType.Triangles, 0, _vertexCount);
		}
	}
}