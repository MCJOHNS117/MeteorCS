using OpenTK.Mathematics;

namespace MeteorEngine
{
	public class Ray
	{
		Vector3 _origin;
		Vector3 _direction;

		public Ray(Vector3 origin, Vector3 direction)
		{
			_origin = origin;
			_direction = direction;
		}

		public Vector3 Origin { get => _origin; protected set => _origin = value; }
		public Vector3 Direction { get => _direction; protected set => _direction = value; }
	}
}