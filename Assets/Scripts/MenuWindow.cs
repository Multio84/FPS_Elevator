using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class MenuWindow : MonoBehaviour
{
    int MinFloor = LevelGenerator.MinFloor;
    int MaxFloor = LevelGenerator.MaxFloor;
    int MinBlocks = LevelGenerator.MinBlocks;
    int MaxBlocks = LevelGenerator.MaxBlocks;
    LevelGenerator generator;
    int FloorsNumber => generator.floorsNumber;
    int PlayerStart => generator.playerStartFloor;
    int ElevatorStart => generator.elevatorStartFloor;
    int ElevatorsNumber => generator.blocksNumber;

    [SerializeField] Slider floorsSlider;
    [SerializeField] TMP_InputField floorsInputField;

    [SerializeField] Slider playerStartSlider;
    [SerializeField] TMP_InputField playerStartInputField;

    [SerializeField] Slider elevatorStartSlider;
    [SerializeField] TMP_InputField elevatorStartInputField;

    [SerializeField] Slider elevatorsSlider;
    [SerializeField] TMP_InputField elevatorsInputField;

    [SerializeField] UnityEngine.UI.Button generateButton;
    [SerializeField] UnityEngine.UI.Button quitButton;


    private void Start()
    {
        generator = LevelGenerator.Instance;

        InitProperty(floorsSlider, floorsInputField, MinFloor + 2, MaxFloor + 1, FloorsNumber);
        floorsSlider.onValueChanged.AddListener(OnFloorsSliderChanged);
        floorsInputField.onEndEdit.AddListener(OnFloorsInputFieldChanged);

        InitProperty(playerStartSlider, playerStartInputField, MinFloor, (int)floorsSlider.value - 1, PlayerStart);
        playerStartSlider.onValueChanged.AddListener(OnPlayerStartSliderChanged);
        playerStartInputField.onEndEdit.AddListener(OnPlayerStartInputFieldChanged);

        InitProperty(elevatorStartSlider, elevatorStartInputField, MinFloor, (int)floorsSlider.value - 1, ElevatorStart);
        elevatorStartSlider.onValueChanged.AddListener(OnElevatorStartSliderChanged);
        elevatorStartInputField.onEndEdit.AddListener(OnElevatorStartInputFieldChanged);

        InitProperty(elevatorsSlider, elevatorsInputField, MinBlocks, MaxBlocks, ElevatorsNumber);
        elevatorsSlider.onValueChanged.AddListener(OnElevatorsSliderChanged);
        elevatorsInputField.onEndEdit.AddListener(OnElevatorsInputFieldChanged);

        generateButton.onClick.AddListener(OnGenerateButtonPressed);
        quitButton.onClick.AddListener(OnQuitButtonPressed);
    }

    private void InitProperty(Slider slider, TMP_InputField inputField, int min, int max, int defaultValue)
    {
        slider.minValue = min;
        slider.maxValue = max;
        slider.value = defaultValue;
        inputField.text = defaultValue.ToString();
    }

    void OnFloorsSliderChanged(float value)
    {
        generator.floorsNumber = ProcessInputField(floorsInputField, value);
        SetMaxFloor();
    }

    void OnFloorsInputFieldChanged(string input)
    {
        generator.floorsNumber = ProcessSlider(floorsSlider, floorsInputField, input, MinFloor + 2, MaxFloor + 1);
        SetMaxFloor();
    }

    void OnPlayerStartSliderChanged(float value)
    {
        generator.playerStartFloor = ProcessInputField(playerStartInputField, value);
    }

    void OnPlayerStartInputFieldChanged(string input)
    {
        generator.playerStartFloor = ProcessSlider(playerStartSlider, playerStartInputField, input, MinFloor, (int)floorsSlider.value);
    }

    void OnElevatorStartSliderChanged(float value)
    {
        generator.elevatorStartFloor = ProcessInputField(elevatorStartInputField, value);
    }

    void OnElevatorStartInputFieldChanged(string input)
    {
        generator.elevatorStartFloor = ProcessSlider(elevatorStartSlider, elevatorStartInputField, input, MinFloor, (int)floorsSlider.value);
    }

    void OnElevatorsSliderChanged(float value)
    {
        generator.blocksNumber = ProcessInputField(elevatorsInputField, value);
    }

    void OnElevatorsInputFieldChanged(string input)
    {
        generator.blocksNumber = ProcessSlider(elevatorsSlider, elevatorsInputField, input, MinBlocks, MaxBlocks);
    }

    // change start player & elevator sliders max value
    // cause it can't be more than total floors in the building
    void SetMaxFloor()
    {
        int currentMaxFloor = (int)floorsSlider.value - 1;

        playerStartSlider.maxValue = currentMaxFloor;
        elevatorStartSlider.maxValue = currentMaxFloor;

        generator.playerStartFloor = currentMaxFloor;
        generator.elevatorStartFloor = currentMaxFloor;
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

    void OnGenerateButtonPressed()
    {
        GameManager.Instance.GenerateGame();
        UIManager.Instance.SetMenuActive(false);
    }

    void OnQuitButtonPressed()
    {
        Application.Quit();
        Debug.Log("Quit button pressed.");
    }

}
