using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class MenuButton : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textComponent;
    [SerializeField] private Button buttonComponent;
    
    public void SetText(string text)
    {
        textComponent.text = text;
    }

    public void SetButtonListener(UnityAction callback)
    {
        buttonComponent.onClick.AddListener(callback);
    }
}
