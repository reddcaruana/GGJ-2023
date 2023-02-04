using UnityEngine.InputSystem;

public interface IBindable
{
    int ID { get; }

    void Bind(PlayerInput playerInput);
    void Release();
}