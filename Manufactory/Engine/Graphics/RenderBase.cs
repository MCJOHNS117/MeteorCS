using OpenTK.Graphics.OpenGL4;
using System;

namespace MeteorEngine
{
	public abstract class RenderBase : IDisposable
	{
		protected bool _initialized;
		protected readonly int _vertexArray;
		protected readonly int _buffer;
		protected readonly int _vertexCount;

		public RenderBase(int vertexCount)
		{
			_vertexArray = GL.GenVertexArray();
			_buffer = GL.GenBuffer();
			_vertexCount = vertexCount;
		}

		protected abstract void Initialize();
		public abstract void Bind();
		public abstract void Render();

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (_initialized)
				{
					GL.DeleteVertexArray(_vertexArray);
					GL.DeleteBuffer(_buffer);
					_initialized = false;
				}
			}
		}
	}
}
