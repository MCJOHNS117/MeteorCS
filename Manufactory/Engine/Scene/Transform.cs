using OpenTK.Mathematics;
using System;
using System.Collections.Generic;

namespace MeteorEngine
{
	public class Transform : Component
	{
		protected Transform? _parent;
		public Transform? Parent { get { return _parent; } set { SetParent(value); } }

		protected List<Transform> m_children = new List<Transform>();
		 
		public int ChildCount { get { return m_children.Count; } }

		private Vector3 _position;
		private Vector3 _scale;
		private Quaternion _rotation;

		protected bool _isDirty;

		protected Matrix4 _world;

		public Matrix4 World { get { if (_isDirty) CalculateWorld(); return _world; } }

		public Vector3 Position { get => _position; set { _position = value; _isDirty = true; } }

		public Vector3 Scale { get => _scale; set { _scale = value; _isDirty = true; } }

		public Quaternion Rotation { get => _rotation; set { _rotation = value; _isDirty = true; } }

		public Vector3 Forward => CalculateForward();
		public Vector3 Up => CalculateUp();
		public Vector3 Left => CalculateLeft();

		public Transform() : base()
		{
			Position = Vector3.Zero;
			Scale = Vector3.One;
			Rotation = Quaternion.Identity;

			_isDirty = true;
		}

		internal Transform(Guid id)
		{
			m_instanceId = id;

			Position = Vector3.Zero;
			Scale = Vector3.One;
			Rotation = Quaternion.Identity;

			_isDirty = true;
		}

		public Transform(Vector3 position, Vector3 scale, Quaternion rotation) : base()
		{
			Position = position;
			Scale = scale;
			Rotation = rotation;

			_isDirty = true;
		}

		private void SetParent(Transform parent)
		{
			_parent = parent;
			_isDirty = true;

		}

		public void Translate(Vector3 translation)
		{
			Position += translation;
		}

		public void Translate(float x, float y, float z)
		{
			Translate(new Vector3(x, y, z));
		}

		public void ApplyScale(Vector3 scale)
		{
			Scale *= scale;
		}

		public void ApplyScale(float uniform)
		{
			Scale *= uniform;
		}

		public void RotateX(float rotation)
		{
			Rotation *= Quaternion.FromEulerAngles(rotation, 0.0f, 0.0f);
		}

		public void RotateY(float rotation)
		{
			Rotation *= Quaternion.FromEulerAngles(0.0f, rotation, 0.0f);
		}

		public void RotateZ(float rotation)
		{
			Rotation *= Quaternion.FromEulerAngles(0.0f, 0.0f, rotation);
		}

		public void Rotate(Vector3 rotationAngles)
		{
			Rotation *= Quaternion.FromEulerAngles(rotationAngles);
		}

		public void Rotate(Vector3 axis, float angle)
		{
			Rotation *= Quaternion.FromAxisAngle(axis, angle);
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

		protected virtual void CalculateWorld()
		{
			//Calculate world
			//Scale -> Rotate -> Translate
			_world = Matrix4.Identity;

			_world = Matrix4.CreateTranslation(_position);
			_world *= Matrix4.CreateFromQuaternion(_rotation);
			_world *= Matrix4.CreateScale(_scale);

			if (Parent != null)
				_world *= Parent.World;

			//Set _isDirty to false
			_isDirty = false;
		}

		public void AddChild(Transform child)
		{
			if(child != this)
			{
				m_children.Add(child);
				child.SetParent(this);
			}
		}

		public void RemoveChild(Transform child)
		{
			if(child != this)
			{
				m_children.Remove(child);
				child.SetParent(null);
			}
		}

		public Transform GetChild(int index)
		{
			if(index < 0 || index > m_children.Count)
			{
				return null;
			}

			return m_children[index];
		}

		public override void Dispose()
		{
			
		}
	}
}