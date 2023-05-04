using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class TransitionScreen : MonoBehaviour
{
    private static readonly int TransitionInTrigger = Animator.StringToHash("transition_in");
    private static readonly int TransitionOutTrigger = Animator.StringToHash("transition_out");

    [SerializeField] private Animator animator;

    private Action _onTransitionScreenShown;

    public void ShowTransitionScreen(Action callback)
    {
        animator.SetTrigger(TransitionInTrigger);
        _onTransitionScreenShown += callback;
    }

    public void CloseTransitionScreen()
    {
        animator.SetTrigger(TransitionOutTrigger);
    }

    public void OnTransitionScreenShown()
    {
        _onTransitionScreenShown?.Invoke();
    }
}
