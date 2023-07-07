using UnityEngine;

public class SettingsHolder : MonoBehaviour
{
    [field: SerializeField] public UtilitySettings UtilitySettings { get; private set; }
    public static SettingsHolder Instance
    {
        get
        {
            if (Instance == null)
                Instance = GameObject.FindObjectOfType<SettingsHolder>();
            return Instance;
        }
        set
        {
            Instance = value;
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