using System;
using System.Collections.Generic;
using Meteor.Engine.Graphics;
using OpenTK;

namespace Meteor.Engine.Application.Assets
{
	public class Mesh : IAsset
	{
		private Vector3[] m_vertices;
		private Vector3[] m_normals;
		private Vector2[] m_uvs;
		private int[] m_triangles;
		
		private VertexArray m_vertexArray;
		private IndexBuffer m_indexBuffer;

		private bool m_isDirty;

		public List<Mesh> SubMeshes;

		public Mesh()
		{
			SubMeshes = new List<Mesh>();
			m_vertexArray = new VertexArray();
			m_isDirty = true;
		}

		public Vector3[] GetVerticies() { return m_vertices; }

		public Vector3[] GetNormals() { return m_normals; }

		public Vector2[] GetUVs() { return m_uvs; }

		public void SetVertices(Vector3[] vertices)
		{
			m_vertices = vertices;
			m_isDirty = true;
		}

		public void SetNormals(Vector3[] normals)
		{
			m_normals = normals;
			m_isDirty = true;
		}

		public void SetUVs(Vector2[] uvs)
		{
			m_uvs = uvs;
			m_isDirty = true;
		}

		public void SetTriangles(int[] triangles)
		{
			m_triangles = triangles;

			m_indexBuffer?.Dispose();

			m_indexBuffer = new IndexBuffer(triangles);
		}

		public VertexArray GetVertexArray()
		{
			if (m_isDirty)
			{
				m_vertexArray?.Dispose();

				//Generate a new VA
				int size = (m_vertices.Length * Vector3.SizeInBytes) + (m_normals.Length * Vector3.SizeInBytes) + (m_uvs.Length * Vector2.SizeInBytes);

				VertexBuffer buffer = new VertexBuffer(size);
				buffer.AddSubData(m_vertices);
				buffer.AddSubData(m_normals);
				buffer.AddSubData(m_uvs);

				m_vertexArray = new VertexArray();
				m_vertexArray.AddBuffer(buffer);

				m_isDirty = false;
			}

			return m_vertexArray;
		}

		public IndexBuffer GetIndexBuffer() { return m_indexBuffer; }

		public int IndexCount() { return m_indexBuffer.Count(); }

		public void Bind()
		{
			m_vertexArray.Bind();
			m_indexBuffer.Bind();
		}

		public void Dispose()
		{
			m_vertexArray?.Dispose();
			m_indexBuffer?.Dispose();
		}
	}
}