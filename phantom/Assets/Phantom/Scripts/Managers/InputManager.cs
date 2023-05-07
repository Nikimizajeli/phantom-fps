using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    private IPlayerControlled _controlledCharacter;
    private IWeapon _selectedWeapon;

    protected void OnEnable()
    {
        EventDispatcher.Instance.AddListener<AssumingCharacterControlEvent>(OnAssumingCharacterControlEvent);
        EventDispatcher.Instance.AddListener<WeaponSelectedEvent>(OnWeaponSelectedEvent);
    }

    protected void OnDisable()
    {
        EventDispatcher.Instance.RemoveListener<AssumingCharacterControlEvent>(OnAssumingCharacterControlEvent);
        EventDispatcher.Instance.RemoveListener<WeaponSelectedEvent>(OnWeaponSelectedEvent);
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

    public void Fire(InputAction.CallbackContext context)
    {
        if (!context.performed)
        {
            return;
        }
        
        _selectedWeapon?.Fire();
    }

    public void Reload(InputAction.CallbackContext context)
    {
        if (!context.performed)
        {
            return;
        }
        
        _selectedWeapon?.Reload();
    }

    private void OnAssumingCharacterControlEvent(AssumingCharacterControlEvent ev)
    {
        _controlledCharacter = ev.ControlledCharacter;
    }

    private void OnWeaponSelectedEvent(WeaponSelectedEvent ev)
    {
        _selectedWeapon = ev.SelectedWeapon;
    }
}
