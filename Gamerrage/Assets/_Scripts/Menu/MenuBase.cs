using UnityEngine;
public abstract class MenuBase : MonoBehaviour
{

    public virtual void Init()
    {
        // Override this method to initialize the even when turned off
    }

    private GameState _prePauseState;

    protected void ToMainMenu()
    {
        if (MenuManager.Instance.CurrentMenu == MenuManager.Instance.MainMenu)
        {
            MenuManager.SwitchMenu(MenuState.Pause);
            return;
        }
        MenuManager.SwitchMenu(MenuState.Main);
    }

    public void PauseInput()
    {
        if (GameManager.GameState == GameState.EditingLevel || GameManager.GameState == GameState.StreamerPlaying)
        {
            _prePauseState = GameManager.GameState;
            GameManager.ChangeGameState(GameState.Paused);
            MenuManager.SwitchMenu(MenuState.Pause);
        }
        else if (GameManager.GameState == GameState.Paused)
        {
            GameManager.ChangeGameState(_prePauseState);
            if (_prePauseState == GameState.EditingLevel)
                MenuManager.SwitchMenu(MenuState.Editor);
            else
                MenuManager.SwitchMenu(MenuState.HUD);
        }
    }

    protected void UnPause()
    {

    }

    protected void ToEditorMenu()
    {
        MenuManager.SwitchMenu(MenuState.Editor);
    }

    protected virtual void ToHUD()
    {
        MenuManager.SwitchMenu(MenuState.HUD);
    }

    protected virtual void ToCreditsMenu()
    {
        MenuManager.SwitchMenu(MenuState.Credits);
    }

    protected virtual void ToPauseMenu()
    {
        MenuManager.SwitchMenu(MenuState.Pause);
    }

    protected void ToControlsMenu()
    {
        MenuManager.SwitchMenu(MenuState.Controls);
    }

    protected virtual void QuitGame()
    {
        Application.Quit();
    }

    // public abstract void SelectFirst();
    public void SelectFirst()
    {
        MenuManager.Instance.EventSystem.SetSelectedGameObject(null);
    }
}