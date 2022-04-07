using OpenTK;
using OpenTK.Graphics;

namespace Meteor.Engine.Fonts
{
	public class GUIText
	{
		public string Text { get; protected set; }
		public float FontSize { get; protected set; }

		public int VertexArrayId { get; protected set; }
		public int VertexCount { get; protected set; }
		public Color4 Color { get; set; }

		public Vector2 Position;
		public float MaxLineSize { get; protected set; }
		public int LineCount { get; set; }

		public FontType Font { get; protected set; }

		public bool Centered { get; protected set; }

		public GUIText(string text, float fontSize, FontType font, Vector2 position, float maxLineLength, bool centered)
		{
			Text = text;
			FontSize = fontSize;

			Font = font;
			Position = position;
			MaxLineSize = maxLineLength;
			Centered = centered;
		}

		public void SetMeshInfo(int vertexArray, int vertexCount)
		{
			VertexArrayId = vertexArray;
			VertexCount = vertexCount;
		}
	}
}