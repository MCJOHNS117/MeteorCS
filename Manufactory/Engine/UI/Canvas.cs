using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace MeteorEngine
{
	public class Canvas : Component
	{
		private RectTransform _root;

		private Shader _shader;

		private Matrix4 _orthoProjection;

		public Canvas(float x, float y, float width, float height)
		{
			_root = new RectTransform(x, y, width, height);

			_shader = Content.Load<Shader>(@"gui.sh");

			_orthoProjection = Matrix4.CreateOrthographic(width, height, 0.0f, 100.0f);
		}

		public void Render()
		{
			////Pre-render setup
			//GL.UseProgram(_shader.Id);

			//GL.Enable(EnableCap.Blend);
			//GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
			//GL.Disable(EnableCap.DepthTest);

			//Matrix4 model = Matrix4.Identity;
			//GL.UniformMatrix4(21, false, ref _orthoProjection);

			////Render loop
			//foreach (UIElement element in _elements)
			//{
			//	model = element.Transform.World;
			//	GL.UniformMatrix4(20, false, ref model);

			//	element.Render();
			//}

			////Post-render clean-up
			//GL.Enable(EnableCap.DepthTest);
		}
	}
}