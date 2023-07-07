using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

[RequireComponent(typeof(PlayerInput))]
public class InputManager : MonoBehaviour
{
    public static InputManager Instance;
    private PlayerInput _playerInput;
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        transform.SetParent(null);
        DontDestroyOnLoad(gameObject);
        _playerInput = GetComponent<PlayerInput>();
        SubscribeToInput();
    }
    public static event Action<CallbackContext> OnMousePos;
    public static event Action<CallbackContext> OnLMB;
    public static event Action<CallbackContext> OnRMB;
    public static event Action<CallbackContext> OnScroll;
    public static event Action<CallbackContext> OnPause;

    private void OnMousePosInput(CallbackContext context)
    {
        OnMousePos?.Invoke(context);
    }

    private void OnLMBInput(CallbackContext context)
    {
        OnLMB?.Invoke(context);
    }

    private void OnRMBInput(CallbackContext context)
    {
        OnRMB?.Invoke(context);
    }

    private void OnScrollInput(CallbackContext context)
    {
        OnScroll?.Invoke(context);
    }

    private void OnPauseInput(CallbackContext context)
    {
        OnPause?.Invoke(context);
    }

    private void SubscribeToInput()
    {
        _playerInput.actions["MousePos"].started += OnMousePosInput;
        _playerInput.actions["MousePos"].performed += OnMousePosInput;
        _playerInput.actions["MousePos"].canceled += OnMousePosInput;

        _playerInput.actions["LMB"].started += OnLMBInput;
        _playerInput.actions["LMB"].performed += OnLMBInput;
        _playerInput.actions["LMB"].canceled += OnLMBInput;

        _playerInput.actions["RMB"].started += OnRMBInput;
        _playerInput.actions["RMB"].performed += OnRMBInput;
        _playerInput.actions["RMB"].canceled += OnRMBInput;

        _playerInput.actions["Scroll"].started += OnScrollInput;
        _playerInput.actions["Scroll"].performed += OnScrollInput;
        _playerInput.actions["Scroll"].canceled += OnScrollInput;

        _playerInput.actions["Pause"].started += OnPauseInput;
        _playerInput.actions["Pause"].performed += OnPauseInput;
        _playerInput.actions["Pause"].canceled += OnPauseInput;
    }

    private void UnSubscribeToInput()
    {
        _playerInput.actions["MousePos"].started -= OnMousePosInput;
        _playerInput.actions["MousePos"].performed -= OnMousePosInput;
        _playerInput.actions["MousePos"].canceled -= OnMousePosInput;

        _playerInput.actions["LMB"].started -= OnLMBInput;
        _playerInput.actions["LMB"].performed -= OnLMBInput;
        _playerInput.actions["LMB"].canceled -= OnLMBInput;

        _playerInput.actions["RMB"].started -= OnRMBInput;
        _playerInput.actions["RMB"].performed -= OnRMBInput;
        _playerInput.actions["RMB"].canceled -= OnRMBInput;

        _playerInput.actions["Scroll"].started -= OnScrollInput;
        _playerInput.actions["Scroll"].performed -= OnScrollInput;
        _playerInput.actions["Scroll"].canceled -= OnScrollInput;

        _playerInput.actions["Pause"].started -= OnPauseInput;
        _playerInput.actions["Pause"].performed -= OnPauseInput;
        _playerInput.actions["Pause"].canceled -= OnPauseInput;
    }


    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
            UnSubscribeToInput();
        }
    }
}
