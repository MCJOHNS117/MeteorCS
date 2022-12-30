using Assimp;
using Assimp.Configs;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;

namespace MeteorEngine
{
	internal class ModelImporter : ITypeImporter
	{
		public static PostProcessSteps PostProcessFlags = PostProcessSteps.Triangulate | PostProcessSteps.JoinIdenticalVertices | PostProcessSteps.FixInFacingNormals | PostProcessSteps.OptimizeMeshes | PostProcessSteps.CalculateTangentSpace;

		public string BasePath { get; }

		public ModelImporter(string basePath)
		{
			BasePath = basePath;
		}

		public object ImportAsset(string filename)
		{
			AssimpContext importer = new AssimpContext();
			importer.SetConfig(new NormalSmoothingAngleConfig(66.0f));

			Assimp.Scene scene;

			try
			{
				scene = importer.ImportFile(BasePath + filename, PostProcessFlags);

				if (scene == null || scene.RootNode == null)
					throw new Exception("MeshLoader.LoadAsset: Incorrect mesh file.");

				Model result = new Model();

				ProcessNode(scene.RootNode, scene, result);

				return result;
			}
			catch (Exception e)
			{

			}

			return null;
		}

		private void ProcessNode(Node node, Assimp.Scene scene, Model model)
		{
			for (int i = 0; i < node.MeshCount; i++)
			{
				Assimp.Mesh assimpMesh = scene.Meshes[node.MeshIndices[i]];
				model.AddMesh(ProcessMesh(assimpMesh, scene));
			}

			for (int i = 0; i < node.ChildCount; i++)
			{
				ProcessNode(node.Children[i], scene, model);
			}
		}

		private Mesh ProcessMesh(Assimp.Mesh assimpMesh, Assimp.Scene scene)
		{
			List<Vector3> vertices = new List<Vector3>();
			List<Vector3> normals = new List<Vector3>();
			List<Vector3> tangents = new List<Vector3>();
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

				vertex.X = assimpMesh.Tangents[i].X;
				vertex.Y = assimpMesh.Tangents[i].Y;
				vertex.Z = assimpMesh.Tangents[i].Z;
				tangents.Add(vertex);

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
			mesh.SetTangents(tangents.ToArray());
			mesh.SetUVs(uvs.ToArray());
			mesh.SetTriangles(triangles.ToArray());

			return mesh;
		}
	}
}