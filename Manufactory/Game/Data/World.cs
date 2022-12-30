using MeteorEngine;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json.Serialization;
using System.Text.Json;
using MeteorEngine.Utils;

namespace MeteorGame
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

		private GameObject modelObject;
		private MeshRenderer meshRenderer;

		public World(string blockTypePath, string texturePackPath)
		{
			_blockCollection = new BlockTypeCollection();
			_blockCollection.LoadFromFile(blockTypePath);

			_diffuseShader = Content.Load<Shader>(@"diffuse.sh");

			_chunks = new Dictionary<ChunkCoord, Chunk>();
			_neighborQueue = new Queue<ChunkCoord>();

			_renderObjects = new List<RenderBase>();

			_textureAtlas = Content.Load<Texture2D>(texturePackPath);

			modelObject = GameObject.Instantiate("Belt 1", new Transform());
			meshRenderer = modelObject.AddComponent<MeshRenderer>();
			meshRenderer.Mesh = Content.Load<Model>(@"Belt1Straight.obj");
			meshRenderer.Material = new Material(_diffuseShader);
			meshRenderer.Material.SetColor(new Vector4(0.0f, 0.5f, 0.5f, 1.0f));
			meshRenderer.Material.SetTexture(TextureType.Diffuse, Content.Load<Texture2D>(@"StraightBeltRed.png"));

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
			while (_neighborQueue.Count > 0)
			{
				CreateNewChunk(_neighborQueue.Dequeue(), ChunkCoord.Zero);
			}
		}

		private void CreateNewChunk(ChunkCoord coord, ChunkCoord playerChunk)
		{
			//If the _chunks container does not currently contain a chunk at this coordinate, create one.
			if (!_chunks.ContainsKey(coord))
			{
				//Create the new chunk, and initialize its voxel data and mesh
				_chunks.Add(coord, new Chunk(this, coord));

				//Check each neighbor of this chunk
				for (int i = 0; i < 4; i++)
				{
					//If the distance to the neighbor chunks coordinates is less than or equal to the chunk draw distance, add the neighbor to the queue to be created.
					if (playerChunk.Distance(coord + ChunkCoord.Neighbors[i]) <= VoxelData.ChunkDrawDistanceInChunks)
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

		public void Update(float deltaTime)
		{
			foreach (Chunk chunk in _chunks.Values)
			{
				chunk.Update();
			}

			if (CInput.IsMousePressed(MouseButton.Left))
			{
				Ray ray = Camera.MainCamera.ScreenToWorld(CInput.MousePosition);

				//Do stuff with the ray
			}
			
			modelObject.Transform.RotateY(MathHelper.DegreesToRadians(15.0f) * deltaTime);
		}

		public void Render(Matrix4 projection, Matrix4 viewMatrix)
		{

			//_diffuseShader.Bind();
			//_diffuseShader.SetUniform("projection", projection);
			//_diffuseShader.SetUniform("view", viewMatrix);
			////_textureAtlas.Apply();

			////foreach (Chunk chunk in _chunks.Values)
			////{
			////	if (chunk.IsActive)
			////	{
			////		_diffuseShader.SetUniform("model", chunk.WorldMatrix);
			////		chunk.ChunkBufferObject.Bind();

			////		GL.DrawElements(BeginMode.Triangles, chunk.ChunkBufferObject.IndexCount, DrawElementsType.UnsignedInt, 0);
			////	}
			////}

			meshRenderer.OnRender();
		}
	}
}