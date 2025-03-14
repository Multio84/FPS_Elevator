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
    [SerializeField] TMP_InputField floorsInput;

    [SerializeField] Slider playerStartSlider;
    [SerializeField] TMP_InputField playerStartInput;

    [SerializeField] Slider elevatorStartSlider;
    [SerializeField] TMP_InputField elevatorStartInput;

    [SerializeField] Slider elevatorsSlider;
    [SerializeField] TMP_InputField elevatorsInput;

    [SerializeField] UnityEngine.UI.Button generateButton;
    [SerializeField] UnityEngine.UI.Button quitButton;


    private void Start()
    {
        generator = LevelGenerator.Instance;

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
        generator.floorsNumber = ProcessInputField(floorsInput, value);
        SetMaxFloor();
    }

    void OnFloorsInputFieldChanged(string input)
    {
        generator.floorsNumber = ProcessSlider(floorsSlider, floorsInput, input, MinFloor + 2, MaxFloor + 1);
        SetMaxFloor();
    }

    void OnPlayerStartSliderChanged(float value)
    {
        generator.playerStartFloor = ProcessInputField(playerStartInput, value);
    }

    void OnPlayerStartInputFieldChanged(string input)
    {
        generator.playerStartFloor = ProcessSlider(playerStartSlider, playerStartInput, input, MinFloor, (int)floorsSlider.value);
    }

    void OnElevatorStartSliderChanged(float value)
    {
        generator.elevatorStartFloor = ProcessInputField(elevatorStartInput, value);
    }

    void OnElevatorStartInputFieldChanged(string input)
    {
        generator.elevatorStartFloor = ProcessSlider(elevatorStartSlider, elevatorStartInput, input, MinFloor, (int)floorsSlider.value);
    }

    void OnElevatorsSliderChanged(float value)
    {
        generator.blocksNumber = ProcessInputField(elevatorsInput, value);
    }

    void OnElevatorsInputFieldChanged(string input)
    {
        generator.blocksNumber = ProcessSlider(elevatorsSlider, elevatorsInput, input, MinBlocks, MaxBlocks);
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
