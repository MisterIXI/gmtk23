using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static GameState;
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public static GameState GameState => Instance._state;
    public static Action<GameState, GameState> OnGameStateChange;
    private GameState _state;
    private float _prePauseTimescale;
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        SubscribeEvents();
    }

    public static void ChangeGameState(GameState newState) => Instance.SwitchGameState(newState);
    private void SwitchGameState(GameState newState)
    {
        // no change
        if (_state == newState)
            return;
        GameState oldstate = _state;
        _state = newState;
        // when pausing
        if (newState == Paused)
        {
            _prePauseTimescale = Time.timeScale;
            Time.timeScale = 0f;
        }
        // unpausing into other states
        if (oldstate == Paused && newState != Validating)
            _prePauseTimescale = 1f;
        // unpausing
        if (oldstate == Paused)
            Time.timeScale = _prePauseTimescale;
        if (oldstate == EditingLevel && newState == Validating)
            MapValidator.Instance.ValidateAndMapLevelData();
        if (oldstate == Menu && newState == EditingLevel)
            LevelCreator.Instance.CreateData(new(25, 54));
        // switching to streamer first time
        if (oldstate == Validating && newState == StreamerPlaying)
        {
            MapValidator.Instance.FindAndFollowPath();
        }
        OnGameStateChange?.Invoke(oldstate, _state);
    }

    public void UpdateValidationState(bool IsValidAndReady)
    {
        if (_state == Validating)
        {
            if (!IsValidAndReady)
                SwitchGameState(EditingLevel);
            else
                SwitchGameState(StreamerPlaying);
        }
    }


    private void PauseGame()
    {
        //already paused
        if (_state == Paused)
            return;

        MenuManager.SwitchMenu(MenuState.Pause);
    }
    private void UnPauseGame()
    {
        // not paused anyway
        if (_state != Paused)
            return;
    }

    private void OnPauseInput(InputAction.CallbackContext context)
    {
        if (context.performed)
        {

        }
    }

    private void SubscribeEvents()
    {
        InputManager.OnPause += OnPauseInput;
    }

    private void UnSubscribeEvents()
    {
        InputManager.OnPause -= OnPauseInput;
    }

    private void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
        UnSubscribeEvents();
    }
}
public enum GameState
{
    Menu,
    EditingLevel,
    Validating,
    StreamerPlaying,
    Paused
}