using MeteorGame;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace MeteorEngine
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
		public CWindow() : base(new GameWindowSettings() { RenderFrequency = 0.0, UpdateFrequency = 90.0 }, new NativeWindowSettings() { Size = (1280, 720), Title = "OpenTK 4.7 Test", API = ContextAPI.OpenGL, APIVersion = new System.Version(4, 6) })
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
				Size = (width, height);
			}
			else
			{
				Size = (1280, 720);
			}

			Title += ": OpenGL Version: " + GL.GetString(StringName.Version);
			_title = Title;
		}

		#region GameWindow Implementation
		protected override void OnResize(ResizeEventArgs e)
		{
			GL.Viewport(0, 0, e.Width, e.Height);
			CreateProjection();
		}

		protected override void OnLoad()
		{
			CInput.AddKeybind(new KeyBind(Keys.Escape), BindType.OnKeyDown, Close);
			CInput.AddKeybind(new KeyBind(Keys.V), BindType.OnKeyDown, () =>
			{
				if (VSync == VSyncMode.On)
					VSync = VSyncMode.Off;
				else
					VSync = VSyncMode.On;
			});
			CInput.AddKeybind(new KeyBind(Keys.F4), BindType.OnKeyDown, () =>
			{
				m_wireframe = !m_wireframe;
				GL.PolygonMode(MaterialFace.FrontAndBack, m_wireframe ? PolygonMode.Line : PolygonMode.Fill);
			});

			Camera.MainCamera = new FlyThroughCamera();

			bool vsync;
			if (!Settings.GetBool("vsync", out vsync))
				vsync = false;

			VSync = vsync ? VSyncMode.On : VSyncMode.Off;

			CreateProjection();

			_canvas = new Canvas(0.0f, 0.0f, Size.X, Size.Y);
			_canvas.AddElement(new Panel(Content.Load<Texture2D>(@"UI\panelTest.png"), new Rect(new Vector2(0.5f, 0.5f), new Vector2(.25f, .5f))));

			CursorVisible = true;

			texturedShader = Content.Load<Shader>(@"textured.sh");

			fontShader = Content.Load<Shader>(@"font.sh");

			diffuseShader = Content.Load<Shader>(@"diffuse.sh");

			FontType berlinsans = new FontType(@"Data\\Fonts\\berlinsans.fnt", (float)Size.X / Size.Y);

			//TextMeshData textMeshData = berlinsans.LoadText(new GUIText("Test GUI text!!", 1.0f, berlinsans, new Vector2(10, 10), 1.0f, false));
			//FontRenderer testFont = new FontRenderer(textMeshData.Vertices, fontShader.Id, Content.Load<Texture2D>(@"Data\Fonts\berlinsans.png"));
			//_fontRenderers.Add(testFont);

			_world = new World(@"Data\Gameplay\Blocks.xml", @"atlas.png");

			GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
			GL.PatchParameter(PatchParameterInt.PatchVertices, 3);
			GL.Enable(EnableCap.DepthTest);

			GL.Enable(EnableCap.CullFace);
			GL.FrontFace(FrontFaceDirection.Ccw);
			GL.CullFace(CullFaceMode.Back);

			m_frameTime = new Stopwatch();
			m_frameTime.Start();

			base.OnLoad();
		}

		protected override void OnUpdateFrame(FrameEventArgs e)
		{
			float elapsed = m_frameTime.ElapsedMilliseconds / 1000.0f;
			m_frameTime.Restart();

			_modelView = Matrix4.Identity;

			Camera.MainCamera.Update(elapsed);
			CInput.Update();
			_world.Update(elapsed);
			_canvas.Update(elapsed);

			base.OnUpdateFrame(e);
		}

		protected override void OnRenderFrame(FrameEventArgs e)
		{
			base.OnRenderFrame(e);

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
			foreach (var renderObject in _renderObjects)
			{
				var modelView = _modelView * viewMatrix;
				renderObject.Bind();
				GL.UniformMatrix4(20, false, ref _perspectiveProjection);
				GL.UniformMatrix4(21, false, ref modelView);
				renderObject.Render();
			}

			_world.Render(_perspectiveProjection, viewMatrix);

			foreach (var fontRenderer in _fontRenderers)
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

		public override void Close()
		{
			foreach (var obj in _renderObjects)
				obj.Dispose();

			Settings.Save();

			base.Close();
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
			CInput.OnMouseMove(e.X, e.Y, e.DeltaX, e.DeltaY);
		}
		#endregion

		#region CWindow Methods
		private void CreateProjection()
		{
			var aspectRatio = (float)Size.X / Size.Y;

			float near, far, fov;
			if (!Settings.GetFloat("near_clip", out near))
				near = 0.1f;

			if (!Settings.GetFloat("far_clip", out far))
				far = 100f;

			if (Settings.GetFloat("fov", out fov))
				fov = 60 * ((float)Math.PI / 180.0f);

			_perspectiveProjection = Matrix4.CreatePerspectiveFieldOfView(fov, aspectRatio, near, far);
		}
		#endregion
	}
}