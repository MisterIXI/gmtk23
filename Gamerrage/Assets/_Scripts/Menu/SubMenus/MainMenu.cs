using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MenuBase
{
    [field: SerializeField] public Button StartGameButton { get; private set; }
    [field: SerializeField] public Button SettingsButton { get; private set; }
    [field: SerializeField] public Button ControlsButton { get; private set; }
    [field: SerializeField] public Button CreditsButton { get; private set; }
    [field: SerializeField] public Button QuitButton { get; private set; }

    public override void Init()
    {
        StartGameButton.onClick.AddListener(ToEditorMenu);
        SettingsButton.onClick.AddListener(ToSettingsMenu);
        ControlsButton.onClick.AddListener(ToControlsMenu);
        CreditsButton.onClick.AddListener(ToCreditsMenu);
        if (Application.platform == RuntimePlatform.WebGLPlayer)
            DisableQuitButton();
        #if UNITY_EDITOR    
            DisableQuitButton();
        #endif
        QuitButton.onClick.AddListener(QuitGame);
    }

    private void DisableQuitButton()
    {
        QuitButton.interactable = false;
    }
    // public override void SelectFirst()
    // {
    //     LevelSelectButton.Select();
    // }
}