using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace MeteorEngine
{
	public class Panel : UIElement
	{
		private Texture2D _texture;

		public Panel(Texture2D texture)
		{
			_texture = texture;
			Transform = new RectTransform(new Vector2(0.0f, 0.0f), new Vector2(_texture.Width, _texture.Height));
		}

		public Panel(Texture2D texture, Rect rect)
		{
			_texture = texture;
			Transform = new RectTransform(rect);
		}

		public void Render()
		{
			GL.ActiveTexture(TextureUnit.Texture0);
			_texture.Apply();

			base.Render();
		}
	}
}