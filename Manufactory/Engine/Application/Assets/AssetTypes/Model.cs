using System.Collections.Generic;

namespace MeteorEngine
{
	public class Model : IAsset
	{
		private List<Mesh> m_meshes;

		public int MeshCount => m_meshes.Count;

		public Model()
		{
			m_meshes = new List<Mesh>();
		}

		public void AddMesh(Mesh mesh)
		{
			m_meshes.Add(mesh);
		}

		public Mesh GetMesh(int index)
		{
			return m_meshes[index];
		}

		public Mesh[] GetMeshes()
		{
			return m_meshes.ToArray();
		}

		public void Dispose()
		{
			for (int i = 0; i < m_meshes.Count; i++)
			{
				m_meshes[i].Dispose();
			}
			Content.UnloadAsset(this);
		}
	}
}