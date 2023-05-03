using System;
using UnityEngine;

public class GameController : SingletonGameObject<GameController>
{
    [SerializeField] private SceneLoader sceneLoader;
    [SerializeField] private InputManager inputManager;

    public string PlayerName { get; set; }

    private MainMenu _mainMenu;

    protected void Awake()
    {
        sceneLoader.LoadMenuScene();
    }

    protected void Start()
    {
        _mainMenu.AddButton(TextConstants.PlayGame, () =>
        {
            _mainMenu.gameObject.SetActive(false);
            sceneLoader.LoadGameScene();
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
        _mainMenu = ev.mainMenu;
    }
}
