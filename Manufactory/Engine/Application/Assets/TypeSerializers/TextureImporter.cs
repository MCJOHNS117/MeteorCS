using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using System.Drawing;

namespace MeteorEngine
{
	internal class TextureImporter : ITypeImporter
	{
		public string BasePath { get; }

		public TextureImporter(string basePath)
		{
			BasePath = basePath;
		}

		private class TextureData
		{
			public Color4[] colorData;
			public float[] floatData;
		}
		private class FactoryTexture2D : Texture2D
		{
			public FactoryTexture2D(int id, int width, int height, Color4[] pixels)
			{
				_id = id;
				Width = width;
				Height = height;
				Pixels = pixels;
			}
		}

		public object ImportAsset(string filename)
		{
			int width, height, _texture;
			var data = LoadTexture(BasePath + filename, out width, out height);
			GL.CreateTextures(TextureTarget.Texture2D, 1, out _texture);
			GL.TextureStorage2D(_texture, 1, SizedInternalFormat.Rgba32f, width, height);

			GL.BindTexture(TextureTarget.Texture2D, _texture);
			GL.TextureSubImage2D(_texture, 0, 0, 0, width, height, PixelFormat.Rgba, PixelType.Float, data.floatData);

			int _minMipmapLevel = 0;
			int _maxMipmapLevel = 3;

			GL.GenerateTextureMipmap(_texture);
			GL.TextureParameterI(_texture, TextureParameterName.TextureBaseLevel, ref _minMipmapLevel);
			GL.TextureParameterI(_texture, TextureParameterName.TextureMaxLevel, ref _maxMipmapLevel);
			var textureMinFilter = (int)TextureMinFilter.Nearest;
			GL.TextureParameterI(_texture, TextureParameterName.TextureMinFilter, ref textureMinFilter);
			var textureMagFilter = (int)TextureMinFilter.Nearest;
			GL.TextureParameterI(_texture, TextureParameterName.TextureMagFilter, ref textureMagFilter);

			return new FactoryTexture2D(_texture, width, height, data.colorData);
		}

		public void SerializeAsset(IAsset asset)
		{
			Texture2D texture = asset as Texture2D;

			if (texture != null)
			{
				//Do stuff
			}
		}

		private TextureData LoadTexture(string filename, out int width, out int height)
		{
			float[] r;
			TextureData data = new TextureData();
			using (var bmp = (Bitmap)Image.FromFile(filename))
			{
				width = bmp.Width;
				height = bmp.Height;

				data.floatData = new float[width * height * 4];
				data.colorData = new Color4[width * height];

				int index = 0;
				for (int y = height - 1; y >= 0; y--)
				{
					for (int x = 0; x < width; x++)
					{
						Color4 pixel = bmp.GetPixel(x, y);
						data.colorData[index / 4] = pixel;
						data.floatData[index++] = pixel.R;
						data.floatData[index++] = pixel.G;
						data.floatData[index++] = pixel.B;
						data.floatData[index++] = pixel.A;
					}
				}
			}
			return data;
		}
	}
}