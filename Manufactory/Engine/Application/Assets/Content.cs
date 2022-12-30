using System;
using System.Collections.Generic;

namespace MeteorEngine
{
	public static class Content
	{
		private static Dictionary<Type, List<string>> _supportedExtensions = new Dictionary<Type, List<string>>();

		private static AssetCache _assetCache;

		private static Dictionary<Type, ITypeImporter> _typeLoaders = new Dictionary<Type, ITypeImporter>();

		public static void Initialize()
		{
			_assetCache = new AssetCache();

			_supportedExtensions.Add(typeof(Texture2D), new List<string>() { "bmp", "gif", "jpg", "jpeg", "jpe", "jif", "jfif", "jfi", "png", "tif", "tiff" });
			_typeLoaders.Add(typeof(Texture2D), new TextureImporter(@"Data\\Textures\\"));

			_supportedExtensions.Add(typeof(Model), new List<string>() { "fbx", "dae", "gltf", "glb", "blend", "3ds", "ase", "obj", "ifc", "xgl", "zgl", "ply", "lwo", "lws", "stl", "x", "ac", "ms3d" });
			_typeLoaders.Add(typeof(Model), new ModelImporter(@"Data\\Models\\"));

			_supportedExtensions.Add(typeof(AudioClip), new List<string>() { "mp3", "wav" });
			_typeLoaders.Add(typeof(AudioClip), new AudioImporter(@"Data\\Audio\\"));

			_supportedExtensions.Add(typeof(Shader), new List<string>() { "sh" });
			_typeLoaders.Add(typeof(Shader), new ShaderImporter(@"Data\\Shaders\\"));

			_supportedExtensions.Add(typeof(Material), new List<string> { "mat" });
			_typeLoaders.Add(typeof(Material), new MaterialImporter(@"Data\\Materials\\"));
		}

		/// <summary>
		/// Attempts to load a given asset type from the given filename.
		/// </summary>
		/// <typeparam name="type"></typeparam>
		/// <param name="filename"></param>
		/// <returns></returns>
		public static type Load<type>(string filename) where type : class, IAsset
		{
			//Retrieve the file extension so we can determine if it is supported, and what loader should be used.
			string ext = GetExtension(filename);

			//Check if the file extension is supported
			if (_supportedExtensions.ContainsKey(typeof(type)) && _supportedExtensions[typeof(type)].Contains(ext))
			{
				//First check the asset cache to see if this content is already loaded.
				type asset = _assetCache.GetCachedObject(filename) as type;

				//If the asset was found in the AssetCache, return it.
				if (null != asset)
					return asset;

				//If the asset was not found, verify we have a Loader to handle this Asset Type
				if (_typeLoaders.ContainsKey(typeof(type)))
				{
					//Attempt to load the file and add it to the asset cache.
					asset = _typeLoaders[typeof(type)].ImportAsset(filename) as type;
					if (typeof(IAsset).IsAssignableFrom(typeof(type)))
						_assetCache.CacheObject(filename, asset);
					return asset;
				}
			}

			return default;
		}

		/// <summary>
		/// Returns the extension of this file by finding the last index of the '.' character.
		/// </summary>
		/// <param name="filename"></param>
		/// <returns></returns>
		private static string GetExtension(string filename)
		{
			return filename.Substring(filename.LastIndexOf('.') + 1).ToLower();
		}

		public static string GetAssetName(IAsset asset)
		{
			return _assetCache.GetAssetName(asset);
		}

		public static void UnloadAsset(IAsset asset)
		{
			_assetCache.UnloadObject(GetAssetName(asset));
		}
	}
}