﻿using OpenTK;
using OpenTK.Graphics;

namespace Meteor.Engine.Utils
{
	public class ObjectFactory
	{
		public static VertexPC[] CreateSolidCube(float side, Color4 color)
		{
			side = side / 2f; // half side - and other half
			VertexPC[] vertices =
			{
				new VertexPC(new Vector4(-side, -side, -side, 1.0f),   color),
				new VertexPC(new Vector4(-side, -side, side, 1.0f),    color),
				new VertexPC(new Vector4(-side, side, -side, 1.0f),    color),
				new VertexPC(new Vector4(-side, side, -side, 1.0f),    color),
				new VertexPC(new Vector4(-side, -side, side, 1.0f),    color),
				new VertexPC(new Vector4(-side, side, side, 1.0f),     color),

				new VertexPC(new Vector4(side, -side, -side, 1.0f),    color),
				new VertexPC(new Vector4(side, side, -side, 1.0f),     color),
				new VertexPC(new Vector4(side, -side, side, 1.0f),     color),
				new VertexPC(new Vector4(side, -side, side, 1.0f),     color),
				new VertexPC(new Vector4(side, side, -side, 1.0f),     color),
				new VertexPC(new Vector4(side, side, side, 1.0f),      color),

				new VertexPC(new Vector4(-side, -side, -side, 1.0f),   color),
				new VertexPC(new Vector4(side, -side, -side, 1.0f),    color),
				new VertexPC(new Vector4(-side, -side, side, 1.0f),    color),
				new VertexPC(new Vector4(-side, -side, side, 1.0f),    color),
				new VertexPC(new Vector4(side, -side, -side, 1.0f),    color),
				new VertexPC(new Vector4(side, -side, side, 1.0f),     color),

				new VertexPC(new Vector4(-side, side, -side, 1.0f),    color),
				new VertexPC(new Vector4(-side, side, side, 1.0f),     color),
				new VertexPC(new Vector4(side, side, -side, 1.0f),     color),
				new VertexPC(new Vector4(side, side, -side, 1.0f),     color),
				new VertexPC(new Vector4(-side, side, side, 1.0f),     color),
				new VertexPC(new Vector4(side, side, side, 1.0f),      color),

				new VertexPC(new Vector4(-side, -side, -side, 1.0f),   color),
				new VertexPC(new Vector4(-side, side, -side, 1.0f),    color),
				new VertexPC(new Vector4(side, -side, -side, 1.0f),    color),
				new VertexPC(new Vector4(side, -side, -side, 1.0f),    color),
				new VertexPC(new Vector4(-side, side, -side, 1.0f),    color),
				new VertexPC(new Vector4(side, side, -side, 1.0f),     color),

				new VertexPC(new Vector4(-side, -side, side, 1.0f),    color),
				new VertexPC(new Vector4(side, -side, side, 1.0f),     color),
				new VertexPC(new Vector4(-side, side, side, 1.0f),     color),
				new VertexPC(new Vector4(-side, side, side, 1.0f),     color),
				new VertexPC(new Vector4(side, -side, side, 1.0f),     color),
				new VertexPC(new Vector4(side, side, side, 1.0f),      color),
			};
			return vertices;
		}

		public static VertexPUV[] CreateTexturedCube(float side)
		{
			side = side / 2f; // half side - and other half

			VertexPUV[] vertices =
			{
				new VertexPUV(new Vector4(-side, -side, -side, 1.0f),   new Vector2(0, 0)),
				new VertexPUV(new Vector4(-side, -side, side, 1.0f),    new Vector2(0, 1)),
				new VertexPUV(new Vector4(-side, side, -side, 1.0f),    new Vector2(1, 0)),
				new VertexPUV(new Vector4(-side, side, -side, 1.0f),    new Vector2(1, 0)),
				new VertexPUV(new Vector4(-side, -side, side, 1.0f),    new Vector2(0, 1)),
				new VertexPUV(new Vector4(-side, side, side, 1.0f),     new Vector2(1, 1)),

				new VertexPUV(new Vector4(side, -side, -side, 1.0f),    new Vector2(0, 0)),
				new VertexPUV(new Vector4(side, side, -side, 1.0f),     new Vector2(1, 0)),
				new VertexPUV(new Vector4(side, -side, side, 1.0f),     new Vector2(0, 1)),
				new VertexPUV(new Vector4(side, -side, side, 1.0f),     new Vector2(0, 1)),
				new VertexPUV(new Vector4(side, side, -side, 1.0f),     new Vector2(1, 0)),
				new VertexPUV(new Vector4(side, side, side, 1.0f),      new Vector2(1, 1)),

				new VertexPUV(new Vector4(-side, -side, -side, 1.0f),   new Vector2(0, 0)),
				new VertexPUV(new Vector4(side, -side, -side, 1.0f),    new Vector2(1, 0)),
				new VertexPUV(new Vector4(-side, -side, side, 1.0f),    new Vector2(0, 1)),
				new VertexPUV(new Vector4(-side, -side, side, 1.0f),    new Vector2(0, 1)),
				new VertexPUV(new Vector4(side, -side, -side, 1.0f),    new Vector2(1, 0)),
				new VertexPUV(new Vector4(side, -side, side, 1.0f),     new Vector2(1, 1)),

				new VertexPUV(new Vector4(-side, side, -side, 1.0f),    new Vector2(0, 0)),
				new VertexPUV(new Vector4(-side, side, side, 1.0f),     new Vector2(0, 1)),
				new VertexPUV(new Vector4(side, side, -side, 1.0f),     new Vector2(1, 0)),
				new VertexPUV(new Vector4(side, side, -side, 1.0f),     new Vector2(1, 0)),
				new VertexPUV(new Vector4(-side, side, side, 1.0f),     new Vector2(0, 1)),
				new VertexPUV(new Vector4(side, side, side, 1.0f),      new Vector2(1, 1)),

				new VertexPUV(new Vector4(-side, -side, -side, 1.0f),   new Vector2(0, 0)),
				new VertexPUV(new Vector4(-side, side, -side, 1.0f),    new Vector2(0, 1)),
				new VertexPUV(new Vector4(side, -side, -side, 1.0f),    new Vector2(1, 0)),
				new VertexPUV(new Vector4(side, -side, -side, 1.0f),    new Vector2(1, 0)),
				new VertexPUV(new Vector4(-side, side, -side, 1.0f),    new Vector2(0, 1)),
				new VertexPUV(new Vector4(side, side, -side, 1.0f),     new Vector2(0, 0)),

				new VertexPUV(new Vector4(-side, -side, side, 1.0f),    new Vector2(0, 0)),
				new VertexPUV(new Vector4(side, -side, side, 1.0f),     new Vector2(1, 0)),
				new VertexPUV(new Vector4(-side, side, side, 1.0f),     new Vector2(0, 1)),
				new VertexPUV(new Vector4(-side, side, side, 1.0f),     new Vector2(0, 1)),
				new VertexPUV(new Vector4(side, -side, side, 1.0f),     new Vector2(1, 0)),
				new VertexPUV(new Vector4(side, side, side, 1.0f),      new Vector2(1, 1)),
			};
			return vertices;
		}

	}
}
