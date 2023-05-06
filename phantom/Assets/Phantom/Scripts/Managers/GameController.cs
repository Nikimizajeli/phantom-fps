using System;
using Phantom.Scripts.Configuration;
using UnityEngine;

public class GameController : SingletonGameObject<GameController>
{
    [SerializeField] private SceneLoader sceneLoader;
    [SerializeField] private InputManager inputManager;

    public string PlayerName { get; set; }

    private MenuController _menuController;

    protected void Awake()
    {
        sceneLoader.LoadMenuScene();
    }

    protected void Start()
    {
        _menuController.AddButton(TextConstants.PlayGame, () =>
        {
            sceneLoader.LoadGameScene(() =>
            {
                _menuController.gameObject.SetActive(false);
            });
            
        });
    }

    protected void OnEnable()
    {
        EventDispatcher.Instance.AddListener<MainMenuLoadedEvent>(OnMainMenuLoadedEvent);
    }

    protected void OnDisable()
    {
        EventDispatcher.Instance.RemoveListener<MainMenuLoadedEvent>(OnMainMenuLoadedEvent);
    }

    private void OnMainMenuLoadedEvent(MainMenuLoadedEvent ev)
    {
        _menuController = ev.MenuController;
    }
}
