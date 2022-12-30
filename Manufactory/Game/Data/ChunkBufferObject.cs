using MeteorEngine;
using OpenTK.Mathematics;

namespace MeteorGame
{
	public class ChunkBufferObject
	{
		public int IndexCount { get { return m_mesh.IndexCount(); } }

		private Mesh m_mesh;

		public ChunkBufferObject(Vector3[] positions, Vector3[] normals, Vector2[] uvs, int[] triangles)
		{
			SetData(positions, normals, uvs, triangles);
		}

		private void SetData(Vector3[] positions, Vector3[] normals, Vector2[] uvs, int[] triangles)
		{
			m_mesh = new Mesh();

			m_mesh.SetVertices(positions);
			m_mesh.SetNormals(normals);
			m_mesh.SetUVs(uvs);
			m_mesh.SetTriangles(triangles);
		}

		public void Bind()
		{
			m_mesh.GetVertexArray().Bind();
			m_mesh.GetIndexBuffer().Bind();
		}

		public void Cleanup()
		{
			//TODO: Add buffer cleanup stuff here for when a ChunkBufferObject is reassigned or deleted.
		}
	}
}