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
        if (SceneManager.GetActiveScene().name == SceneType.GameScene.ToString())
        {
            SceneManager.UnloadSceneAsync(SceneType.GameScene.ToString());
        }

        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }

    private void OnSceneUnloaded(Scene scene)
    {
        Debug.Log($"{scene} unloaded");
        Resources.UnloadUnusedAssets();
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
    }
}
