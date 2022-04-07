using Meteor.Engine.Graphics;
using OpenTK;
using System;
using System.Collections.Generic;

namespace Meteor.Game.Data
{
	public class Chunk
	{
		private World _world;

		public Matrix4 WorldMatrix;
		public ChunkCoord Coordinates { get; protected set; }

		private Block[,,] voxelMap = new Block[VoxelData.ChunkWidth, VoxelData.ChunkHeight, VoxelData.ChunkWidth];

		public ChunkBufferObject ChunkBufferObject { get; private set; }

		private bool _requiresUpdate;
		public bool RequiresUpdate
		{
			get { return _requiresUpdate; }
			set { _requiresUpdate = value; }
		}

		private bool _isActive;
		public bool IsActive
		{
			get { return _isActive; }
			set { _isActive = value; }
		}

		public Chunk(World world, ChunkCoord coord)
		{
			_world = world;
			Coordinates = coord;

			WorldMatrix = Matrix4.CreateTranslation(coord.X * VoxelData.ChunkWidth, 0.0f, coord.Z * VoxelData.ChunkWidth);

			_isActive = true;
			_requiresUpdate = true;

			Initialize();
		}

		private void Initialize()
		{
			Random rand = new Random(42);

			/* Populate Voxel Map with Data */
			for (int y = 0; y < VoxelData.ChunkHeight - 7; y++)
			{
				for (int x = 0; x < VoxelData.ChunkWidth; x++)
				{
					for (int z = 0; z < VoxelData.ChunkWidth; z++)
					{
						if (rand.Next(100) > 25)
							voxelMap[x, y, z] = new Block(3, 0);
						else
							voxelMap[x, y, z] = new Block(0, 0);
					}
				}
			}
		}

		private bool IsVoxelSolid(Vector3 pos)
		{
			int x = (int)Math.Floor(pos.X);
			int y = (int)Math.Floor(pos.Y);
			y = (y < 0) ? 0 : ((y > VoxelData.ChunkHeight - 1) ? VoxelData.ChunkHeight - 1 : y);
			int z = (int)Math.Floor(pos.Z);

			Block block;

			//If the voxel is in this chunk, get its block;
			if (IsVoxelInChunk(x, y, z))
				block = voxelMap[x, y, z];
			else //Else, check the neighbor chunks for this blocks neighbors.
			{
				Vector3 offset = new Vector3();
				EDirection direction = EDirection.North;

				if (x < 0)
				{
					offset = new Vector3(15f, pos.Y, pos.Z);
					direction = EDirection.West;
				}
				else if (x > 15)
				{
					offset = new Vector3(0, pos.Y, pos.Z);
					direction = EDirection.East;
				}

				if (z < 0)
				{
					offset = new Vector3(pos.X, pos.Y, 15f);
					direction = EDirection.South;
				}
				else if (z > 15)
				{
					offset = new Vector3(pos.X, pos.Y, 0f);
					direction = EDirection.North;
				}

				block = _world.GetChunkNeighbor(Coordinates, direction)?.GetVoxel(offset);
			}

			return block != null ? _world.GetBlockType(block.Type).IsSolid : false;
		}

		private bool IsVoxelInChunk(int x, int y, int z)
		{
			if (x < 0 || x > VoxelData.ChunkWidth - 1 || y < 0 || y > VoxelData.ChunkHeight - 1 || z < 0 || z > VoxelData.ChunkWidth - 1)
				return false;
			else
				return true;
		}

		private bool IsVoxelInWorld(Vector3 pos)
		{
			return (pos.X > -VoxelData.WorldSizeInVoxels && pos.X < VoxelData.WorldSizeInVoxels) 
				&& (pos.Y > 0 && pos.X < VoxelData.ChunkHeight) 
				&& (pos.Z > -VoxelData.WorldSizeInVoxels && pos.Z < VoxelData.WorldSizeInVoxels);
		}

		private Block GetVoxel(Vector3 pos)
		{
			int x = (int)Math.Floor(pos.X);
			int y = (int)Math.Floor(pos.Y);
			int z = (int)Math.Floor(pos.Z);

			if (!IsVoxelInChunk(x, y, z))
				return null;

			return voxelMap[x, y, z];
		}

		public void Update()
		{
			if (_requiresUpdate)
			{
				CreateMesh();

				_requiresUpdate = false;
			}
		}
		
