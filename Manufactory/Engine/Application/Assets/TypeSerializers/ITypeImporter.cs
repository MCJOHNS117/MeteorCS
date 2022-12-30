namespace MeteorEngine
{
	public interface ITypeImporter
	{
		string BasePath { get; }
		object ImportAsset(string path);
	}
}