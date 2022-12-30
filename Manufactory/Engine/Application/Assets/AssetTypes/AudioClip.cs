namespace MeteorEngine
{
	public class AudioClip : IAsset
	{
		public AudioClip()
		{

		}

		public void Dispose()
		{
			Content.UnloadAsset(this);
		}
	}
}
