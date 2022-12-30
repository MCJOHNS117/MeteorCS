using OpenTK.Graphics.OpenGL;

namespace MeteorEngine
{
	public class MeshRenderer : Renderer
	{
		public Model Mesh = new Model();

		public Material Material = Material.Default;

		public override void OnRender()
		{
			Material.Apply();
			Material.Shader.SetUniform("projection", Camera.MainCamera.GetProjectionMatrix());
			Material.Shader.SetUniform("view", Camera.MainCamera.GetViewMatrix());
			Material.Shader.SetUniform("model", Transform.World);

			for (int i = 0; i < Mesh.MeshCount; i++)
			{
				Mesh.GetMesh(i).Bind();
				GL.DrawElements(BeginMode.Triangles, Mesh.GetMesh(i).IndexCount(), DrawElementsType.UnsignedInt, 0);
			}
		}

		public override void Dispose()
		{
			Content.UnloadAsset(Mesh);
		}
	}
}