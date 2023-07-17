using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MenuBase
{
    [field: SerializeField] public Button ResumeButton { get; private set; }
    [field: SerializeField] public Button RetryButton { get; private set; }
    [field: SerializeField] public Button BackToMenuButton { get; private set; }
    [field: SerializeField] public Button VolumeButton { get; private set; }
    [field: SerializeField] public Slider VolumeSlider { get; private set; }
    public static float Volume { get; private set; }

    public override void Init(){
        ResumeButton.onClick.AddListener(OnResume);
        RetryButton.onClick.AddListener(OnRetry);
        BackToMenuButton.onClick.AddListener(OnBackToMenu);
        VolumeButton.onClick.AddListener(ChangeVolume);
        Volume = PlayerPrefs.GetFloat("Volume", 0.3f);
        VolumeSlider.value = Volume;
        VolumeSlider.onValueChanged.AddListener(OnVolumeChanged);
    }

  

    private void OnVolumeChanged(float value)
    {
        Volume = value;
    }

    private void ChangeVolume(){
        Volume = 0;
    }
}