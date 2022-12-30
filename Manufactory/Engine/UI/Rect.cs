using OpenTK.Mathematics;

namespace MeteorEngine
{
	public class Rect
	{
		public Vector2 Position { get; set; }
		public Vector2 Size { get; set; }

		public Rect(float x, float y, float width, float height)
		{
			Position = new Vector2(x, y);
			Size = new Vector2(width, height);
		}

		public Rect(Vector2 position, Vector2 size)
		{
			Position = position;
			Size = size;
		}

		public Rect()
		{
			Position = new Vector2();
			Size = new Vector2();
		}
	}
}