using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelGeneratorUI : MonoBehaviour
{
    [SerializeField] Slider totalSlider;
    [SerializeField] TMP_InputField totalInputField;

    [SerializeField] Slider playerStartSlider;
    [SerializeField] TMP_InputField playerStartInputField;

    [SerializeField] Slider elevatorStartSlider;
    [SerializeField] TMP_InputField elevatorStartInputField;

    [SerializeField] Button generateButton;

    private void Start()
    {
        totalSlider.minValue = LevelGenerator.MinFloor + 2;
        totalSlider.maxValue = LevelGenerator.MaxFloor + 1;
        totalSlider.value = 5;
        totalInputField.text = ((int)totalSlider.value).ToString();

        totalSlider.onValueChanged.AddListener(OnTotalSliderChanged);
        totalInputField.onEndEdit.AddListener(OnTotalInputFieldChanged);

        playerStartSlider.minValue = LevelGenerator.MinFloor;
        playerStartSlider.maxValue = totalSlider.value;
        playerStartSlider.value = 0;
        playerStartInputField.text = ((int)playerStartSlider.value).ToString();

        playerStartSlider.onValueChanged.AddListener(OnPlayerStartSliderChanged);
        playerStartInputField.onEndEdit.AddListener(OnPlayerStartInputFieldChanged);

        elevatorStartSlider.minValue = LevelGenerator.MinFloor;
        elevatorStartSlider.maxValue = totalSlider.value;
        elevatorStartSlider.value = 0;
        elevatorStartInputField.text = ((int)elevatorStartSlider.value).ToString();

        elevatorStartSlider.onValueChanged.AddListener(OnElevatorStartSliderChanged);
        elevatorStartInputField.onEndEdit.AddListener(OnElevatorStartInputFieldChanged);
    }


    void OnTotalSliderChanged(float value)
    {
        totalInputField.text = ((int)value).ToString();
        SetMaxFloor();
    }

    void OnTotalInputFieldChanged(string input)
    {
        int value;
        if (int.TryParse(input, out value))
        {
            value = Mathf.Clamp(value, LevelGenerator.MinFloor + 2, LevelGenerator.MaxFloor + 1);
            
            totalSlider.value = value;
            totalInputField.text = value.ToString();

            SetMaxFloor();
        }
        else
        {
            totalInputField.text = ((int)totalSlider.value).ToString();
        }
    }

    void SetMaxFloor()
    {
        playerStartSlider.maxValue = totalSlider.value;
        elevatorStartSlider.maxValue = totalSlider.value;
    }

    void OnPlayerStartSliderChanged(float value)
    {
        playerStartInputField.text = ((int)value).ToString();
    }

    void OnPlayerStartInputFieldChanged(string input)
    {
        int value;
        if (int.TryParse(input, out value))
        {
            value = Mathf.Clamp(value, LevelGenerator.MinFloor, (int)totalSlider.value);
            playerStartSlider.value = value;
            playerStartInputField.text = value.ToString();
        }
        else
        {
            playerStartInputField.text = playerStartSlider.value.ToString();
        }
    }

    void OnElevatorStartSliderChanged(float value)
    {
        elevatorStartInputField.text = ((int)value).ToString();
    }

    void OnElevatorStartInputFieldChanged(string input)
    {
        int value;
        if (int.TryParse(input, out value))
        {
            value = Mathf.Clamp(value, LevelGenerator.MinFloor, (int)totalSlider.value);
            elevatorStartSlider.value = value;
            elevatorStartInputField.text = value.ToString();
        }
        else
        {
            elevatorStartInputField.text = elevatorStartSlider.value.ToString();
        }
    }
}
