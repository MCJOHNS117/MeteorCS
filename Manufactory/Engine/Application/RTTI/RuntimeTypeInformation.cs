using MeteorEngine.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MeteorEngine
{
	internal class RuntimeTypeInformation
	{
		private List<FieldInfo> fieldInfos;

		private Type type;

		private RuntimeTypeInformation(Type type)
		{
			this.type = type;

			PopulateFields(this.type);
		}

		private void PopulateFields(Type type)
		{
			if (type.BaseType != null)
				PopulateFields(type.BaseType);

			fieldInfos = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance).ToList();
		}

		private static Dictionary<Type, RuntimeTypeInformation> _instances = new Dictionary<Type, RuntimeTypeInformation>();

		public static RuntimeTypeInformation GetRTTI(Type type)
		{
			if(!_instances.ContainsKey(type))
				_instances.Add(type, new RuntimeTypeInformation(type));

			return _instances[type];
		}

		public static DataFile Serialize(object instance)
		{
			DataFile result = new DataFile();

			RuntimeTypeInformation rtti = GetRTTI(instance.GetType());

			//result["type"].SetString(rtti.type.Name);

			foreach(FieldInfo field in rtti.fieldInfos)
			{
				string json = JsonConvert.SerializeObject(field.GetValue(instance));

				//if(_serializeTypes.ContainsKey(field.FieldType))
				//{
				//	result["type"][field.Name].SetString(_serializeTypes[field.FieldType](field, instance));
				//}
				//else if(field.FieldType.IsArray)
				//{
				//	Array arr = (Array)field.GetValue(instance);
				//	Type elementType = arr.GetType().GetElementType();
				//	if (arr.Rank == 1)
				//	{
				//		result["type"][field.Name]["Length"].SetInt(arr.Length);

				//		if (_serializeTypes.ContainsKey(elementType)) //Element type is a basic value type
				//		{
							
				//			for(int i = 0; i < arr.Length; i++)
				//			{
				//				result["type"][field.Name]["Array"][i.ToString()].SetString(arr.GetValue(i).ToString());
				//			}
				//		}
				//		else if (field.FieldType.IsClass)
				//		{

				//		}
				//	}
				//	else if(arr.Rank == 2)
				//	{
				//		int l0 = arr.GetLength(0);
				//		int l1 = arr.GetLength(1);

				//		result["type"][field.Name]["Length 1"].SetInt(l0);
				//		result["type"][field.Name]["Length 2"].SetInt(l1);

				//		if (_serializeTypes.ContainsKey(elementType))
				//		{
				//			for(int i = 0; i < l0; i++)
				//			{
				//				for(int j = 0; j < l1; j++)
				//				{
				//					result["type"][field.Name]["Array"][i.ToString()][j.ToString()].SetString(arr.GetValue(i, j).ToString());
				//				}
				//			}
				//		}
				//		else if (field.FieldType.IsClass)
				//		{

				//		}
				//	}
				//}
				//else if(field.FieldType.IsGenericType)
				//{

				//}
				//else if(field.FieldType.IsClass)
				//{

				//}
			}

			return result;
		}

		private static Dictionary<Type, Func<FieldInfo, object, string>> _serializeTypes = new Dictionary<Type, Func<FieldInfo, object, string>>()
		{
			{typeof(sbyte), (f, o) => { return f.GetValue(o).ToString(); } },
			{typeof(byte), (f, o) => { return f.GetValue(o).ToString(); } },
			{typeof(short), (f, o) => { return f.GetValue(o).ToString(); } },
			{typeof(ushort), (f, o) => { return f.GetValue(o).ToString(); } },
			{typeof(int), (f, o) => { return f.GetValue(o).ToString(); } },
			{typeof(uint), (f, o) => { return f.GetValue(o).ToString(); } },
			{typeof(long), (f, o) => { return f.GetValue(o).ToString(); } },
			{typeof(ulong), (f, o) => { return f.GetValue(o).ToString(); } },
			{typeof(char), (f, o) => { return f.GetValue(o).ToString(); } },
			{typeof(float), (f, o) => { return f.GetValue(o).ToString(); } },
			{typeof(double), (f, o) => { return f.GetValue(o).ToString(); } },
			{typeof(bool), (f, o) => { return f.GetValue(o).ToString(); } },
			{typeof(decimal), (f, o) => { return f.GetValue(o).ToString(); } },
			{typeof(string), (f, o) => { return (string)f.GetValue(o); } }
		};

		private static Dictionary<Type, Func<DataFile, FieldInfo, object, bool>> _deserializeTypes = new Dictionary<Type, Func<DataFile, FieldInfo, object, bool>>()
		{
			{typeof(sbyte), (df, f, o) => { f.SetValue(o, df[f.Name].GetSByte()); return true; } },
			{typeof(byte), (df, f, o) => { f.SetValue(o, df[f.Name].GetByte()); return true; } },
			{typeof(short), (df, f, o) => { f.SetValue(o, df[f.Name].GetShort()); return true; } },
			{typeof(ushort), (df, f, o) => { f.SetValue(o, df[f.Name].GetUShort()); return true; } },
			{typeof(int), (df, f, o) => { f.SetValue(o, df[f.Name].GetInt()); return true; } },
			{typeof(uint), (df, f, o) => { f.SetValue(o, df[f.Name].GetUInt()); return true; } },
			{typeof(long), (df, f, o) => { f.SetValue(o, df[f.Name].GetLong()); return true; } },
			{typeof(ulong), (df, f, o) => { f.SetValue(o, df[f.Name].GetULong()); return true; } },
			{typeof(char), (df, f, o) => { f.SetValue(o, df[f.Name].GetChar()); return true; } },
			{typeof(float), (df, f, o) => { f.SetValue(o, df[f.Name].GetFloat()); return true; } },
			{typeof(double), (df, f, o) => { f.SetValue(o, df[f.Name].GetDouble()); return true; } },
			{typeof(bool), (df, f, o) => { f.SetValue(o, df[f.Name].GetBool()); return true; } },
			{typeof(decimal), (df, f, o) => { f.SetValue(o, df[f.Name].GetDecimal()); return true; } },
			{typeof(string), (df, f, o) => { f.SetValue(o, df[f.Name].GetString()); return true; } }
		};
	}
}