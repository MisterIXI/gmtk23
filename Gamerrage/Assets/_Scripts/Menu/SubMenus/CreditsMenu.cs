using UnityEngine;
using UnityEngine.UI;

public class CreditsMenu : MenuBase
{
    [field: SerializeField] public Button BackButton { get; private set; }

    public override void Init()
    {
        BackButton.onClick.AddListener(ToMainMenu);
    }
    // public override void SelectFirst()
    // {
    //     BackButton.Select();
    // }
}