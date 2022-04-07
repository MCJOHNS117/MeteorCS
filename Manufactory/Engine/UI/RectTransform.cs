using OpenTK;

namespace Meteor.Engine.UI
{
	public class RectTransform
	{
		public RectTransform Parent { get; protected set; }

		private Vector2 _position;
		public Vector2 Position { get { return _position; } set { _position = value; _isDirty = true; } }

		private Vector2 _size;
		public Vector2 Size { get { return _size; } set { _size = value; _isDirty = true; } }

		private float _rotation;
		public float Rotation { get { return _rotation; } set { _rotation = value; _isDirty = true; } }

		private bool _isDirty;
		private bool _hasParent;

		private Matrix4 _matrix;
		public Matrix4 Matrix { get { if (_isDirty) Recalculate(); return _matrix; } }

		public RectTransform()
		{
			_position = new Vector2();
			_size = new Vector2();
			_rotation = 0.0f;
			_matrix = new Matrix4();

			_isDirty = true;
			_hasParent = false;
		}

		public RectTransform(Rect rect)
		{
			_position = rect.Position;
			_size = rect.Size;
			_rotation = 0.0f;
			_matrix = new Matrix4();

			_isDirty = true;
			_hasParent = false;
		}

		public RectTransform(Vector2 position)
		{
			_position = position;
			_size = new Vector2();
			_rotation = 0.0f;
			_matrix = new Matrix4();

			_isDirty = true;
			_hasParent = false;
		}

		public RectTransform(Vector2 pos, Vector2 size)
		{
			_position = pos;
			_size = size;
			_rotation = 0.0f;
			_matrix = new Matrix4();

			_isDirty = true;
			_hasParent = false;
		}

		public RectTransform(Vector2 pos, Vector2 size, float rotation)
		{
			_position = pos;
			_size = size;
			_rotation = rotation;
			_matrix = new Matrix4();

			_isDirty = true;
			_hasParent = false;
		}

		public void SetParent(RectTransform parent)
		{
			if (null == parent)
				_hasParent = false;

			if(!_hasParent)
			{
				Parent = parent;
				_isDirty = true;
				_hasParent = true;
			}
		}

		private void Recalculate()
		{
			if (_isDirty)
			{
				_matrix = Matrix4.Identity;
				_matrix = Matrix4.CreateTranslation(new Vector3(_position.X, _position.Y, -1.0f));
				_matrix *= Matrix4.CreateScale(new Vector3(_size.X, _size.Y, 1.0f));

				if (_hasParent)
				{
					_matrix *= Parent.Matrix;
				}

				_isDirty = false;
			}
		}
	}
}