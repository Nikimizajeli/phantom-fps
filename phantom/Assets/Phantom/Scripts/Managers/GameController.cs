using System;
using System.Collections;
using Phantom.Scripts.Configuration;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class GameController : SingletonGameObject<GameController>
{
    [SerializeField] private SceneLoader sceneLoader;
    [SerializeField] private InputManager inputManager;
    [SerializeField] private PlayerHUD playerHUD;

    public string PlayerName { get; set; }
    public bool IsPaused { get; private set; }

    private MenuController _menuController;
    private int _secondsTimer;
    private IGameMode _gameMode;


    protected void Awake()
    {
        sceneLoader.LoadMenuScene();
        _gameMode = GetComponent<IGameMode>();
    }

    protected void Start()
    {
        // _menuController.AddButton(TextConstants.PlayGame, () =>
        // {
        //     if (string.IsNullOrEmpty(PlayerName))
        //     {
        //         return;
        //     }
        //     
        //     sceneLoader.LoadGameScene(() =>
        //     {
        //         _gameMode.StartGame();
        //         playerHUD.gameObject.SetActive(true);
        //         _menuController.gameObject.SetActive(false);
        //     });
        //     
        // });
    }

    protected void OnEnable()
    {
        EventDispatcher.Instance.AddListener<MainMenuLoadedEvent>(OnMainMenuLoadedEvent);
    }

    protected void OnDisable()
    {
        EventDispatcher.Instance.RemoveListener<MainMenuLoadedEvent>(OnMainMenuLoadedEvent);
    }

    public void OnPauseKey()
    {
        if (IsPaused)
        {
            UnpauseGame();
        }
        else
        {
            PauseGame();
        }
    }

    private void OnMainMenuLoadedEvent(MainMenuLoadedEvent ev)
    {
        _menuController = ev.MenuController;
        SetMenuButtonsListeners();
    }

    private void SetMenuButtonsListeners()
    {
        ButtonsListeners listeners = new ButtonsListeners
        {
            PlayButtonListener = StartGame,
            ResumeButtonListener = UnpauseGame,
            RestartButtonListener = StartGame,
            QuitButtonListener = MenuBack
        };

        _menuController.SetButtonsListeners(listeners);
    }

    private void StartGame()
    {
        if (string.IsNullOrEmpty(PlayerName))
        {
            return;
        }

        sceneLoader.LoadGameScene(() =>
        {
            _gameMode.StartGame();
            playerHUD.gameObject.SetActive(true);
            _menuController.gameObject.SetActive(false);
        });
    }

    private void PauseGame()
    {
        IsPaused = true;
        Time.timeScale = 0f;
        playerHUD.ShowHUD(false);
        _menuController.gameObject.SetActive(true);
        Cursor.visible = true;
    }

    private void UnpauseGame()
    {
        IsPaused = false;
        Time.timeScale = 1f;
        playerHUD.ShowHUD(true);
        _menuController.gameObject.SetActive(false);
        Cursor.visible = false;
    }

    private void MenuBack()
    {
        if (_menuController.CurrentState == MenuController.MenuState.MainMenu)
        {
            QuitGame();
        }
        else
        {
            _menuController.SwitchMenuState(MenuController.MenuState.MainMenu);
            sceneLoader.UnloadGameScene();
        }
    }
    
    private void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}