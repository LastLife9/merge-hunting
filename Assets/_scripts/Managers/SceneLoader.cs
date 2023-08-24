using UnityEngine.SceneManagement;

public class SceneLoader
{
    public void LoadHuntScene()
    {
        SceneManager.LoadScene(1);
    }
    public void LoadMenuScene()
    {
        SceneManager.LoadScene(0);
    }
}