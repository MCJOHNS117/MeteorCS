namespace MeteorEngine
{
	public class TextMeshData
	{
		public FontVertex[] Vertices { get; protected set; }

		public int VertexCount { get { return Vertices.Length / 2; } }

		public TextMeshData(FontVertex[] vertices)
		{
			Vertices = vertices;
		}
	}
}