using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance { get; private set; }
    [field: SerializeField] public EventSystem EventSystem { get; private set; }
    [field: SerializeField] public MenuBase MainMenu { get; private set; }
    [field: SerializeField] public MenuBase Editor { get; private set; }
    [field: SerializeField] public MenuBase HUD { get; private set; }
    [field: SerializeField] public MenuBase Pause { get; private set; }
    [field: SerializeField] public MenuBase Controls { get; private set; }
    [field: SerializeField] public MenuBase Credits { get; private set; }

    public MenuBase CurrentMenu { get; private set; }
    public MenuBase PreviousMenu { get; private set; }
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

        MainMenu.Init();
        Editor.Init();
        HUD.Init();
        Pause.Init();
        Controls.Init();
        Credits.Init();
        CurrentMenu = MainMenu;
        InputManager.OnPause += OnPauseButtonInput;
    }

    private void OnPauseButtonInput(InputAction.CallbackContext context)
    {
        if (context.performed)
            Pause.PauseInput();
    }
    public static void SwitchMenu(MenuState menuState)
    {
        Debug.Log($"Switching to {menuState}");
        Instance.CurrentMenu?.gameObject.SetActive(false);
        Instance.PreviousMenu = Instance.CurrentMenu;
        switch (menuState)
        {
            case MenuState.Main:
                Instance.CurrentMenu = Instance.MainMenu;
                break;
            case MenuState.Editor:
                Instance.CurrentMenu = Instance.Editor;
                break;
            case MenuState.HUD:
                Instance.CurrentMenu = Instance.HUD;
                break;
            case MenuState.Pause:
                Instance.CurrentMenu = Instance.Pause;
                break;
            case MenuState.Controls:
                Instance.CurrentMenu = Instance.Controls;
                break;
            case MenuState.Credits:
                Instance.CurrentMenu = Instance.Credits;
                break;
        }
        Instance.CurrentMenu.SelectFirst();
        Instance.CurrentMenu.gameObject.SetActive(true);
    }

    // private void OnDestroy()
    // {
    //     if (Instance == this)
    //     {
    //         Instance = null;
    //         InputManager.OnPause -= OnPauseButtonInput;
    //     }

    // }
}