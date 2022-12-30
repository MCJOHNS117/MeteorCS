using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.IO;

namespace MeteorEngine
{
	internal class MaterialImporter : ITypeImporter
	{
		private class MaterialFactory : Material
		{
			public void SetShader(Shader shader)
			{
				Shader = shader;
			}
		}

		private Dictionary<string, TextureType> textureTypeDelimiters = new Dictionary<string, TextureType>()
		{
			{ "diffuse", TextureType.Diffuse },
			{ "normal", TextureType.Normal },
			{ "specular", TextureType.Specular },
			{ "occlusion", TextureType.Occlusion },
			{ "emmision", TextureType.Emmision },
			{ "detail", TextureType.DetailMask }
		};

		private string[] materialPartitions = new string[]
		{
			"--shader--",
			"--textures--",
			"--color--"
		};

		public string BasePath { get; }

		public MaterialImporter(string basePath)
		{
			BasePath = basePath;
		}

		public object ImportAsset(string filename)
		{
			string materialSource = File.ReadAllText(BasePath + filename);

			MaterialFactory material = new MaterialFactory();

			for (int i = 0; i < materialPartitions.Length; i++)
			{
				if (materialSource.Contains(materialPartitions[i]))
				{
					int from = materialSource.IndexOf(materialPartitions[i]);
					int to = materialSource.LastIndexOf(materialPartitions[i]);

					string substring = materialSource.Substring(from + materialPartitions[i].Length, to - from - materialPartitions[i].Length);

					ProcessData(ref material, materialPartitions[i], substring);
				}
			}

			return material;
		}

		private void ProcessData(ref MaterialFactory material, string delimiter, string substring)
		{
			switch (delimiter)
			{
				case "--shader--": //Process the shader component (filename:path)
					string[] tokens = substring.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
					material.SetShader(Content.Load<Shader>(tokens[1]));
					break;
				case "--textures--":
					string[] textures = substring.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
					for (int i = 0; i < textures.Length; i += 2)
					{
						material.SetTexture(textureTypeDelimiters[textures[i]], Content.Load<Texture2D>(textures[i + 1]));
					}
					break;
				case "--color--":
					string[] colorTokens = substring.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
					Vector4 color = new Vector4();
					color.X = float.Parse(colorTokens[0]);
					color.Y = float.Parse(colorTokens[1]);
					color.Z = float.Parse(colorTokens[2]);
					color.W = float.Parse(colorTokens[3]);
					material.SetColor(color);
					break;
			}
		}
	}
}