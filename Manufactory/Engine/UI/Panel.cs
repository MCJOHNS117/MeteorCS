using Meteor.Engine.Application.Assets;
using OpenTK;
using OpenTK.Graphics.OpenGL4;

namespace Meteor.Engine.UI
{
	public class Panel : UIElement
	{
		private Texture2D _texture;

		public Panel(Texture2D texture)
		{
			_texture = texture;
			Transform = new RectTransform(new Vector2(_texture.Width, _texture.Height));
		}

		public Panel(Texture2D texture, Rect rect)
		{
			_texture = texture;
			Transform = new RectTransform(rect);
		}

		public override void Render()
		{
			GL.ActiveTexture(TextureUnit.Texture0);
			_texture.Apply();

			GL.BindVertexArray(_quadVAO);
			GL.DrawArrays(PrimitiveType.TriangleStrip, 0, 4);
			GL.BindVertexArray(0);
		}

		public override void Update() {  }
	}
}