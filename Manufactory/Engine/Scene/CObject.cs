using System;
using System.Collections.Generic;
using System.Linq;

namespace Meteor.Engine.Scene
{
	public class CObject
	{ 
		public Guid InstanceID { get; protected set; }
		public string Tag { get; set; }

		public string Name { get; set; }

		protected CObject()
		{
			InstanceID = Guid.NewGuid();
		}

		protected CObject(Guid id)
		{
			InstanceID = id;
		}

		public override string ToString()
		{
			return Name;
		}

		#region Statics
		private static Dictionary<Guid, CObject> _sceneObjects = new Dictionary<Guid, CObject>();

		//public static 

		public static CObject FinObjectWithTag(string tag)
		{
			return _sceneObjects.Values.Where(k => k.Tag == tag).ToList().FirstOrDefault();
		}

		public static CObject[] FindObjectsWithTag(string tag)
		{
			return _sceneObjects.Values.Where(v => v.Tag == tag).ToArray();
		}

		public static CObject FindObjectOfType(Type type)
		{
			return _sceneObjects.Values.Where(v => v.GetType() == type).ToList().FirstOrDefault();
		}

		public static CObject[] FindObjectsOfType(Type type)
		{
			return _sceneObjects.Values.Where(v => v.GetType() == type).ToArray();
		}
		#endregion
	}
}