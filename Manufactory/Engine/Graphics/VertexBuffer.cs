using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using System;

namespace MeteorEngine
{
	public class VertexBuffer : IDisposable
	{
		private int m_rendererId;

		private int m_offset;

		private VertexBufferLayout m_vertexBufferLayout;

		public VertexBuffer(int size, BufferUsageHint bufferUsageHint = BufferUsageHint.StaticDraw)
		{
			m_rendererId = GL.GenBuffer();
			GL.BindBuffer(BufferTarget.ArrayBuffer, m_rendererId);
			GL.BufferData(BufferTarget.ArrayBuffer, size, (IntPtr)null, bufferUsageHint);
			m_vertexBufferLayout = new VertexBufferLayout();
			m_offset = 0;
		}

		public void AddSubData(Vector2[] data)
		{
			GL.BindBuffer(BufferTarget.ArrayBuffer, m_rendererId);
			GL.BufferSubData(BufferTarget.ArrayBuffer, (IntPtr)m_offset, data.Length * Vector2.SizeInBytes, data);
			m_vertexBufferLayout.AddLayout(VertexAttribPointerType.Float, 2, data.Length * Vector2.SizeInBytes);
			m_offset += (data.Length * Vector2.SizeInBytes);
		}

		public void AddSubData(Vector3[] data)
		{
			GL.BindBuffer(BufferTarget.ArrayBuffer, m_rendererId);
			GL.BufferSubData(BufferTarget.ArrayBuffer, (IntPtr)m_offset, data.Length * Vector3.SizeInBytes, data);
			m_vertexBufferLayout.AddLayout(VertexAttribPointerType.Float, 3, data.Length * Vector3.SizeInBytes);
			m_offset += (data.Length * Vector3.SizeInBytes);
		}

		public void AddSubData(Vector4[] data)
		{
			GL.BindBuffer(BufferTarget.ArrayBuffer, m_rendererId);
			GL.BufferSubData(BufferTarget.ArrayBuffer, (IntPtr)m_offset, data.Length * Vector4.SizeInBytes, data);
			m_vertexBufferLayout.AddLayout(VertexAttribPointerType.Float, 4, data.Length * Vector4.SizeInBytes);
			m_offset += (data.Length * Vector4.SizeInBytes);
		}

		public VertexBufferLayout GetBufferLayout()
		{
			return m_vertexBufferLayout;
		}

		public void Dispose()
		{
			GL.DeleteBuffer(m_rendererId);
		}

		public void Bind()
		{
			GL.BindBuffer(BufferTarget.ArrayBuffer, m_rendererId);
		}

		public void Unbind()
		{
			GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
		}
	}
}