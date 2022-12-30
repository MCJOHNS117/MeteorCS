using OpenTK.Mathematics;

namespace MeteorEngine
{
	public class Mesh
	{
		private Vector3[] m_vertices;
		private Vector3[] m_normals;
		private Vector3[] m_tangents;
		private Vector2[] m_uvs;
		private int[] m_triangles;

		private VertexArray m_vertexArray;
		private IndexBuffer m_indexBuffer;

		private bool m_isDirty;

		public Mesh()
		{
			m_vertexArray = new VertexArray();
			m_isDirty = true;
		}

		public Vector3[] GetVerticies() { return m_vertices; }

		public Vector3[] GetNormals() { return m_normals; }

		public Vector3[] GetTangents() { return m_tangents; }

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

		public void SetTangents(Vector3[] tangents)
		{
			m_tangents = tangents;
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
				int size = 0;
				if (m_vertices != null)
					size += (m_vertices.Length * Vector3.SizeInBytes);
				if (m_normals != null)
					size += (m_normals.Length * Vector3.SizeInBytes);
				if (m_tangents != null)
					size += (m_tangents.Length * Vector3.SizeInBytes);
				if (m_uvs != null)
					size += (m_uvs.Length * Vector2.SizeInBytes);

				VertexBuffer buffer = new VertexBuffer(size);
				buffer.AddSubData(m_vertices);
				buffer.AddSubData(m_normals);
				buffer.AddSubData(m_tangents);
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
			GetVertexArray().Bind();
			m_indexBuffer.Bind();
		}

		public void Dispose()
		{
			m_vertexArray?.Dispose();
			m_indexBuffer?.Dispose();
		}
	}
}