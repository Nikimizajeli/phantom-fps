using System;
using System.Collections;
using System.Collections.Generic;
using Phantom.Scripts.Configuration;
using TMPro;
using UnityEngine;

public class PlayerNameInput : MonoBehaviour
{
    [SerializeField] private TMP_InputField nameInputField;
    [SerializeField] private TextMeshProUGUI placeholderText;

    protected void Start()
    {
        placeholderText.text = TextConstants.EnterYourName;
        nameInputField.characterLimit = Constants.PlayerNameCharacterLimit;
    }

    public void OnInputValueChanged(string input)
    {
        GameController.Instance.PlayerName = input;
    }
}
