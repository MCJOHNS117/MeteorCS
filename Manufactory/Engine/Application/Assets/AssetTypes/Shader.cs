using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using System.Collections.Generic;

namespace MeteorEngine
{
	public class Shader : IAsset
	{
		public int Id { get; protected set; }

		private Dictionary<string, int> m_uniformCache;

		public Shader()
		{
			Id = GL.CreateProgram();
			m_uniformCache = new Dictionary<string, int>();
		}

		public void Dispose()
		{
			GL.DeleteProgram(Id);
		}

		public void Bind()
		{
			GL.UseProgram(Id);
		}

		public void Unbind()
		{
			GL.UseProgram(0);
		}

		#region Uniform Accesors
		private int GetUniformLocation(string name)
		{
			if (m_uniformCache.ContainsKey(name))
			{
				return m_uniformCache[name];
			}
			else
			{
				int id = GL.GetUniformLocation(Id, name);
				m_uniformCache.Add(name, id);
				return id;
			}
		}

		public void SetUniform(string name, float value)
		{
			GL.Uniform1(GetUniformLocation(name), value);
		}

		public void SetUniform(string name, double value)
		{
			GL.Uniform1(GetUniformLocation(name), value);
		}

		public void SetUniform(string name, int value)
		{
			GL.Uniform1(GetUniformLocation(name), value);
		}

		public void SetUniform(string name, uint value)
		{
			GL.Uniform1(GetUniformLocation(name), value);
		}
		public void SetUniform(string name, byte value)
		{
			GL.Uniform1(GetUniformLocation(name), value);
		}

		public void SetUniform(string name, short value)
		{
			GL.Uniform1(GetUniformLocation(name), value);
		}

		public void SetUniform(string name, Vector2 value)
		{
			GL.Uniform2(GetUniformLocation(name), value);
		}

		public void SetUniform(string name, Color4 color)
		{
			GL.Uniform4(GetUniformLocation(name), color);
		}

		public void SetUniform(string name, Vector3 value)
		{
			GL.Uniform3(GetUniformLocation(name), value);
		}

		public void SetUniform(string name, Vector4 value)
		{
			GL.Uniform4(GetUniformLocation(name), value);
		}

		public void SetUniform(string name, Matrix4 value, bool transpose = false)
		{
			GL.UniformMatrix4(GetUniformLocation(name), transpose, ref value);
		}

		public void SetUniform(string name, Matrix3 value, bool transpose = false)
		{
			GL.UniformMatrix3(GetUniformLocation(name), transpose, ref value);
		}

		public void SetUniform(string name, Matrix2 value, bool transpose = false)
		{
			GL.UniformMatrix2(GetUniformLocation(name), transpose, ref value);
		}
		#endregion
	}
}