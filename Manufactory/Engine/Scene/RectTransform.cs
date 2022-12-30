using OpenTK.Mathematics;
using System;

namespace MeteorEngine
{
    public class RectTransform : Transform
    {
        internal Rect mRectanlge;
        internal float mRotation;

        public RectTransform(float x, float y, float width, float height)
        {
            mRectanlge = new Rect(x, y, width, height);
        }

		public RectTransform(Vector2 position, Vector2 size)
		{
			mRectanlge.Position = position;
			mRectanlge.Size = size;
			_isDirty = true;
		}

		public RectTransform(Rect rect)
        {
            mRectanlge = rect;
			_isDirty = true;
		}
        

        internal RectTransform(Guid id)
        {
            m_instanceId = id;
			_isDirty = true;
		}

        public void SetPosition(Vector2 position)
        {
            mRectanlge.Position = position;
			_isDirty = true;
		}

        public void Translate(Vector2 translation)
        {
            mRectanlge.Position += translation;
			_isDirty = true;
		}

        public void SetSize(Vector2 size)
        {
            mRectanlge.Size = size;
			_isDirty = true;
		}

        public new void Scale(Vector2 scale)
        {
            mRectanlge.Size *= scale;
			_isDirty = true;
		}

        public void SetRotation(float angleRadians)
        {
            mRotation = angleRadians;
			_isDirty = true;
		}

        public void Rotate(float angleRadians)
        {
            mRotation += angleRadians;
			_isDirty = true;
		}


        protected override void CalculateWorld()
        {
            _world = Matrix4.Identity;

            _world = Matrix4.CreateTranslation(mRectanlge.Position.X, mRectanlge.Position.Y, 0.0f);

            _world *= Matrix4.CreateTranslation(mRectanlge.Size.X * 0.5f, mRectanlge.Size.Y * 0.5f, 0.0f);
            _world *= Matrix4.CreateRotationZ(mRotation);
			_world *= Matrix4.CreateTranslation(mRectanlge.Size.X * -0.5f, mRectanlge.Size.Y * -0.5f, 0.0f);

            _world *= Matrix4.CreateScale(new Vector3(mRectanlge.Size) { Z = 1.0f });

            if (_parent != null)
                _world *= _parent.World;

            _isDirty = false;
		}
    }
}