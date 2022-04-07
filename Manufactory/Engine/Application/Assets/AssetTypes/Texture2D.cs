using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;

namespace Meteor.Engine.Application.Assets
{
	public class Texture2D : IAsset
	{
		protected int _id;

		public int Width { get; protected set; }
		public int Height { get; protected set; }

		public Color4[] Pixels { get; protected set; }

		public void SetPixel(int x, int y, Color4 color)
		{

		}

		public Color4 GetPixel(int x, int y)
		{
			return new Color4();
		}

		public void Apply()
		{
			GL.BindTexture(TextureTarget.Texture2D, 0);
			GL.BindTexture(TextureTarget.Texture2D, _id);
		}

		public void Dispose()
		{
			Content.UnloadAsset(this);
			GL.DeleteTexture(_id);
		}
	}
}