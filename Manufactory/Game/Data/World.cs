using Meteor.Engine.Application;
using Meteor.Engine.Application.Assets;
using Meteor.Engine.Graphics;
using Meteor.Engine.Utils;
using OpenTK;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Input;
using System;
using System.Collections.Generic;
using Meteor.Engine.Scene;

namespace Meteor.Game.Data
{
	/// <summary>
	/// The World contains the Chunks and manages generating new chunks/freeing old chunks as the player moves, as well as updating
	/// objects each frame.
	/// </summary>
	public class World
	{
		private BlockTypeCollection _blockCollection;

		private Dictionary<ChunkCoord, Chunk> _chunks;
		private Queue<ChunkCoord> _neighborQueue;
		private List<RenderBase> _renderObjects;

		private Shader _diffuseShader;
		private Texture2D _textureAtlas;

		private Mesh m_mesh;
		private Transform m_meshTransform;
		private Texture2D m_meshTexture;

		public World(string blockTypePath, string texturePackPath)
		{
			_blockCollection = new BlockTypeCollection();
			_blockCollection.LoadFromFile(blockTypePath);

			_diffuseShader = Content.Load<Shader>(@"Data\Shaders\diffuse.sh");

			_chunks = new Dictionary<ChunkCoord, Chunk>();
			_neighborQueue = new Queue<ChunkCoord>();

			_renderObjects = new List<RenderBase>();

			_textureAtlas = Content.Load<Texture2D>(texturePackPath);

			m_mesh = Content.Load<Mesh>(@"Data\Models\cube.obj");
			m_meshTexture = Content.Load<Texture2D>(@"Data\Textures\grass.jpg");
			m_meshTransform = new Transform();

			GenerateWorld();
		}

		private void GenerateWorld()
		{
			//Generate seed chunk (0,0)
			//TODO: Change seed chunk position, so loading last character position is possible
			//Add coordinates of all neighbors of chunk to queue if Abs(X) + Abs(Z) < VoxelData.ChunkDrawDistanceInChunks
			//While(chunkQueue.Count > 0)
			//	-Dequeue next chunk coords
			//	-Generate new chunk at coordinates
			//	-Add new chunks neighbors to queue
			CreateNewChunk(ChunkCoord.Zero, ChunkCoord.Zero);

			//Continue to create new chunks while there are still chunks in this queue.
			//TODO: Seperate the chunk initialization from the chunk mesh creation
			while(_neighborQueue.Count > 0)
			{
				CreateNewChunk(_neighborQueue.Dequeue(), ChunkCoord.Zero);
			}
		}

		private void CreateNewChunk(ChunkCoord coord, ChunkCoord playerChunk)
		{
			//If the _chunks container does not currently contain a chunk at this coordinate, create one.
			if(!_chunks.ContainsKey(coord))
			{
				//Create the new chunk, and initialize its voxel data and mesh
				_chunks.Add(coord, new Chunk(this, coord));

				//Check each neighbor of this chunk
				for (int i = 0; i < 4; i++)
				{
					//If the distance to the neighbor chunks coordinates is less than or equal to the chunk draw distance, add the neighbor to the queue to be created.
					if(playerChunk.Distance(coord + ChunkCoord.Neighbors[i]) <= VoxelData.ChunkDrawDistanceInChunks)
					{
						_neighborQueue.Enqueue(coord + ChunkCoord.Neighbors[i]);
					}
				}
			}
		}

		public Chunk GetChunkFromWorldPosition()
		{
			throw new NotImplementedException();
		}

		public Chunk GetChunkNeighbor(ChunkCoord origin, EDirection direction)
		{
			ChunkCoord neighbor = origin + ChunkCoord.Neighbors[(int)direction];

			return _chunks.ContainsKey(neighbor) ? _chunks[neighbor] : null;
		}

		public BlockType GetBlockType(ushort type)
		{
			return _blockCollection.ContainsKey(type) ? _blockCollection[type] : null;
		}

		public void Update()
		{
			foreach(Chunk chunk in _chunks.Values)
			{
				chunk.Update();
			}

			if(CInput.IsMousePressed(MouseButton.Left))
			{
				Ray ray = Camera.MainCamera.ScreenToWorld(CInput.MousePosition);

				//Do stuff with the ray

			}
		}

		public void Render(Matrix4 projection, Matrix4 viewMatrix)
		{

			_diffuseShader.Bind();
			_diffuseShader.SetUniform("projection", projection);
			_diffuseShader.SetUniform("view", viewMatrix);
			//_textureAtlas.Apply();

			//foreach (Chunk chunk in _chunks.Values)
			//{
			//	if (chunk.IsActive)
			//	{
			//		_diffuseShader.SetUniform("model", chunk.WorldMatrix);
			//		chunk.ChunkBufferObject.Bind();

			//		GL.DrawElements(BeginMode.Triangles, chunk.ChunkBufferObject.IndexCount, DrawElementsType.UnsignedInt, 0);
			//	}
			//}

			m_meshTexture.Apply();
			_diffuseShader.SetUniform("model", m_meshTransform.World);
			m_mesh.Bind();
			GL.DrawElements(BeginMode.Triangles, m_mesh.IndexCount(), DrawElementsType.UnsignedInt, 0);
		}
	}
}