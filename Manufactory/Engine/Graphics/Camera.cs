using Meteor.Engine.Application;
using Meteor.Engine.Utils;
using OpenTK;
using System;

namespace Meteor.Engine.Graphics
{
	public abstract class Camera
	{
		public enum EProjectionType { Perspective, Orthogonal };

		public static Camera MainCamera { get; set; }

		protected Matrix4 _viewMatrix;

		protected Matrix4 _perspectiveProjectionMatrix;
		protected Matrix4 _orthoProjectionMatrix;

		protected int _screenWidth, _screenHeight;
		protected float _nearClip, _farClip, _fov;

		protected Vector3 _position;
		protected Vector3 _rotation;
		protected Vector3 _front;
		protected Vector3 _up;

		public Camera()
		{
			_viewMatrix = Matrix4.Identity;
			_perspectiveProjectionMatrix = Matrix4.Identity;
			_orthoProjectionMatrix = Matrix4.Identity;

			CreateProjectionMatrix();
		}

		public abstract void Update(float deltaTime);

		public Matrix4 GetViewMatrix()
		{
			CalculateViewMatrix();
			return _viewMatrix;
		}

		public Matrix4 GetProjectionMatrix(EProjectionType eProjectionType = EProjectionType.Perspective)
		{
			switch (eProjectionType)
			{
				case EProjectionType.Perspective:
					return _perspectiveProjectionMatrix;
				case EProjectionType.Orthogonal:
					return _orthoProjectionMatrix;
				default:
					return _perspectiveProjectionMatrix;
			}
		}


		protected abstract void CalculateViewMatrix();
		
		public void CreateProjectionMatrix()
		{
			Settings.GetInt("screen_width", out _screenWidth);

			Settings.GetInt("screen_height", out _screenHeight);

			var aspectRatio = (float)_screenWidth / _screenHeight;

			if (!Settings.GetFloat("near_clip", out _nearClip))
				_nearClip = 0.1f;

			if (!Settings.GetFloat("far_clip", out _farClip))
				_farClip = 100f;

			if (Settings.GetFloat("fov", out _fov))
				_fov = 60 * ((float)Math.PI / 180.0f);

			_perspectiveProjectionMatrix = Matrix4.CreatePerspectiveFieldOfView(_fov, aspectRatio, _nearClip, _farClip);
			_orthoProjectionMatrix = Matrix4.CreateOrthographic(_screenWidth, _screenHeight, 0.0f, 100.0f);
		}

		public Ray ScreenToWorld(Vector2 screenCoordinates)
		{
			//Convert from Viewport Space to Normalized Device Space
			float x = (2f * screenCoordinates.X) / _screenWidth - 1f;
			float y = 1f - (2f * screenCoordinates.Y) / _screenHeight;

			//Convert from Normalized Device Space to Homogenous Clip Space
			Vector4 clipCoordinates = new Vector4(x, y, -1f, 1f);

			//Convert from Homogenous Clip Space to Eye Space
			Vector4 eyeCoordinates = clipCoordinates * _perspectiveProjectionMatrix.Inverted();

			//Convert from Eye Space to World Space
			Vector4 worldCoordinates = eyeCoordinates * _viewMatrix.Inverted();

			//Convert the 4D Vector to a 3D vector and normalize. This is our ray direction
			return new Ray(_position, new Vector3(worldCoordinates.X, worldCoordinates.Y, worldCoordinates.Z).Normalized());
		}

		//public Vector2 WorldToScreen(Vector3 position)
		//{
		//	throw new NotImplementedException();
		//}
	}
}