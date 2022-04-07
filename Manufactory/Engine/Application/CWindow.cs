using Meteor.Engine.Application.Assets;
using Meteor.Engine.Fonts;
using Meteor.Engine.Graphics;
using Meteor.Engine.Graphics.Renderers;
using Meteor.Engine.UI;
using Meteor.Game.Cameras;
using Meteor.Game.Data;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Meteor.Engine.Application
{
	public sealed class CWindow : GameWindow
	{
		#region Test Variables
		private List<RenderBase> _renderObjects = new List<RenderBase>();
		private List<FontRenderer> _fontRenderers = new List<FontRenderer>();
		private double _time;
		private Matrix4 _perspectiveProjection;
		private Matrix4 _orthoProjection;
		private Matrix4 _modelView;
		private string _title;

		private Shader diffuseShader;
		private Shader texturedShader;
		private Shader fontShader;
		private Shader guiShader;

		private Stopwatch m_frameTime;

		private World _world;
		private Canvas _canvas;

		private bool m_wireframe = false;
		#endregion
		public CWindow() : base(1280, 720, GraphicsMode.Default, "Meteor", GameWindowFlags.Default, DisplayDevice.Default, 4, 0, GraphicsContextFlags.Default)
		{
			GL.Enable(EnableCap.DebugOutput);

			Settings.Initialize();

			Settings.AddSettingListener("near_clip", CreateProjection);
			Settings.AddSettingListener("far_clip", CreateProjection);
			Settings.AddSettingListener("fov", CreateProjection);
			Settings.AddSettingListener("screen_width", CreateProjection);
			Settings.AddSettingListener("screen_height", CreateProjection);

			Content.Initialize();

			int width, height;
			if (Settings.GetInt("screen_width", out width) && Settings.GetInt("screen_height", out height))
			{
				Width = width;
				Height = height;
			}
			else
			{
				Width = 1280;
				Height = 720;
			}

			Title += ": OpenGL Version: " + GL.GetString(StringName.Version);
			_title = Title;
		}

		#region GameWindow Implementation
		protected override void OnResize(EventArgs e)
		{
			GL.Viewport(0, 0, Width, Height);
			CreateProjection();
		}

		protected override void OnLoad(EventArgs e)
		{
			CInput.AddKeybind(new KeyBind(Key.Escape), BindType.OnKeyDown, Exit);
			CInput.AddKeybind(new KeyBind(Key.V), BindType.OnKeyDown, () => {
				if (VSync == VSyncMode.On)
					VSync = VSyncMode.Off;
				else
					VSync = VSyncMode.On;
			});
			CInput.AddKeybind(new KeyBind(Key.F4), BindType.OnKeyDown, () =>
			{
				m_wireframe = !m_wireframe; 
				GL.PolygonMode(MaterialFace.Front, m_wireframe ? PolygonMode.Line : PolygonMode.Fill); });

			Camera.MainCamera = new FlyThroughCamera();

			bool vsync;
			if (!Settings.GetBool("vsync", out vsync))
				vsync = false;

			VSync = vsync ? VSyncMode.On : VSyncMode.Off;

			CreateProjection();

			_canvas = new Canvas(_orthoProjection);
			_canvas.AddElement(new Panel(Content.Load<Texture2D>(@"Data\Textures\UI\panelTest.png"), new Rect(new Vector2(0.5f, 0.5f), new Vector2(.25f, .5f))));

			CursorVisible = true;

			texturedShader = Content.Load<Shader>(@"Data\Shaders\textured.sh");

			fontShader = Content.Load<Shader>(@"Data\Shaders\font.sh");

			diffuseShader = Content.Load<Shader>(@"Data\Shaders\diffuse.sh");

			FontType berlinsans = new FontType(@"Data\Fonts\berlinsans.fnt", (float)Width / Height);

			TextMeshData textMeshData = berlinsans.LoadText(new GUIText("Test GUI text!!", 1.0f, berlinsans, new Vector2(10, 10), 1.0f, false));
			FontRenderer testFont = new FontRenderer(textMeshData.Vertices, fontShader.Id, Content.Load<Texture2D>(@"Data\Fonts\berlinsans.png"));
			_fontRenderers.Add(testFont);

			_world = new World(@"Data\Gameplay\Blocks.xml", @"Data\Textures\atlas.png");
			_world.Update();

			GL.PolygonMode(MaterialFace.Front, PolygonMode.Fill);
			GL.PatchParameter(PatchParameterInt.PatchVertices, 3);
			GL.Enable(EnableCap.DepthTest);

			GL.Enable(EnableCap.CullFace);
			GL.FrontFace(FrontFaceDirection.Ccw);
			GL.CullFace(CullFaceMode.Back);

			Closed += OnClosed;

			m_frameTime = new Stopwatch();
			m_frameTime.Start();
		}

		protected override void OnUpdateFrame(FrameEventArgs e)
		{
			float elapsed = m_frameTime.ElapsedMilliseconds / 1000.0f;
			m_frameTime.Restart();

			_modelView = Matrix4.Identity;

			Camera.MainCamera.Update(elapsed);
			CInput.Update();
			_world.Update();
			_canvas.Update();
		}

		protected override void OnRenderFrame(FrameEventArgs e)
		{
			_time += e.Time;
			Title = $"{_title}: (Vsync: {VSync}) FPS: {1f / e.Time:000.0}";

			Color4 backColor;
			backColor.A = 1.0f;
			backColor.R = 0.1f;
			backColor.G = 0.1f;
			backColor.B = 0.3f;
			GL.ClearColor(backColor);
			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

			//add shader attributes here
			float c = 0.0f;
			var viewMatrix = Camera.MainCamera.GetViewMatrix();
			foreach(var renderObject in _renderObjects)
			{
				var modelView = _modelView * viewMatrix;
				renderObject.Bind();
				GL.UniformMatrix4(20, false, ref _perspectiveProjection);
				GL.UniformMatrix4(21, false, ref modelView);
				renderObject.Render();
			}

			_world.Render(_perspectiveProjection, viewMatrix);

			foreach(var fontRenderer in _fontRenderers)
			{
				fontRenderer.Bind();

				GL.Uniform2(3, new Vector2());
				GL.Uniform4(GL.GetUniformLocation(fontShader.Id, "textColor"), Color4.Tomato);

				fontRenderer.Render();
			}

			_canvas.Render();
			GL.PointSize(10);
			SwapBuffers();
		}

		private void OnClosed(object sender, EventArgs eventArgs)
		{
			Exit();
		}

		public override void Exit()
		{
			foreach (var obj in _renderObjects)
				obj.Dispose();

			Settings.Save();

			base.Exit();
		}
		protected override void OnKeyDown(KeyboardKeyEventArgs e)
		{
			CInput.OnKeyDown(e.Key);
		}

		protected override void OnKeyUp(KeyboardKeyEventArgs e)
		{
			CInput.OnKeyUp(e.Key);
		}

		protected override void OnMouseDown(MouseButtonEventArgs e)
		{
			CInput.OnMouseDown(e.Button);
		}

		protected override void OnMouseUp(MouseButtonEventArgs e)
		{
			CInput.OnMouseUp(e.Button);
		}

		protected override void OnMouseMove(MouseMoveEventArgs e)
		{
			CInput.OnMouseMove(e.X, e.Y, e.XDelta, e.YDelta);
		}
		#endregion

		#region CWindow Methods
		private void CreateProjection()
		{
			var aspectRatio = (float)Width / Height;

			float near, far, fov;
			if (!Settings.GetFloat("near_clip", out near))
				near = 0.1f;

			if (!Settings.GetFloat("far_clip", out far))
				far = 100f;

			if (Settings.GetFloat("fov", out fov))
				fov = 60 * ((float)Math.PI / 180.0f);

			_perspectiveProjection = Matrix4.CreatePerspectiveFieldOfView(fov, aspectRatio, near, far);
			_orthoProjection = Matrix4.CreateOrthographic(Width, Height, 0.0f, 100.0f);
		}
		#endregion
	}
}