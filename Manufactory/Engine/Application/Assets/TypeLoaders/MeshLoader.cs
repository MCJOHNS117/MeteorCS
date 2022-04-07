using System;
using System.Collections.Generic;
using Assimp;
using Assimp.Configs;
using OpenTK;

namespace Meteor.Engine.Application.Assets.TypeLoaders
{
	public class MeshLoader : ITypeLoader
	{
		public object LoadAsset(string path)
		{
			AssimpContext importer = new AssimpContext();
			importer.SetConfig(new NormalSmoothingAngleConfig(66.0f));

			Assimp.Scene scene;

			try
			{
				scene = importer.ImportFile(path, PostProcessSteps.Triangulate);

				if (scene == null || scene.RootNode == null)
					throw new Exception("MeshLoader.LoadAsset: Incorrect mesh file.");

				Mesh mesh = ProcessNode(scene.RootNode, scene);

				return mesh;
			}
			catch (Exception e)
			{
				
			}

			return null;
		}

		private Mesh ProcessNode(Node node, Assimp.Scene scene)
		{
			return ProcessMesh(scene.Meshes[0], scene);

			return null;

			//for (int i = 0; i < node.MeshCount; i++)
			//{
			//	Assimp.Mesh assimpMesh = scene.Meshes[node.MeshIndices[i]];
			//	ProcessMesh(assimpMesh, scene);
			//}

			//for (int i = 0; i < node.ChildCount; i++)
			//{
			//	ProcessNode(node.Children[i], scene, mesh);
			//}
		}

		private Mesh ProcessMesh(Assimp.Mesh assimpMesh, Assimp.Scene scene)
		{
			List<Vector3> vertices = new List<Vector3>();
			List<Vector3> normals = new List<Vector3>();
			List<Vector2> uvs = new List<Vector2>();
			List<int> triangles = new List<int>();

			Vector3 vertex = new Vector3();
			Vector2 uv = new Vector2();
			for (int i = 0; i < assimpMesh.VertexCount; i++)
			{
				vertex.X = assimpMesh.Vertices[i].X;
				vertex.Y = assimpMesh.Vertices[i].Y;
				vertex.Z = assimpMesh.Vertices[i].Z;
				vertices.Add(vertex);

				vertex.X = assimpMesh.Normals[i].X;
				vertex.Y = assimpMesh.Normals[i].Y;
				vertex.Z = assimpMesh.Normals[i].Z;
				normals.Add(vertex);

				if (assimpMesh.TextureCoordinateChannels[0].Count > 0)
				{
					uv.X = assimpMesh.TextureCoordinateChannels[0][i].X;
					uv.Y = assimpMesh.TextureCoordinateChannels[0][i].Y;
					uvs.Add(uv);
				}
				else
				{
					uvs.Add(Vector2.Zero);
				}
			}

			for (int i = 0; i < assimpMesh.FaceCount; i++)
			{
				Assimp.Face face = assimpMesh.Faces[i];
				for (int j = 0; j < face.IndexCount; j++)
				{
					triangles.Add(face.Indices[j]);
				}
			}

			Mesh mesh = new Mesh();
			mesh.SetVertices(vertices.ToArray());
			mesh.SetNormals(normals.ToArray());
			mesh.SetUVs(uvs.ToArray());
			mesh.SetTriangles(triangles.ToArray());

			return mesh;
		}
	}
}