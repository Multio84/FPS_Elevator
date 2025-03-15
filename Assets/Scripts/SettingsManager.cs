using UnityEngine;


public class SettingsManager : MonoBehaviour
{
    public static SettingsManager Instance { get; private set; }

    const string MouseSensKey = "MouseSensitivity";
    public const float MinMouseSensitivity = 0.1f;
    public const float MaxMouseSensitivity = 6f;

    [SerializeField] float defaultMouseSensitivity = 1f;
    public float MouseSensitivity
    {
        get => defaultMouseSensitivity;
        set
        {
            if (defaultMouseSensitivity != value)
            {
                defaultMouseSensitivity = value;
                PlayerPrefs.SetFloat(MouseSensKey, defaultMouseSensitivity);
                PlayerPrefs.Save();
                OnMouseSensitivityChanged?.Invoke(defaultMouseSensitivity);
            }
        }
    }

    public event System.Action<float> OnMouseSensitivityChanged;


    public void Init()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        defaultMouseSensitivity = PlayerPrefs.GetFloat(MouseSensKey, defaultMouseSensitivity);
    }
}
