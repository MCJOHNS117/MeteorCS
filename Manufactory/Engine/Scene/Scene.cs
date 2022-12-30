namespace MeteorEngine
{
	public class Scene
	{
		public void OnLoad()
		{
			//Load the scene file here
		}

		public void OnUpdate(float deltaTime)
		{
			GameObject[] gameObjects = Object.FindObjectsOfType(typeof(GameObject)) as GameObject[];

			for(int i = 0; i < gameObjects.Length; i++)
			{
				if(gameObjects[i] != null && gameObjects[i].Enabled)
				{
					Component[] components = gameObjects[i].GetComponents();
					for(int j = 0; j < components.Length; j++)
					{
						components[j].Update(deltaTime);
					}
				}
			}
		}

		public void OnFixedUpdate()
		{
			GameObject[] gameObjects = Object.FindObjectsOfType(typeof(GameObject)) as GameObject[];

			for (int i = 0; i < gameObjects.Length; i++)
			{
				if (gameObjects[i] != null && gameObjects[i].Enabled)
				{
					Component[] components = gameObjects[i].GetComponents();
					for (int j = 0; j < components.Length; j++)
					{
						components[j].FixedUpdate();
					}
				}
			}
		}

		public void OnLateUpdate(float deltaTime)
		{
			GameObject[] gameObjects = Object.FindObjectsOfType(typeof(GameObject)) as GameObject[];

			for (int i = 0; i < gameObjects.Length; i++)
			{
				if (gameObjects[i] != null && gameObjects[i].Enabled)
				{
					Component[] components = gameObjects[i].GetComponents();
					for (int j = 0; j < components.Length; j++)
					{
						components[j].LateUpdate(deltaTime);
					}
				}
			}
		}

		public void OnRender()
		{
			Renderer[] renderers = Object.FindObjectsOfType(typeof(Renderer)) as Renderer[];

			for (int i = 0; i < renderers.Length; i++)
			{
				if (renderers[i] != null && renderers[i].GameObject.Enabled)
				{
					renderers[i].OnRender();
				}
			}
		}

		public void OnDestroy()
		{
			//Unload the scene here
		}
	}
}