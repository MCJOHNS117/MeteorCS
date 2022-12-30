using OpenTK.Graphics.OpenGL4;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace MeteorEngine
{
	internal class ShaderImporter : ITypeImporter
	{
		private KeyValuePair<ShaderType, string>[] shaderTypeDelimiters = {
			new KeyValuePair<ShaderType, string>(ShaderType.VertexShader, "--VERTEX--"),
			new KeyValuePair<ShaderType, string>(ShaderType.FragmentShader, "--FRAGMENT--"),
			new KeyValuePair<ShaderType, string>(ShaderType.GeometryShader,  "--GEOMETRY--"),
			new KeyValuePair<ShaderType, string>(ShaderType.ComputeShader, "--COMPUTE--"),
			new KeyValuePair<ShaderType, string>(ShaderType.TessEvaluationShader, "--TESS-EVAL--"),
			new KeyValuePair<ShaderType, string>(ShaderType.TessControlShader, "--TESS-CONTROL--")
		};

		public string BasePath { get; }

		public ShaderImporter(string basePath)
		{
			BasePath = basePath;
		}

		private class ShaderFactory : Shader
		{
			private Dictionary<ShaderType, int> shaders;

			public ShaderFactory() : base()
			{
				shaders = new Dictionary<ShaderType, int>();
			}

			public void AddShaderSource(ShaderType shaderType, string shaderSource)
			{
				var shader = GL.CreateShader(shaderType);
				GL.ShaderSource(shader, shaderSource);
				GL.CompileShader(shader);

				var info = GL.GetShaderInfoLog(shader);
				if (!string.IsNullOrWhiteSpace(info))
					Debug.WriteLine($"GL.CompileShader [{shaderType}] had info log: {info}");

				if (!shaders.ContainsKey(shaderType))
				{
					shaders.Add(shaderType, shader);
				}
				else
				{
					shaders[shaderType] = shader;
				}
			}

			public void LinkShaders()
			{
				foreach (var shader in shaders.Values)
					GL.AttachShader(Id, shader);

				GL.LinkProgram(Id);
				var info = GL.GetProgramInfoLog(Id);
				if (!string.IsNullOrWhiteSpace(info))
					Debug.WriteLine($"GL.LinkProgram had info log: {info}");

				foreach (var shader in shaders.Values)
				{
					GL.DetachShader(Id, shader);
					GL.DeleteShader(shader);
				}

				shaders.Clear();
			}
		}

		public object ImportAsset(string filename)
		{
			ShaderFactory shader = new ShaderFactory();

			string shaderSource = File.ReadAllText(BasePath + filename);

			for (int i = 0; i < shaderTypeDelimiters.Length; i++)
			{
				if (shaderSource.Contains(shaderTypeDelimiters[i].Value))
				{
					int from = shaderSource.IndexOf(shaderTypeDelimiters[i].Value);
					int to = shaderSource.LastIndexOf(shaderTypeDelimiters[i].Value);

					string sourceSubstring = shaderSource.Substring(from + shaderTypeDelimiters[i].Value.Length, to - from - shaderTypeDelimiters[i].Value.Length);

					if (sourceSubstring.Length > 0)
					{
						shader.AddShaderSource(shaderTypeDelimiters[i].Key, sourceSubstring);
					}
				}
			}

			shader.LinkShaders();

			return shader;
		}
	}
}