		private void CreateMesh()
		{
			List<Vector3> positions = new List<Vector3>();
			List<Vector3> normals = new List<Vector3>();
			List<Vector2> uvs = new List<Vector2>();
			List<int> trianglesList = new List<int>();

			Vector3 pos = new Vector3();

			int vertexIndex = 0;

			/* Create Mesh Data */
			for (int y = 0; y < VoxelData.ChunkHeight; y++) //Loop through chunk 'layers'
			{
				for (int x = 0; x < VoxelData.ChunkWidth; x++) //Loop through chunk 'rows'
				{
					for (int z = 0; z < VoxelData.ChunkWidth; z++) //Loop through chunk 'columns'
					{
						pos.X = x;
						pos.Y = y;
						pos.Z = z;

						//Get the block from the voxel array
						Block block = GetVoxel(pos);

						//if this block is not solid, or the chunk does not contain this block exit early
						if (null == block || !_world.GetBlockType(block.Type).IsSolid)
							continue;

						//Loop for Faces
						for (int f = 0; f < 6; f++)
						{
							//Is this voxel position in the chunk && is it solid. This test will not take into consideration blocks in neighboring chunks.
							if (!IsVoxelSolid(pos + VoxelData.voxelNormals[f]) && block != null)
							{
								uvs.AddRange(GenerateTextureMapCoords(_world.GetBlockType(block.Type).TextureIds[f]));

								positions.Add(pos + VoxelData.voxelVerts[VoxelData.voxelTris[f, 0]]);
								positions.Add(pos + VoxelData.voxelVerts[VoxelData.voxelTris[f, 1]]);
								positions.Add(pos + VoxelData.voxelVerts[VoxelData.voxelTris[f, 2]]);
								positions.Add(pos + VoxelData.voxelVerts[VoxelData.voxelTris[f, 3]]);

								normals.Add(VoxelData.voxelNormals[f]);
								normals.Add(VoxelData.voxelNormals[f]);
								normals.Add(VoxelData.voxelNormals[f]);
								normals.Add(VoxelData.voxelNormals[f]);

								trianglesList.Add(vertexIndex);
								trianglesList.Add(vertexIndex + 1);
								trianglesList.Add(vertexIndex + 2);

								trianglesList.Add(vertexIndex + 2);
								trianglesList.Add(vertexIndex + 1);
								trianglesList.Add(vertexIndex + 3);
								vertexIndex += 4;
							}
						}
					}
				}
			}

			if (null != ChunkBufferObject)
				ChunkBufferObject.Cleanup();

			ChunkBufferObject = new ChunkBufferObject(positions.ToArray(), normals.ToArray(), uvs.ToArray(), trianglesList.ToArray());
		}

		/// <summary>
		/// Creates a set of UV coordinates from a given texture ID.
		/// </summary>
		/// <param name="textureId"></param>
		/// <returns></returns>
		private Vector2[] GenerateTextureMapCoords(int textureId)
		{
			float y = textureId / VoxelData.TextureAtlasDivisions;
			float x = textureId - (y * VoxelData.TextureAtlasDivisions);

			x *= VoxelData.NormalizedBlockTextureSize;
			y *= VoxelData.NormalizedBlockTextureSize;

			y = 1f - y - VoxelData.NormalizedBlockTextureSize;

			Vector2[] results = new Vector2[4];
			results[0] = new Vector2(x, y);
			results[1] = new Vector2(x, y + VoxelData.NormalizedBlockTextureSize);
			results[2] = new Vector2(x + VoxelData.NormalizedBlockTextureSize, y);
			results[3] = new Vector2(x + VoxelData.NormalizedBlockTextureSize, y + VoxelData.NormalizedBlockTextureSize);

			return results;
		}
	}

	public class ChunkCoord
	{
		public int X;
		public int Z;

		private int _hash;

		public ChunkCoord(int x, int z)
		{
			X = x;
			Z = z;

			_hash = ("" + X + ":" + Z).GetHashCode();
		}

		public override int GetHashCode()
		{
			return _hash;
		}

		public override bool Equals(object obj)
		{
			if (null == obj)
				return false;

			ChunkCoord coord = obj as ChunkCoord;

			if (null == coord)
				return false;

			return coord.X == X && coord.Z == Z;
		}

		public static readonly ChunkCoord Zero = new ChunkCoord(0, 0);

		public static readonly ChunkCoord[] Neighbors = new ChunkCoord[4]
		{
			new ChunkCoord(0, 1),	//North
			new ChunkCoord(1, 0),	//East
			new ChunkCoord(0, -1),	//South
			new ChunkCoord(-1, 0)	//West
		};

		public float Distance(ChunkCoord other)
		{
			int dX = other.X - X;
			int dZ = other.Z - Z;

			float d = (float)Math.Sqrt(dX * dX + dZ * dZ);

			return d;

			//return Math.Abs(other.X - X) + Math.Abs(other.Z - Z);
		}

		public static ChunkCoord operator+(ChunkCoord left, ChunkCoord right)
		{
			return new ChunkCoord(left.X + right.X, left.Z + right.Z);
		}

		public static ChunkCoord operator-(ChunkCoord left, ChunkCoord right)
		{
			return new ChunkCoord(left.X - right.X, left.Z - right.Z);
		}

		//public static ChunkCoord operator*(ChunkCoord left, int right)
		//{
		//	return new ChunkCoord(left.X * right, left.Z * right);
		//}
	}
}