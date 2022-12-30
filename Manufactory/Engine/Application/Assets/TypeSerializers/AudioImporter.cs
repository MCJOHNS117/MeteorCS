using System;

namespace MeteorEngine
{
	internal class AudioImporter : ITypeImporter
	{
		public string BasePath { get; }

		public AudioImporter(string basePath)
		{
			BasePath = basePath;
		}

		public object ImportAsset(string path)
		{
			throw new NotImplementedException();
		}
	}
}
