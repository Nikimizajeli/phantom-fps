using System.Collections.Generic;
using Phantom.Scripts.Configuration;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class ButtonsListeners
{
    public UnityAction PlayButtonListener;
    public UnityAction ResumeButtonListener;
    public UnityAction RestartButtonListener;
    public UnityAction QuitButtonListener;
}

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
    [SerializeField] private GameObject highScoresRoot;

    public MenuState CurrentState { get; set; }

    protected void Awake()
    {
        EventDispatcher.Instance.Raise<MainMenuLoadedEvent>(new MainMenuLoadedEvent{ MenuController = this });
        
        SwitchMenuState(MenuState.MainMenu);
        
        SetButtonsText();
        highScoresButton.SetButtonListener(()=>
        {
            SwitchMenuState(MenuState.HighScores);
        });
    }

    public void SwitchMenuState(MenuState state)
    {
        switch (state)
        {
            case MenuState.PauseMenu:
                CurrentState = MenuState.PauseMenu;
                headerText.text = TextConstants.Pause;
                
                playerNameInput.gameObject.SetActive(false);
                highScoresRoot.SetActive(false);
                playButton.gameObject.SetActive(false);
                highScoresButton.gameObject.SetActive(false);
                resumeButton.gameObject.SetActive(true);
                restartButton.gameObject.SetActive(false);
                quitButton.gameObject.SetActive(true);
                break;
            case MenuState.HighScores:
                CurrentState = MenuState.HighScores;
                headerText.text = TextConstants.HighScores;
                    
                playerNameInput.gameObject.SetActive(false);
                highScoresRoot.SetActive(true);
                playButton.gameObject.SetActive(false);
                highScoresButton.gameObject.SetActive(false);
                resumeButton.gameObject.SetActive(false);
                restartButton.gameObject.SetActive(false);
                quitButton.gameObject.SetActive(true);
                break;
            case MenuState.GameOver:
                CurrentState = MenuState.GameOver;
                headerText.text = TextConstants.GameOver;
                
                playerNameInput.gameObject.SetActive(false);
                highScoresRoot.SetActive(false);
                playButton.gameObject.SetActive(false);
                highScoresButton.gameObject.SetActive(false);
                resumeButton.gameObject.SetActive(false);
                restartButton.gameObject.SetActive(true);
                quitButton.gameObject.SetActive(true);
                break;
            case MenuState.MainMenu:
            default:
                CurrentState = MenuState.MainMenu;
                headerText.text = TextConstants.MainMenu;
                
                playerNameInput.gameObject.SetActive(true);
                highScoresRoot.SetActive(false);
                playButton.gameObject.SetActive(true);
                highScoresButton.gameObject.SetActive(true);
                resumeButton.gameObject.SetActive(false);
                restartButton.gameObject.SetActive(false);
                quitButton.gameObject.SetActive(true);
                break;
        }
    }
    
    public void SetButtonsListeners(ButtonsListeners listeners)
    {
        playButton.SetButtonListener(listeners.PlayButtonListener);
        resumeButton.SetButtonListener(listeners.ResumeButtonListener);
        restartButton.SetButtonListener(listeners.RestartButtonListener);
        quitButton.SetButtonListener(listeners.QuitButtonListener);
    }
    
    private void SetButtonsText()
    {
        playButton.SetText(TextConstants.PlayGame);
        highScoresButton.SetText(TextConstants.HighScores);
        resumeButton.SetText(TextConstants.Resume);
        restartButton.SetText(TextConstants.Restart);
        quitButton.SetText(TextConstants.QuitApp);
    }
}
