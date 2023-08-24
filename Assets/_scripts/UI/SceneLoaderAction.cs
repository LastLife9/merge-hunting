using UnityEngine;

public class SceneLoaderAction : MonoBehaviour
{
    SceneLoader sceneLoader = new SceneLoader();

    public void LoadMenu()
    {
        sceneLoader.LoadMenuScene();
    }

    public void LoadHunt()
    {
        sceneLoader.LoadMenuScene();
    }
}
