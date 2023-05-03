using UnityEngine;

public interface IMovableCharacter
{
    void Move(Vector2 direction);
    void Jump();
    void Sprint(bool sprint);
    void LookAround(Vector2 direction);
}
