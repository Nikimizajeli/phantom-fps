using UnityEngine;
using UnityEngine.SceneManagement;

public enum SceneType
{
    StartupScene,
    MenuScene,
    GameScene
}
public class SceneLoader : MonoBehaviour
{
    public void LoadMenuScene()
    {
        SceneManager.LoadScene(SceneType.MenuScene.ToString(), LoadSceneMode.Additive);
    }

    public void LoadGameScene()
    {
        SceneManager.LoadScene(SceneType.GameScene.ToString(), LoadSceneMode.Additive);
    }
}
