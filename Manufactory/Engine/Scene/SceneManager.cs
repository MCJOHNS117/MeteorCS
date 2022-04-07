using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Meteor.Engine.Scene
{
	public class SceneManager
	{
		private void Test()
		{
			ObjectHandle handle = Activator.CreateInstance("AssemblyName", "TypeName");

			Transform trans = (Transform)handle.Unwrap();
			trans.GetType().GetField("FieldName", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(trans, "FieldValue");

			var jsonString = JsonConvert.SerializeObject(trans);
			Type type = Type.GetType("Transform");
			MethodInfo method = typeof(JsonConvert).GetMethod(nameof(JsonConvert.DeserializeObject), BindingFlags.Static);
			MethodInfo generic = method.MakeGenericMethod(type);
			object res = generic.Invoke(null, new object[] {jsonString});
			

			/*
			 *	Component - Assembly Name, Type Name
			 *	 
			 */
		}

		private void Test2<T>() where T : class
		{

		}
	}
}