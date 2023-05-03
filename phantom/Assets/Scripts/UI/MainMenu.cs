using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private MenuButton _buttonPrefab;
    [SerializeField] private Transform _buttonsRoot;
    
    private readonly List<MenuButton> _buttons = new List<MenuButton>();

    protected void Awake()
    {
        EventDispatcher.Instance.Raise<MainMenuLoadedEvent>(new MainMenuLoadedEvent{ mainMenu = this });
    }

    public void AddButton(string text, Action callback)
    {
        var menuButton = Instantiate(_buttonPrefab, _buttonsRoot);
        _buttons.Add(menuButton);
        menuButton.SetText(text);
        menuButton.GetComponent<Button>().onClick.AddListener(() =>
        {
            callback?.Invoke();
        });
    }
    
    private void ClearButtons()
    {
        foreach (var button in _buttons)
        {
            Destroy(button.gameObject);
        }
        
        _buttons.Clear();
    }
}
