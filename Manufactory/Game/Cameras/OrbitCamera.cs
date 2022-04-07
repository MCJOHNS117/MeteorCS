using Meteor.Engine.Graphics;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Meteor.Game.Cameras
{
	public class OrbitCamera : Camera
	{
		private float _radius = 10.0f;
		private float _rotation = 0;
		//Radians per second
		private float _speed;
		private Vector3 _lookAt;
		private bool _rotate;

		public OrbitCamera(Vector3 lookAt, float radius, float speed)
		{
			_radius = radius;
			_lookAt = lookAt;
			_speed = speed;
			_rotate = true;
		}

		public void Toggle()
		{
			_rotate = !_rotate;
		}

		public void SetRadius(float radius)
		{
			_radius = radius;
		}

		public override void Update(float deltaTime)
		{
			if(_rotate)
				_rotation += _speed * deltaTime;
		}

		protected override void CalculateViewMatrix()
		{
			Vector3 position = new Vector3();

			position.X = _lookAt.X + (_radius * (float)Math.Cos(_rotation));
			position.Y = 0f;
			position.Z = _lookAt.Z + (_radius * (float)Math.Sin(_rotation));

			_viewMatrix = Matrix4.LookAt(position, _lookAt, Vector3.UnitY);
		}
	}
}