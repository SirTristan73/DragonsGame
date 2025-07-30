using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class InputManager : PersistentSingleton<InputManager>
{
    public event Action<float> _fire;
    public event Action<Vector2> _move;
    public event Action<float> _onEscape;


  
    public void FireCallbackContext(InputAction.CallbackContext ctx)
    {
        _fire?.Invoke(ctx.ReadValue<float>());
    }


    public void OnMoveCallbackContext(InputAction.CallbackContext ctx)
    {
        _move?.Invoke(ctx.ReadValue<Vector2>());
    }


    public void OnEscapeCallbackContext(InputAction.CallbackContext ctx)
    {
        _onEscape?.Invoke(ctx.ReadValue<float>());
    }
}
