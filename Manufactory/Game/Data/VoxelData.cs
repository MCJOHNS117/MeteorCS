using OpenTK.Mathematics;

namespace MeteorGame
{
	public static class VoxelData
	{
		public static readonly int ChunkWidth = 16;
		public static readonly int ChunkHeight = 16;
		public static readonly int WorldSizeInChunks = 5;
		public static readonly int WorldSizeInVoxels = ChunkWidth * WorldSizeInChunks;
		public static readonly int ChunkDrawDistanceInChunks = 2;
		public static readonly int TextureAtlasDivisions = 32;

		public static float NormalizedBlockTextureSize
		{
			get { return 1.0f / (float)TextureAtlasDivisions; }
		}

		public static readonly Vector3[] voxelVerts = new Vector3[8]
		{
			new Vector3(0, 0, 0),
			new Vector3(1, 0, 0),
			new Vector3(1, 1, 0),
			new Vector3(0, 1, 0),

			new Vector3(0, 0, 1),
			new Vector3(1, 0, 1),
			new Vector3(1, 1, 1),
			new Vector3(0, 1, 1)
		};

		public static readonly int[,] voxelTris = new int[6, 4]
		{
			{0, 3, 1, 2 },	//Back Face
			{5, 6, 4, 7 },	//Front Face
			{3, 7, 2, 6 },	//Top Face
			{1, 5, 0, 4 },	//Bottom Face
			{4, 7, 0, 3 },	//Left Face
			{1, 2, 5, 6 }	//Right Face
		};

		public static readonly Vector3[] voxelNormals = new Vector3[6]
		{
			new Vector3(0, 0, -1),	//Back Face
			new Vector3(0, 0, 1),	//Front Face
			new Vector3(0, 1, 0),	//Top Face
			new Vector3(0, -1, 0),	//Bottom Face
			new Vector3(-1, 0, 0),	//Left Face
			new Vector3(1, 0, 0),	//Right Face
		};

		public static readonly Vector2[] voxelUVs = new Vector2[4]
		{
			new Vector2(0, 0),
			new Vector2(0, 1),
			new Vector2(1, 0),
			new Vector2(1, 1)
		};
	}
}