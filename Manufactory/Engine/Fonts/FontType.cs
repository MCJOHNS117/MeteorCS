namespace MeteorEngine
{
	public class FontType
	{
		private TextMeshCreator _loader;

		public FontType(string filename, float aspectRatio)
		{
			_loader = new TextMeshCreator(filename, aspectRatio);
		}

		public TextMeshData LoadText(GUIText text)
		{
			return _loader.CreateTextMesh(text);
		}
	}
}
