using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Meteor.Engine.Scene
{
	/// <summary>
	/// Represents an axis aligned bounding box
	/// </summary>
	public class Bounds
	{
		public Vector3 Center { get; set; }
		public Vector3 Extents { get; protected set; }
		public Vector3 Max { get; protected set; }
		public Vector3 Min { get; protected set; }
		public Vector3 Size { get; protected set; }

		public Bounds(Vector3 center, Vector3 extents)
		{
			Center = center;
			Extents = extents;
			Max = Center + Extents;
			Min = Center - Extents;
			Size = Extents * 2.0f;
		}

		public Vector3 ClosestPoint(Vector3 point)
		{
			Vector3 max = Max;	//Cache to prevent unneccesary calculations
			Vector3 min = Min;

			Vector3 result = point; //Set to origin point to remove unnecesary else statement

			//Calculate X
			if (point.X > max.X)
				point.X = max.X;
			else if (point.X < min.X)
				point.X = min.X;

			//Calculate Y
			if (point.Y > max.Y)
				point.Y = max.Y;
			else if (point.Y < min.Y)
				point.Y = min.Y;

			//Calculate Z
			if (point.Z > max.Z)
				point.Z = max.Z;
			else if (point.Z < min.Z)
				point.Z = min.Z;

			return result;
		}

		public bool Contains(Vector3 point)
		{
			return ClosestPoint(point) == point;
		}

		public void Encapsulate(Vector3 point)
		{

		}

		public void Expand(float amount)
		{

		}
	}
}
