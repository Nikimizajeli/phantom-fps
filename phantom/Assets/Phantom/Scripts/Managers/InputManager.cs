using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    private IPlayerControlled _controlledCharacter;

    protected void OnEnable()
    {
        EventDispatcher.Instance.AddListener<AssumingCharacterControlEvent>(OnAssumingCharacterControlEvent);
    }

    protected void OnDisable()
    {
        EventDispatcher.Instance.RemoveListener<AssumingCharacterControlEvent>(OnAssumingCharacterControlEvent);
    }

    public void Move(InputAction.CallbackContext context)
    {
        var direction = context.ReadValue<Vector2>();
        _controlledCharacter?.Move(direction);
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (!context.performed)
        {
            return;
        }
        _controlledCharacter?.Jump();
    }

    public void Sprint(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            _controlledCharacter?.Sprint(true);
        }
        else if (context.canceled)
        {
            _controlledCharacter?.Sprint(false);
        }
        
    }

    public void Look(InputAction.CallbackContext context)
    {
        var direction = context.ReadValue<Vector2>();
        _controlledCharacter?.LookAround(direction);
    }

    private void OnAssumingCharacterControlEvent(AssumingCharacterControlEvent ev)
    {
        _controlledCharacter = ev.ControlledCharacter;
    }
}
