namespace Meteor.Engine.Fonts
{
	public class Character
	{
		public int Id { get; private set; }

		public float X { get; private set; }

		public float Y { get; private set; }

		public float Width { get; private set; }

		public float Height { get; private set; }

		public float XOffset { get; private set; }

		public float YOffset { get; private set; }

		public float sizeX { get; private set; }

		public float sizeY { get; private set; }

		public float XAdvance { get; private set; }

		public Character(int id, float x, float y, float width, float height,
			float xoffset, float yoffset, float sizex, float sizey, float xadvance)
		{
			Id = id;
			X = x;
			Y = y;
			Width = width;
			Height = height;
			XOffset = xoffset;
			YOffset = yoffset;
			sizeX = sizex;
			sizeY = sizey;
			XAdvance = xadvance;
		}
	}
}
