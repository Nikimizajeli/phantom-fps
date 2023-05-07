using Phantom.Scripts.Configuration;
using TMPro;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    public enum MenuState
    {
        MainMenu,
        PauseMenu,
        HighScores,
        GameOver
    }
    
    [SerializeField] private Transform buttonsRoot;
    [SerializeField] private TextMeshProUGUI headerText;
    [SerializeField] private PlayerNameInput playerNameInput;
    [SerializeField] private MenuButton playButton;
    [SerializeField] private MenuButton highScoresButton;
    [SerializeField] private MenuButton resumeButton;
    [SerializeField] private MenuButton restartButton;
    [SerializeField] private MenuButton quitButton;
    
    
    // private readonly List<MenuButton> _buttons = new List<MenuButton>();
    private MenuState _currentState;

    protected void Awake()
    {
        EventDispatcher.Instance.Raise<MainMenuLoadedEvent>(new MainMenuLoadedEvent{ MenuController = this });
        
        SwitchMenuState(MenuState.MainMenu);
        
        SetButtonsText();
    }

    public void SwitchMenuState(MenuState state)
    {
        switch (state)
        {
            case MenuState.PauseMenu:
                _currentState = MenuState.PauseMenu;
                headerText.text = TextConstants.Pause;
                
                playerNameInput.gameObject.SetActive(false);
                playButton.gameObject.SetActive(false);
                highScoresButton.gameObject.SetActive(false);
                resumeButton.gameObject.SetActive(true);
                restartButton.gameObject.SetActive(false);
                quitButton.gameObject.SetActive(true);
                break;
            case MenuState.HighScores:
                _currentState = MenuState.HighScores;
                headerText.text = TextConstants.HighScores;
                    
                playerNameInput.gameObject.SetActive(false);
                playButton.gameObject.SetActive(false);
                highScoresButton.gameObject.SetActive(false);
                resumeButton.gameObject.SetActive(false);
                restartButton.gameObject.SetActive(false);
                quitButton.gameObject.SetActive(true);
                break;
            case MenuState.GameOver:
                _currentState = MenuState.GameOver;
                headerText.text = TextConstants.GameOver;
                
                playerNameInput.gameObject.SetActive(false);
                playButton.gameObject.SetActive(false);
                highScoresButton.gameObject.SetActive(false);
                resumeButton.gameObject.SetActive(false);
                restartButton.gameObject.SetActive(true);
                quitButton.gameObject.SetActive(true);
                break;
            case MenuState.MainMenu:
            default:
                _currentState = MenuState.MainMenu;
                headerText.text = TextConstants.MainMenu;
                
                playerNameInput.gameObject.SetActive(true);
                playButton.gameObject.SetActive(true);
                highScoresButton.gameObject.SetActive(true);
                resumeButton.gameObject.SetActive(false);
                restartButton.gameObject.SetActive(false);
                quitButton.gameObject.SetActive(true);
                break;
        }
    }

    private void SetButtonsText()
    {
        playButton.SetText(TextConstants.PlayGame);
        highScoresButton.SetText(TextConstants.HighScores);
        resumeButton.SetText(TextConstants.Resume);
        restartButton.SetText(TextConstants.Restart);
        quitButton.SetText(TextConstants.QuitApp);
    }
    // public void AddButton(string text, Action callback)
    // {
    //     var menuButton = Instantiate(buttonPrefab, buttonsRoot);
    //     _buttons.Add(menuButton);
    //     menuButton.SetText(text);
    //     menuButton.GetComponent<Button>().onClick.AddListener(() =>
    //     {
    //         callback?.Invoke();
    //     });
    // }
    
    // private void ClearButtons()
    // {
    //     foreach (var button in _buttons)
    //     {
    //         Destroy(button.gameObject);
    //     }
    //     
    //     _buttons.Clear();
    // }
}
