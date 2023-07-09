using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static GameState;

public class EditorMenu : MenuBase
{
    [field: SerializeField] public Button ValidateButton { get; private set; }
    [field: SerializeField] public TextMeshProUGUI InfoText { get; private set; }
    private TextMeshProUGUI buttonText;
    public override void Init()
    {
        base.Init();
        ValidateButton.onClick.AddListener(OnValidationButton);
        buttonText = ValidateButton.gameObject.GetComponent<TextMeshProUGUI>();
        SubscribeEvents();
    }
    public void OnGameStateChange(GameState oldstate, GameState newstate)
    {
        if (newstate == GameState.Validating)
        {
            ValidateButton.enabled = false;
            buttonText.text = "Validating...";
        }
        if (newstate == GameState.EditingLevel)
        {
            ValidateButton.enabled = true;
            buttonText.text = "Validate and Submit";
            if (oldstate != GameState.Validating)
            {
                InfoText.gameObject.SetActive(false);
                Debug.Log("disabling msg");
            }
        }
        if (oldstate == Validating && newstate == EditingLevel)
        {
            InfoText.gameObject.SetActive(true);
            InfoText.text = MapValidator.Instance.RejectionReason;
            Debug.Log("Error msg active");
        }
    }
    public void OnValidationButton()
    {
        InfoText.gameObject.SetActive(false);
        GameManager.ChangeGameState(GameState.Validating);
    }

    private void SubscribeEvents()
    {
        GameManager.OnGameStateChange += OnGameStateChange;
    }

    private void UnSubscribeEvents()
    {
        GameManager.OnGameStateChange -= OnGameStateChange;
    }

    private void OnDestroy()
    {
        UnSubscribeEvents();
    }
}