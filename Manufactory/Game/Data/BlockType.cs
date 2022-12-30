using System;

namespace MeteorGame
{
	[Serializable]
	public class BlockType
	{
		public ushort Type { get; set; }
		public bool IsSolid { get; set; }

		public string Name { get; set; }

		public int[] TextureIds { get; set; }
		//Back Face
		//Front Face
		//Top Face
		//Bottom Face
		//Left Face
		//Right Face

		public BlockType(ushort type, bool isSolid, string name, int[] textureIds)
		{
			Type = type;
			IsSolid = isSolid;
			Name = name;
			TextureIds = textureIds;
		}

		public BlockType()
		{

		}
	}
}