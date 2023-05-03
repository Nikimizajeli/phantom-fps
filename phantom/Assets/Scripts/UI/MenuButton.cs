using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MenuButton : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI TextComponent;

    public void SetText(string text)
    {
        TextComponent.text = text;
    }
}
