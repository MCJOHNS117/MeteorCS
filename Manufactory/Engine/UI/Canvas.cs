using System.Collections.Generic;
using Meteor.Engine.Application.Assets;
using OpenTK.Graphics.OpenGL4;
using OpenTK;

namespace Meteor.Engine.UI
{
	public class Canvas
	{
		private RectTransform _root = new RectTransform();

		private Shader _shader;

		private List<UIElement> _elements;

		private Matrix4 _orthoProjection;

		public Canvas(Matrix4 projection)
		{
			_shader = Content.Load<Shader>(@"Data\Shaders\gui.sh");

			_elements = new List<UIElement>();

			_orthoProjection = projection;
		}

		public void AddElement(UIElement element)
		{
			if (!_elements.Contains(element))
				_elements.Add(element);
		}

		public void Update()
		{
			foreach(UIElement element in _elements)
			{
				element.Update();
			}
		}

		public void Render()
		{
			//Pre-render setup
			GL.UseProgram(_shader.Id);

			Matrix4 model = Matrix4.Identity;
			GL.UniformMatrix4(21, false, ref _orthoProjection);

			//Render loop
			foreach(UIElement element in _elements)
			{
				model = element.Transform.Matrix;
				GL.UniformMatrix4(20, false, ref model);

				element.Render();
			}

			//Post-render clean-up
		}
	}
}