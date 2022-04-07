using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;

namespace Meteor.Engine.Application.Assets
{
	public class Shader : IAsset
	{
		public int Id { get; protected set; }

		public Shader()
		{
			Id = GL.CreateProgram();
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
		public void SetUniform(string name, float value)
		{
			GL.Uniform1(GL.GetUniformLocation(Id, name), value);
		}

		public void SetUniform(string name, double value)
		{
			GL.Uniform1(GL.GetUniformLocation(Id, name), value);
		}

		public void SetUniform(string name, int value)
		{
			GL.Uniform1(GL.GetUniformLocation(Id, name), value);
		}

		public void SetUniform(string name, uint value)
		{
			GL.Uniform1(GL.GetUniformLocation(Id, name), value);
		}
		public void SetUniform(string name, byte value)
		{
			GL.Uniform1(GL.GetUniformLocation(Id, name), value);
		}

		public void SetUniform(string name, short value)
		{
			GL.Uniform1(GL.GetUniformLocation(Id, name), value);
		}

		public void SetUniform(string name, Vector2 value)
		{
			GL.Uniform2(GL.GetUniformLocation(Id, name), value);
		}

		public void SetUniform(string name, Color4 color)
		{
			GL.Uniform4(GL.GetUniformLocation(Id, name), color);
		}

		public void SetUniform(string name, Vector3 value)
		{
			GL.Uniform3(GL.GetUniformLocation(Id, name), value);
		}

		public void SetUniform(string name, Vector4 value)
		{
			GL.Uniform4(GL.GetUniformLocation(Id, name), value);
		}

		public void SetUniform(string name, Matrix4 value, bool transpose = false)
		{
			GL.UniformMatrix4(GL.GetUniformLocation(Id, name), transpose, ref value);
		}

		public void SetUniform(string name, Matrix3 value, bool transpose = false)
		{
			GL.UniformMatrix3(GL.GetUniformLocation(Id, name), transpose, ref value);
		}

		public void SetUniform(string name, Matrix2 value, bool transpose = false)
		{
			GL.UniformMatrix2(GL.GetUniformLocation(Id, name), transpose, ref value);
		}
		#endregion
	}
}