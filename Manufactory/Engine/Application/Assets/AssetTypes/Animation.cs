namespace MeteorEngine
{
	public class Animation : IAsset
	{
		public void Dispose()
		{
			Content.UnloadAsset(this);
		}
	}
}
