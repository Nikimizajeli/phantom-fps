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
        var gameScene = SceneManager.LoadSceneAsync(SceneType.GameScene.ToString(), LoadSceneMode.Additive);
        gameScene.allowSceneActivation = false;
        
        transitionScreen.ShowTransitionScreen(() =>
        {
            gameScene.allowSceneActivation = true;
        });
        
        gameScene.completed += (op) =>
        {
            callback?.Invoke();
            transitionScreen.CloseTransitionScreen();
        };
    }
}
