using Meteor.Engine.Application;
using Meteor.Engine.Graphics;
using OpenTK;
using OpenTK.Input;
using System;
using KeyModifiers = Meteor.Engine.Application.KeyModifiers;

namespace Meteor.Game.Cameras
{
	public class FlyThroughCamera : Camera
	{
		private float _moveSpeed = 10f;
		private float _rotateSpeed = 90f;

		private bool moveForward = false;
		private bool moveLeft = false;
		private bool moveRight = false;
		private bool moveBack = false;
		private bool sprint = false;

		public FlyThroughCamera()
		{
			CInput.AddKeybind(new KeyBind(Key.W), BindType.OnKeyDown, () => moveForward = true );
			CInput.AddKeybind(new KeyBind(Key.S), BindType.OnKeyDown, () => moveBack = true);
			CInput.AddKeybind(new KeyBind(Key.D), BindType.OnKeyDown, () => moveRight = true);
			CInput.AddKeybind(new KeyBind(Key.A), BindType.OnKeyDown, () => moveLeft = true);
			CInput.AddKeybind(new KeyBind(Key.LShift, (byte)KeyModifiers.Shift), BindType.OnKeyDown, () => SetSprint(true));

			CInput.AddKeybind(new KeyBind(Key.W), BindType.OnKeyUp, () => moveForward = false);
			CInput.AddKeybind(new KeyBind(Key.S), BindType.OnKeyUp, () => moveBack = false);
			CInput.AddKeybind(new KeyBind(Key.D), BindType.OnKeyUp, () => moveRight = false);
			CInput.AddKeybind(new KeyBind(Key.A), BindType.OnKeyUp, () => moveLeft = false);
			CInput.AddKeybind(new KeyBind(Key.LShift, (byte)KeyModifiers.Shift), BindType.OnKeyUp, () => SetSprint(false));


			_front = new Vector3(0, 0, 1);
			_up = new Vector3(0, 1, 0);
		}

		public override void Update(float deltaTime)
		{
			if(CInput.IsMousePressed(MouseButton.Right))
			{
				Vector2 mouseDelta = CInput.MouseDelta;

				//if (mouseDelta.X < 5.0f && mouseDelta.Y < 5.0f)
				//	mouseDelta = Vector2.Zero;

				_rotation.X += CInput.MouseDelta.X * _rotateSpeed * deltaTime;
				_rotation.Y += CInput.MouseDelta.Y * _rotateSpeed * deltaTime;

				if (_rotation.Y > 89.0f)
					_rotation.Y = 89.0f;
				if (_rotation.Y < -89.0f)
					_rotation.Y = -89.0f;
			}

			float speed = _moveSpeed;
			if (sprint)
				speed *= 2.0f;

			if (moveForward)
			{
				_position += speed * _front * deltaTime;
			}

			if (moveBack)
			{
				_position -= speed * _front * deltaTime;
			}

			if (moveLeft)
			{
				_position -= Vector3.Normalize(Vector3.Cross(_front, _up)) * speed * deltaTime;
			}

			if (moveRight)
			{
				_position += Vector3.Normalize(Vector3.Cross(_front, _up)) * speed * deltaTime;
			}
			
			_front.X = (float)Math.Cos(MathHelper.DegreesToRadians(_rotation.Y)) * (float)Math.Cos(MathHelper.DegreesToRadians(_rotation.X));
			_front.Y = (float)Math.Sin(MathHelper.DegreesToRadians(_rotation.Y));
			_front.Z = (float)Math.Cos(MathHelper.DegreesToRadians(_rotation.Y)) * (float)Math.Sin(MathHelper.DegreesToRadians(_rotation.X));
			_front.Normalize();
		}

		protected override void CalculateViewMatrix()
		{
			_viewMatrix = Matrix4.LookAt(_position, _position + _front, _up);

			//Vector3 transPos = _position;
			//transPos *= -1.0f;
			//_viewMatrix = Matrix4.CreateTranslation(transPos);
			//_viewMatrix *= Matrix4.CreateRotationY(MathHelper.DegreesToRadians(_rotation.X));
			//_viewMatrix *= Matrix4.CreateRotationX(MathHelper.DegreesToRadians(_rotation.Y));
		}

		private void SetSprint(bool sp)
		{
			sprint = sp;
		}
	}
}