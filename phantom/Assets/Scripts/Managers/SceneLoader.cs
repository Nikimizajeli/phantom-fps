using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    private const string MenuSceneName = "Menu";
    private const string GameSceneName = "Game";
    
    public void LoadMenuScene()
    {
        SceneManager.LoadScene(MenuSceneName, LoadSceneMode.Additive);
    }

    public void LoadGameScene()
    {
        SceneManager.LoadScene(GameSceneName, LoadSceneMode.Additive);
    }
}
