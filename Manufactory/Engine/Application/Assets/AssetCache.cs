using System.Collections.Generic;

namespace Meteor.Engine.Application.Assets
{
	public class AssetCache
	{
		private Dictionary<string, IAsset> cachedObjects;
		private Dictionary<string, int> cacheReferences;

		public AssetCache()
		{
			cachedObjects = new Dictionary<string, IAsset>();
			cacheReferences = new Dictionary<string, int>();
		}

		public object GetCachedObject(string name)
		{
			if(cachedObjects.ContainsKey(name) && cacheReferences.ContainsKey(name))
			{
				cacheReferences[name]++;
				return cachedObjects[name];
			}

			return null;
		}

		public void CacheObject(string name, IAsset obj)
		{
			if (cachedObjects.ContainsKey(name) && cacheReferences.ContainsKey(name))
			{
				cacheReferences[name]++;
			}
			else
			{
				cachedObjects.Add(name, obj);
				cacheReferences.Add(name, 1);
			}
		}

		public void UnloadObject(string name)
		{
			if(cachedObjects.ContainsKey(name) && cacheReferences.ContainsKey(name))
			{
				if (cacheReferences[name] > 0)
				{
					cacheReferences[name]--;

					if (cacheReferences[name] == 0)
					{
						cacheReferences.Remove(name);
						cachedObjects.Remove(name);
					}
				}
				else
					cacheReferences[name]--;
			}
		}
	}
}