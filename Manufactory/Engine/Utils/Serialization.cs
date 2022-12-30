using OpenTK.Mathematics;
using System;
using System.IO;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MeteorEngine
{
	public class JsonConverters
	{
		public static JsonSerializerOptions SerializerOptions = new()
		{
			ReferenceHandler = ReferenceHandler.Preserve,
			Converters = {
					new Vector2Converter(),
					new Vector3Converter(),
					new Vector4Converter(),
					new QuaternionConverter(),
					new Matrix4Converter(),
					new GameObjectConverter(),
					new TransformConverter()
				},
			WriteIndented = true
		};

		public class Vector2Converter : JsonConverter<Vector2>
		{
			public override Vector2 Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
			{
				Vector2 result = new Vector2();

				string propertyName = "";

				while (reader.Read())
				{
					if (reader.TokenType == JsonTokenType.EndObject)
					{
						return result;
					}

					if (reader.TokenType == JsonTokenType.PropertyName)
					{
						propertyName = reader.GetString();
						reader.Read();
						if (propertyName == "X") result.X = reader.GetSingle();
						else if (propertyName == "Y") result.Y = reader.GetSingle();
					}
				}

				return result;
			}

			public override void Write(Utf8JsonWriter writer, Vector2 value, JsonSerializerOptions options)
			{
				writer.WriteStartObject();
				writer.WriteNumber("X", value.X);
				writer.WriteNumber("Y", value.Y);
				writer.WriteEndObject();
			}
		}

		public class Vector3Converter : JsonConverter<Vector3>
		{
			public override Vector3 Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
			{
				Vector3 result = new Vector3();
				while (reader.Read())
				{
					if (reader.TokenType == JsonTokenType.EndObject)
					{
						return result;
					}

					if (reader.TokenType == JsonTokenType.PropertyName)
					{
						string propertyName = reader.GetString();
						reader.Read();
						if (propertyName == "X") result.X = reader.GetSingle();
						else if (propertyName == "Y") result.Y = reader.GetSingle();
						else if (propertyName == "Z") result.Z = reader.GetSingle();
					}
				}

				return result;
			}

			public override void Write(Utf8JsonWriter writer, Vector3 value, JsonSerializerOptions options)
			{
				writer.WriteStartObject();
				writer.WriteNumber("X", value.X);
				writer.WriteNumber("Y", value.Y);
				writer.WriteNumber("Z", value.Z);
				writer.WriteEndObject();
			}
		}

		public class Vector4Converter : JsonConverter<Vector4>
		{
			public override Vector4 Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
			{
				Vector4 result = new Vector4();
				while (reader.Read())
				{
					if (reader.TokenType == JsonTokenType.EndObject)
					{
						return result;
					}

					if (reader.TokenType == JsonTokenType.PropertyName)
					{
						string propertyName = reader.GetString();
						reader.Read();
						if (propertyName == "X") result.X = reader.GetSingle();
						else if (propertyName == "Y") result.Y = reader.GetSingle();
						else if (propertyName == "Z") result.Z = reader.GetSingle();
						else if (propertyName == "W") result.W = reader.GetSingle();
					}
				}

				return result;
			}

			public override void Write(Utf8JsonWriter writer, Vector4 value, JsonSerializerOptions options)
			{
				writer.WriteStartObject();
				writer.WriteNumber("X", value.X);
				writer.WriteNumber("Y", value.Y);
				writer.WriteNumber("Z", value.Z);
				writer.WriteNumber("W", value.W);
				writer.WriteEndObject();
			}
		}

		public class QuaternionConverter : JsonConverter<Quaternion>
		{
			public override Quaternion Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
			{
				Quaternion result = new Quaternion();
				while (reader.Read())
				{
					if (reader.TokenType == JsonTokenType.EndObject)
					{
						return result;
					}

					if (reader.TokenType == JsonTokenType.PropertyName)
					{
						string propertyName = reader.GetString();
						reader.Read();
						if (propertyName == "X") result.X = reader.GetSingle();
						else if (propertyName == "Y") result.Y = reader.GetSingle();
						else if (propertyName == "Z") result.Z = reader.GetSingle();
						else if (propertyName == "W") result.W = reader.GetSingle();
					}
				}

				return result;
			}

			public override void Write(Utf8JsonWriter writer, Quaternion value, JsonSerializerOptions options)
			{
				writer.WriteStartObject();
				writer.WriteNumber("X", value.X);
				writer.WriteNumber("Y", value.Y);
				writer.WriteNumber("Z", value.Z);
				writer.WriteNumber("W", value.W);
				writer.WriteEndObject();
			}
		}

		public class Matrix4Converter : JsonConverter<Matrix4>
		{
			public override Matrix4 Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
			{
				Matrix4 result = new Matrix4();

				while (reader.Read())
				{
					if (reader.TokenType == JsonTokenType.EndObject)
					{
						return result;
					}

					if (reader.TokenType == JsonTokenType.PropertyName)
					{
						string propertyName = reader.GetString();
						reader.Read();

						switch (propertyName)
						{
							case "_11":
								result.M11 = reader.GetSingle();
								break;
							case "_12":
								result.M12 = reader.GetSingle();
								break;
							case "_13":
								result.M13 = reader.GetSingle();
								break;
							case "_14":
								result.M14 = reader.GetSingle();
								break;

							case "_21":
								result.M21 = reader.GetSingle();
								break;
							case "_22":
								result.M22 = reader.GetSingle();
								break;
							case "_23":
								result.M23 = reader.GetSingle();
								break;
							case "_24":
								result.M24 = reader.GetSingle();
								break;

							case "_31":
								result.M31 = reader.GetSingle();
								break;
							case "_32":
								result.M32 = reader.GetSingle();
								break;
							case "_33":
								result.M33 = reader.GetSingle();
								break;
							case "_34":
								result.M34 = reader.GetSingle();
								break;

							case "_41":
								result.M41 = reader.GetSingle();
								break;
							case "_42":
								result.M42 = reader.GetSingle();
								break;
							case "_43":
								result.M43 = reader.GetSingle();
								break;
							case "_44":
								result.M44 = reader.GetSingle();
								break;
						}
					}
				}

				return result;
			}

			public override void Write(Utf8JsonWriter writer, Matrix4 value, JsonSerializerOptions options)
			{
				writer.WriteStartObject();

				writer.WriteNumber("_11", value.M11);
				writer.WriteNumber("_12", value.M12);
				writer.WriteNumber("_13", value.M13);
				writer.WriteNumber("_14", value.M14);

				writer.WriteNumber("_21", value.M21);
				writer.WriteNumber("_22", value.M22);
				writer.WriteNumber("_23", value.M23);
				writer.WriteNumber("_24", value.M24);

				writer.WriteNumber("_31", value.M31);
				writer.WriteNumber("_32", value.M32);
				writer.WriteNumber("_33", value.M33);
				writer.WriteNumber("_34", value.M34);

				writer.WriteNumber("_41", value.M41);
				writer.WriteNumber("_42", value.M42);
				writer.WriteNumber("_43", value.M43);
				writer.WriteNumber("_44", value.M44);

				writer.WriteEndObject();
			}
		}

		public class TransformConverter : JsonConverter<Transform>
		{
			public override Transform Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
			{
				//Deserialize the root transform
				//Deserialize the transforms children
				Transform result = null;

				using (JsonDocument jsonDoc = JsonDocument.ParseValue(ref reader))
				{
					JsonElement currentElement = jsonDoc.RootElement.GetProperty("InstanceID");
					Guid id = Guid.Parse(currentElement.GetString());
					currentElement = jsonDoc.RootElement.GetProperty("Position");
					Vector3 position = JsonSerializer.Deserialize<Vector3>(currentElement, SerializerOptions);
					currentElement = jsonDoc.RootElement.GetProperty("Scale");
					Vector3 scale = JsonSerializer.Deserialize<Vector3>(currentElement, SerializerOptions);
					currentElement = jsonDoc.RootElement.GetProperty("Rotation");
					Quaternion rotation = JsonSerializer.Deserialize<Quaternion>(currentElement, SerializerOptions);

					currentElement = jsonDoc.RootElement.GetProperty("GameObject");
					GameObject gameObject = JsonSerializer.Deserialize<GameObject>(currentElement, SerializerOptions);

					result = new Transform(id) { Position = position, Scale = scale, Rotation = rotation, GameObject = gameObject };

					currentElement = jsonDoc.RootElement.GetProperty("Children");
					foreach (JsonElement child in currentElement.EnumerateArray())
					{
						//Recursively load children
						result.AddChild(JsonSerializer.Deserialize<Transform>(child, SerializerOptions));
					}
				}

				return result;
			}

			public override void Write(Utf8JsonWriter writer, Transform value, JsonSerializerOptions options)
			{
				writer.WriteStartObject();

				writer.WriteString("InstanceID", value.GetInstanceID());

				writer.WritePropertyName("Position");
				JsonSerializer.Serialize(writer, value.Position, SerializerOptions);
				writer.WritePropertyName("Scale");
				JsonSerializer.Serialize(writer, value.Scale, SerializerOptions);
				writer.WritePropertyName("Rotation");
				JsonSerializer.Serialize(writer, value.Rotation, SerializerOptions);
				writer.WritePropertyName("GameObject");
				JsonSerializer.Serialize(writer, value.GameObject, SerializerOptions);

				writer.WritePropertyName("Children");
				writer.WriteStartArray();

				for (int i = 0; i < value.ChildCount; i++)
				{
					JsonSerializer.Serialize(writer, value.GetChild(i), SerializerOptions);
				}

				writer.WriteEndArray();

				writer.WriteEndObject();
			}
		}

		public class GameObjectConverter : JsonConverter<GameObject>
		{
			public override GameObject Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
			{
				using (JsonDocument jsonDoc = JsonDocument.ParseValue(ref reader))
				{
					string n = jsonDoc.RootElement.GetProperty("Name").GetString();
					string t = jsonDoc.RootElement.GetProperty("Tag").GetString();
					string g = jsonDoc.RootElement.GetProperty("InstanceID").GetString();

					//TODO: Deserialize components

					return new GameObject(Guid.Parse(g), n, t);
				}
			}

			public override void Write(Utf8JsonWriter writer, GameObject value, JsonSerializerOptions options)
			{
				writer.WriteStartObject();

				writer.WriteString("Name", value.Name);
				writer.WriteString("Tag", value.Tag);
				writer.WriteString("InstanceID", value.GetInstanceID());

				//TODO: Serialize componenets

				writer.WriteEndObject();
			}
		}
	}
}