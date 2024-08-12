using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(menuName = "SO/Player/Data/Input")]
public class InputDataSO : ScriptableObject, InputData.ISniperActions
{
    public event Action<Vector2> OnMouseMoveEvt;
    public event Action OnMouseRightButtonDownEvt;
    public event Action OnMouseRightButtonUpEvt;
    public event Action OnMouseLeftButtonDownEvt;
    public event Action OnMouseLeftButtonUpEvt;

    private InputData inputData;

    public InputDataSO Init()
    {
        var data = Instantiate(this);
        data.Enable();

        return data;
    }

    public void Enable()
    {
        if (inputData == null)
        {
            inputData = new InputData();
        }

        inputData.Sniper.SetCallbacks(this);
        inputData.Sniper.Enable();

    }

    public void OnMouseMove(InputAction.CallbackContext context)
    {
        Vector2 vec = context.ReadValue<Vector2>();
        OnMouseMoveEvt?.Invoke(vec);
    }

    public void OnLeftButton(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            OnMouseLeftButtonDownEvt?.Invoke();
        }
        else if (context.canceled)
        {
            OnMouseLeftButtonUpEvt?.Invoke();
        }
    }

    public void OnRightButton(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            OnMouseRightButtonDownEvt?.Invoke();
        }
        else if (context.canceled)
        {
            OnMouseRightButtonUpEvt?.Invoke();
        }
    }
}
