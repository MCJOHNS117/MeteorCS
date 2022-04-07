using OpenTK;

namespace Meteor.Engine.Scene
{
	public class Transform
	{
		private Transform _parent;
		public Transform Parent { get { return _parent; } set { SetParent(value); } }

		private Vector3 _position;
		private Vector3 _scale;
		private Quaternion _rotation;

		private bool _isDirty;
		private bool _hasParent;

		private Matrix4 _world;
		public Matrix4 World { get { if (_isDirty) CalculateWorld(); return _world; } }

		public Vector3 Position { get => _position; set { _position = value; _isDirty = true; } }

		public Vector3 Scale { get => _scale; set { _scale = value; _isDirty = true; } }

		public Quaternion Rotation { get => _rotation; set { _rotation = value; _isDirty = true; } }

		public Vector3 Forward => CalculateForward();
		public Vector3 Up => CalculateUp();

		public Vector3 Left => CalculateLeft();

		public Transform()
		{
			Position = Vector3.Zero;
			Scale = Vector3.One;
			Rotation = Quaternion.Identity;

			_hasParent = false;
			_isDirty = true;
		}

		public Transform(Vector3 position, Vector3 scale, Quaternion rotation)
		{
			Position = position;
			Scale = scale;
			Rotation = rotation;

			_hasParent = false;
			_isDirty = true;
		}

		private void SetParent(Transform parent)
		{
			if (null == parent)
				_hasParent = false;
			else
			{
				_parent = parent;
				_isDirty = true;
				_hasParent = true;
			}
		}

		private Vector3 CalculateForward()
		{
			Vector3 result = new Vector3();
			result.X = 2 * (_rotation.X * _rotation.Z + _rotation.W * _rotation.Y);
			result.Y = 2 * (_rotation.Y * _rotation.Z - _rotation.W * _rotation.X);
			result.Z = 1 - 2 * (_rotation.X * _rotation.X + _rotation.Y * _rotation.Y);
			return result;
		}

		private Vector3 CalculateUp()
		{
			Vector3 result = new Vector3();
			result.X = 2 * (_rotation.X * _rotation.Y - _rotation.W * _rotation.Z);
			result.Y = 1 - 2 * (_rotation.X * _rotation.X + _rotation.Z * _rotation.Z);
			result.Z = 2 * (_rotation.Y * _rotation.Z + _rotation.W * _rotation.X);
			return result;
		}

		private Vector3 CalculateLeft()
		{
			Vector3 result = new Vector3();
			result.X = 1 - 2 * (_rotation.Y * _rotation.Y + _rotation.Z * _rotation.Z);
			result.Y = 2 * (_rotation.X * _rotation.Y + _rotation.W * _rotation.Z);
			result.Z = 2 * (_rotation.X * _rotation.Z - _rotation.W * _rotation.Y);
			return result;
		}

		private void CalculateWorld()
		{
			//Calculate world
			//Scale -> Rotate -> Translate
			_world = Matrix4.Identity;

			_world = Matrix4.CreateScale(_scale);
			_world *= Matrix4.CreateFromQuaternion(_rotation);
			_world *= Matrix4.CreateTranslation(_position);

			if (_hasParent)
				_world *= Parent.World;

			//Set _isDirty to false
			_isDirty = false;
		}
	}
}