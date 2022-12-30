using System;

namespace MeteorEngine
{
	internal class AnimationImporter : ITypeImporter
	{
		public string BasePath { get; }

		public AnimationImporter(string basePath)
		{
			BasePath = basePath;
		}

		public object ImportAsset(string path)
		{
			throw new NotImplementedException();
		}
	}
}