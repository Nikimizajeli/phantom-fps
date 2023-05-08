using System.Collections.Generic;
using UnityEngine;

public class GameController : SingletonGameObject<GameController>
{
    [SerializeField] private SceneLoader sceneLoader;
    [SerializeField] private InputManager inputManager;
    [SerializeField] private PlayerHUD playerHUD;
    [SerializeField] private HighScoresManager highScoresManager;

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

    protected void OnEnable()
    {
        EventDispatcher.Instance.AddListener<MainMenuLoadedEvent>(OnMainMenuLoadedEvent);
        EventDispatcher.Instance.AddListener<GameCompletedEvent>(OnGameCompleted);
    }

    protected void OnDisable()
    {
        EventDispatcher.Instance.RemoveListener<MainMenuLoadedEvent>(OnMainMenuLoadedEvent);
        EventDispatcher.Instance.RemoveListener<GameCompletedEvent>(OnGameCompleted);
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

    public SortedSet<HighScoreEntry> GetHighScores()
    {
        return highScoresManager.GetHighScores();
    }
    
    private void OnMainMenuLoadedEvent(MainMenuLoadedEvent ev)
    {
        _menuController = ev.MenuController;
        SetMenuButtonsListeners();
    }

    private void OnGameCompleted(GameCompletedEvent ev)
    {
        _menuController.gameObject.SetActive(true);
        _menuController.SwitchMenuState(MenuController.MenuState.GameOver);
        sceneLoader.UnloadGameScene();
        Cursor.visible = true;
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
            _menuController.gameObject.SetActive(false);
            Cursor.visible = false;
        });
    }

    private void PauseGame()
    {
        if (!sceneLoader.IsGameSceneActive)
        {
            return;
        }
        
        IsPaused = true;
        Time.timeScale = 0f;
        _menuController.gameObject.SetActive(true);
        _menuController.SwitchMenuState(MenuController.MenuState.PauseMenu);
        Cursor.visible = true;
    }

    private void UnpauseGame()
    {
        if (!sceneLoader.IsGameSceneActive)
        {
            return;
        }
        
        IsPaused = false;
        Time.timeScale = 1f;
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