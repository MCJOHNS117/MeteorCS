using OpenTK.Graphics.OpenGL4;
using System;

namespace Meteor.Engine.Graphics
{
	public class VertexArray : IDisposable
	{
		private int m_rendererId;

		public VertexArray()
		{
			m_rendererId = GL.GenVertexArray();
		}

		public void Dispose()
		{
			Unbind();
			GL.DeleteVertexArrays(1, ref m_rendererId);
		}

		public void AddBuffer(VertexBuffer buffer)
		{
			Bind();
			buffer.Bind();
			VertexBufferElement[] elements = buffer.GetBufferLayout().VertexBufferElements().ToArray();
			int offset = 0;
			for (int i = 0; i < elements.Length; i++)
			{
				VertexBufferElement element = elements[i];
				GL.EnableVertexAttribArray(i);
				GL.VertexAttribPointer(i, element.count, element.type, element.normalized, element.stride, offset);
				offset += element.size;
			}
		}

		public void Bind()
		{
			GL.BindVertexArray(m_rendererId);
		}

		public void Unbind()
		{
			GL.BindVertexArray(0);
		}
	}
}