public abstract class MenuBase : Monobehaviour
{
    public virtual void Init()
    {
        // Override this method to initialize the even when turned off
    }
    protected void ToMainMenu()
    {
        if (MenuManager.Instance.PreviousMenu != MenuManager.Instance.MainMenu){
            MenuManager.SwitchMenu(MenuState.Pause);
            return;
        }
        MenuManager.SwitchMenu(MenuState.Main);
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

        protected void ToSettingsMenu()
    {
        MenuManager.SwitchMenu(MenuState.Settings);
    }

    protected virtual void QuitGame()
    {
        Application.Quit();
    }

}