using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using System.Collections.Generic;

namespace MeteorEngine
{
	public enum TextureType : int
	{
		Diffuse = TextureUnit.Texture0,
		Normal = TextureUnit.Texture1,
		Specular = TextureUnit.Texture2,
		Occlusion = TextureUnit.Texture3,
		Emmision = TextureUnit.Texture4,
		DetailMask = TextureUnit.Texture5,
	}

	public class Material : IAsset
	{
		public static Material Default = new Material(Content.Load<Shader>(@"diffuse.sh"));

		public Shader Shader { get; protected set; }

		private Dictionary<TextureType, Texture2D> m_textures;

		private Vector4 m_color;

		public Material(Shader shader)
		{
			Shader = shader;
			m_textures = new Dictionary<TextureType, Texture2D>();
			m_color = new Vector4(1.0f, 1.0f, 1.0f, 1.0f);
		}

		public Material()
		{

		}

		public void SetColor(Vector4 color)
		{
			m_color = color;
		}

		public void SetTexture(TextureType textureType, Texture2D texture)
		{
			m_textures[textureType] = texture;
		}

		public void Apply()
		{
			Shader.Bind();

			Shader.SetUniform("color", m_color);

			foreach (TextureType textureType in m_textures.Keys)
			{
				GL.ActiveTexture(TextureUnit.Texture0 + (int)textureType);
				m_textures[textureType].Apply();
			}
		}

		public void Dispose()
		{
			Content.UnloadAsset(this);
		}
	}
}