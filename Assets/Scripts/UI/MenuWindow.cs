using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;


public class MenuWindow : MonoBehaviour
{
    int MinFloor = LevelGenerator.MinFloor;
    int MaxFloor = LevelGenerator.MaxFloor;
    int MinBlocks = LevelGenerator.MinBlocks;
    int MaxBlocks = LevelGenerator.MaxBlocks;
    int FloorsNumber => LevelGenerator.floorsNumber;
    int PlayerStart => LevelGenerator.playerStartFloor;
    int ElevatorStart => LevelGenerator.elevatorStartFloor;
    int ElevatorsNumber => LevelGenerator.blocksNumber;

    [SerializeField] Slider floorsSlider;
    [SerializeField] TMP_InputField floorsInput;

    [SerializeField] Slider playerStartSlider;
    [SerializeField] TMP_InputField playerStartInput;

    [SerializeField] Slider elevatorStartSlider;
    [SerializeField] TMP_InputField elevatorStartInput;

    [SerializeField] Slider elevatorsSlider;
    [SerializeField] TMP_InputField elevatorsInput;

    [SerializeField] UnityEngine.UI.Button generateButton;
    [SerializeField] UnityEngine.UI.Button quitButton;

    [SerializeField] Slider sensitivitySlider;
    [SerializeField] TMP_Text sensitivityTextValue;


    void OnEnable()
    {
        InitProperty(floorsSlider, floorsInput, MinFloor + 2, MaxFloor + 1, FloorsNumber);
        floorsSlider.onValueChanged.AddListener(OnFloorsSliderChanged);
        floorsInput.onEndEdit.AddListener(OnFloorsInputFieldChanged);

        InitProperty(playerStartSlider, playerStartInput, MinFloor, (int)floorsSlider.value - 1, PlayerStart);
        playerStartSlider.onValueChanged.AddListener(OnPlayerStartSliderChanged);
        playerStartInput.onEndEdit.AddListener(OnPlayerStartInputFieldChanged);

        InitProperty(elevatorStartSlider, elevatorStartInput, MinFloor, (int)floorsSlider.value - 1, ElevatorStart);
        elevatorStartSlider.onValueChanged.AddListener(OnElevatorStartSliderChanged);
        elevatorStartInput.onEndEdit.AddListener(OnElevatorStartInputFieldChanged);

        InitProperty(elevatorsSlider, elevatorsInput, MinBlocks, MaxBlocks, ElevatorsNumber);
        elevatorsSlider.onValueChanged.AddListener(OnElevatorsSliderChanged);
        elevatorsInput.onEndEdit.AddListener(OnElevatorsInputFieldChanged);

        generateButton.onClick.AddListener(GameManager.Instance.CreateGame);
        quitButton.onClick.AddListener(OnQuitButtonPressed);

        sensitivitySlider.maxValue = SettingsManager.MaxMouseSensitivity;
        sensitivitySlider.minValue = SettingsManager.MinMouseSensitivity;
        sensitivitySlider.value = SettingsManager.Instance.MouseSensitivity;
        sensitivitySlider.onValueChanged.AddListener(OnSensitivitySliderChanged);
        SetSensitivityTextValue(sensitivitySlider.value);
    }

    void OnDisable()
    {
        floorsSlider.onValueChanged.RemoveListener(OnFloorsSliderChanged);
        floorsInput.onEndEdit.RemoveListener(OnFloorsInputFieldChanged);

        playerStartSlider.onValueChanged.RemoveListener(OnPlayerStartSliderChanged);
        playerStartInput.onEndEdit.RemoveListener(OnPlayerStartInputFieldChanged);

        elevatorStartSlider.onValueChanged.RemoveListener(OnElevatorStartSliderChanged);
        elevatorStartInput.onEndEdit.RemoveListener(OnElevatorStartInputFieldChanged);

        elevatorsSlider.onValueChanged.RemoveListener(OnElevatorsSliderChanged);
        elevatorsInput.onEndEdit.RemoveListener(OnElevatorsInputFieldChanged);

        generateButton.onClick.RemoveListener(GameManager.Instance.CreateGame);
        quitButton.onClick.RemoveListener(OnQuitButtonPressed);

        sensitivitySlider.onValueChanged.RemoveListener(OnSensitivitySliderChanged);
    }

    void InitProperty(Slider slider, TMP_InputField inputField, int min, int max, int defaultValue)
    {
        slider.minValue = min;
        slider.maxValue = max;
        slider.value = defaultValue;
        inputField.text = defaultValue.ToString();
    }

    void OnFloorsSliderChanged(float value)
    {
        LevelGenerator.floorsNumber = ProcessInputField(floorsInput, value);
        SetMaxFloor();
    }

    void OnFloorsInputFieldChanged(string input)
    {
        LevelGenerator.floorsNumber = ProcessSlider(floorsSlider, floorsInput, input, MinFloor + 2, MaxFloor + 1);
        SetMaxFloor();
    }

    void OnPlayerStartSliderChanged(float value)
    {
        LevelGenerator.playerStartFloor = ProcessInputField(playerStartInput, value);
    }

    void OnPlayerStartInputFieldChanged(string input)
    {
        LevelGenerator.playerStartFloor = ProcessSlider(playerStartSlider, playerStartInput, input, MinFloor, (int)floorsSlider.value);
    }

    void OnElevatorStartSliderChanged(float value)
    {
        LevelGenerator.elevatorStartFloor = ProcessInputField(elevatorStartInput, value);
    }

    void OnElevatorStartInputFieldChanged(string input)
    {
        LevelGenerator.elevatorStartFloor = ProcessSlider(elevatorStartSlider, elevatorStartInput, input, MinFloor, (int)floorsSlider.value);
    }

    void OnElevatorsSliderChanged(float value)
    {
        LevelGenerator.blocksNumber = ProcessInputField(elevatorsInput, value);
    }

    void OnElevatorsInputFieldChanged(string input)
    {
        LevelGenerator.blocksNumber = ProcessSlider(elevatorsSlider, elevatorsInput, input, MinBlocks, MaxBlocks);
    }

    // change start player & elevator sliders max value
    // cause it can't be more than total floors in the building
    void SetMaxFloor()
    {
        int currentMaxFloor = (int)floorsSlider.value - 1;

        playerStartSlider.maxValue = currentMaxFloor;
        elevatorStartSlider.maxValue = currentMaxFloor;
    }

    int ProcessInputField(TMP_InputField field, float value)
    {
        int intValue = (int)value;
        field.text = intValue.ToString();
        
        return intValue;
    }

    int ProcessSlider(Slider slider, TMP_InputField field, string input, int min, int max)
    {
        int intValue;
        if (int.TryParse(input, out intValue))
        {
            intValue = Mathf.Clamp(intValue, min, max);
            slider.value = intValue;
            field.text = intValue.ToString();
        }
        else
        {
            field.text = slider.value.ToString();
        }

        return intValue;
    }

    void OnSensitivitySliderChanged(float value)
    {
        SetSensitivityTextValue(value);
        SettingsManager.Instance.MouseSensitivity = value;
    }

    void SetSensitivityTextValue(float value)
    { 
        sensitivityTextValue.text = ((float)Math.Round(value, 1)).ToString();
    }

    void OnQuitButtonPressed()
    {
        Application.Quit();
    }

}
