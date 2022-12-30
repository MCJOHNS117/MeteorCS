using OpenTK.Mathematics;

public class Vertex
{
	public readonly int Size;
}

public struct VertexPC
{
	public const int Size = (4 + 4) * 4; //Size of struct in bytes

	private readonly Vector4 _position;
	private readonly Color4 _color;

	public VertexPC(Vector4 pos, Color4 col)
	{
		_position = pos;
		_color = col;
	}
}

public struct FontVertex
{
	public const int Size = (2 + 2) * 4; // size of struct in bytes

	private readonly Vector2 _position;
	private readonly Vector2 _textureCoordinate;

	public FontVertex(Vector2 position, Vector2 textureCoordinate)
	{
		_position = position;
		_textureCoordinate = textureCoordinate;
	}
}


public struct VertexPUV
{
	public const int Size = (4 + 2) * 4; // size of struct in bytes

	private readonly Vector4 _position;
	private readonly Vector2 _textureCoordinate;

	public VertexPUV(Vector4 position, Vector2 textureCoordinate)
	{
		_position = position;
		_textureCoordinate = textureCoordinate;
	}

	public VertexPUV(Vector2 position, Vector2 textureCoordinate)
	{
		_position = new Vector4(position);
		_textureCoordinate = textureCoordinate;
	}
}

public struct VertexPUVN
{
	public const int Size = (3 + 3 + 2) * 4;

	private readonly Vector3 _position;
	private readonly Vector3 _normal;
	private readonly Vector2 _uv;

	public VertexPUVN(Vector3 pos, Vector3 norm, Vector2 uv)
	{
		_position = pos;
		_normal = norm;
		_uv = uv;
	}
}