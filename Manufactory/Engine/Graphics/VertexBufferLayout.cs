using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;

namespace Meteor.Engine.Graphics
{
	public struct VertexBufferElement
	{
		public VertexAttribPointerType type;
		public int count;
		public bool normalized;
		public int stride;
		public int size;

		public static int GetSizeOfType(VertexAttribPointerType type)
		{
			switch(type)
			{
				case VertexAttribPointerType.Float: return 4;
				case VertexAttribPointerType.Int: return 4;
				case VertexAttribPointerType.Byte: return 1;
			}

			throw new Exception("VertexBufferElement.GetSizeOfType: Error - \"" + type.ToString() +
			                    "\" not supported.");
			return 0;
		}
	}

	public class VertexBufferLayout
	{
		private List<VertexBufferElement> m_vertexBufferElements;

		public VertexBufferLayout()
		{
			m_vertexBufferElements = new List<VertexBufferElement>();
		}

		public void AddLayout(VertexAttribPointerType vertexAttribPointerType, int count, int size)
		{
			m_vertexBufferElements.Add(new VertexBufferElement()
			{
				count = count,
				normalized = false,
				type = vertexAttribPointerType, 
				stride = count * VertexBufferElement.GetSizeOfType(vertexAttribPointerType),
				size = size
			});
		}

		public List<VertexBufferElement> VertexBufferElements()
		{
			return m_vertexBufferElements;
		}
	}
}