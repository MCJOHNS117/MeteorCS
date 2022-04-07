using OpenTK.Graphics.OpenGL4;
using System;

namespace Meteor.Engine.Graphics
{
	public class IndexBuffer : IDisposable
	{
		private int m_rendererId;
		private int m_count;

		public IndexBuffer(int[] data, BufferUsageHint bufferUsageHint = BufferUsageHint.StaticDraw)
		{
			m_count = data.Length;

			m_rendererId = GL.GenBuffer();
			GL.BindBuffer(BufferTarget.ElementArrayBuffer, m_rendererId);
			GL.BufferData(BufferTarget.ElementArrayBuffer, data.Length * sizeof(int), data, bufferUsageHint);
		}

		public int Count()
		{
			return m_count;
		}

		public void Bind()
		{
			GL.BindBuffer(BufferTarget.ElementArrayBuffer, m_rendererId);
		}

		public void Unbind()
		{
			GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
		}

		public void Dispose()
		{
			GL.DeleteBuffer(m_rendererId);
		}
	}
}