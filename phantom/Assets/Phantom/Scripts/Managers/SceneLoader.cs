using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public enum SceneType
{
    StartupScene,
    MenuScene,
    GameScene
}
public class SceneLoader : MonoBehaviour
{
    [SerializeField] private TransitionScreen transitionScreen;

    public bool IsGameSceneActive => SceneManager.GetActiveScene().name == SceneType.GameScene.ToString();

    public void LoadMenuScene()
    {
        SceneManager.LoadScene(SceneType.MenuScene.ToString(), LoadSceneMode.Additive);
    }

    public void LoadGameScene(Action callback = null)
    {
        var sceneLoadingOperation = SceneManager.LoadSceneAsync(SceneType.GameScene.ToString(), LoadSceneMode.Additive);
        sceneLoadingOperation.allowSceneActivation = false;
        
        transitionScreen.ShowTransitionScreen(() =>
        {
            sceneLoadingOperation.allowSceneActivation = true;
        });
        
        sceneLoadingOperation.completed += (op) =>
        {
            SceneManager.SetActiveScene(SceneManager.GetSceneByName(SceneType.GameScene.ToString()));
            callback?.Invoke();
            transitionScreen.CloseTransitionScreen();
        };
    }

    public void UnloadGameScene()
    {
        if (IsGameSceneActive)
        {
            SceneManager.UnloadSceneAsync(SceneType.GameScene.ToString());
            SceneManager.SetActiveScene(SceneManager.GetSceneByName(SceneType.MenuScene.ToString()));
        }

        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }

    private void OnSceneUnloaded(Scene scene)
    {
        Debug.Log($"{scene.name} unloaded");
        Resources.UnloadUnusedAssets();
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
    }
}
