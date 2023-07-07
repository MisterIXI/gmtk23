using UnityEngine;

public class SettingsHolder : MonoBehaviour
{
    [field: SerializeField] public UtilitySettings UtilitySettings { get; private set; }
    private static SettingsHolder _instance;
    public static SettingsHolder Instance
    {
        get
        {
            if (_instance == null)
                _instance = GameObject.FindObjectOfType<SettingsHolder>();
            return _instance;
        }
        set
        {
            _instance = value;
        }
    }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
    }
}