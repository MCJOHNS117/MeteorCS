namespace Meteor.Engine.Application.Assets.TypeLoaders
{
	public interface ITypeLoader
	{
		object LoadAsset(string path);
	}
}