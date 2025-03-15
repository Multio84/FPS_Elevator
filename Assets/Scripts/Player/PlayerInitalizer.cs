using StarterAssets;
using UnityEngine;


public class PlayerInitalizer : MonoBehaviour
{
    FirstPersonController fpsController;


    public void Init()
    {
        fpsController = GetComponent<FirstPersonController>();
        fpsController.RotationSpeed = SettingsManager.Instance.MouseSensitivity;
    }

    void OnEnable()
    {
        SettingsManager.Instance.OnMouseSensitivityChanged += UpdateRotationSpeed;
    }

    void OnDisable()
    {
        if (SettingsManager.Instance != null)
            SettingsManager.Instance.OnMouseSensitivityChanged -= UpdateRotationSpeed;
    }

    void UpdateRotationSpeed(float newSensitivity)
    {
        fpsController.RotationSpeed = newSensitivity;
    }
}
