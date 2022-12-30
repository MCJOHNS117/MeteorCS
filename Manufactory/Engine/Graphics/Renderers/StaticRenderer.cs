using OpenTK.Graphics.OpenGL4;
using System;

namespace MeteorEngine
{
	public class StaticRenderer : RenderBase
	{
		private VertexPC[] _vertices;
		private int _program;
		public StaticRenderer(VertexPC[] vertices, int program) : base(vertices.Length)
		{
			_program = program;
			_vertices = vertices;
			Initialize();
		}
		protected override void Initialize()
		{
			GL.BindVertexArray(_vertexArray);
			GL.BindBuffer(BufferTarget.ArrayBuffer, _buffer);

			GL.NamedBufferStorage(_buffer, VertexPC.Size * _vertices.Length, _vertices, BufferStorageFlags.MapWriteBit);

			//Position
			GL.VertexArrayAttribBinding(_vertexArray, 0, 0);
			GL.EnableVertexArrayAttrib(_vertexArray, 0);
			GL.VertexArrayAttribFormat(_vertexArray, 0, 4, VertexAttribType.Float, false, 0);

			//Color
			GL.VertexArrayAttribBinding(_vertexArray, 1, 0);
			GL.EnableVertexArrayAttrib(_vertexArray, 1);
			GL.VertexArrayAttribFormat(_vertexArray, 1, 4, VertexAttribType.Float, false, 16);

			GL.VertexArrayVertexBuffer(_vertexArray, 0, _buffer, IntPtr.Zero, VertexPC.Size);

			_initialized = true;
		}

		public override void Bind()
		{
			GL.UseProgram(_program);
			GL.BindVertexArray(_vertexArray);
		}
		public override void Render()
		{
			GL.DrawArrays(PrimitiveType.Triangles, 0, _vertexCount);
		}
	}
}
