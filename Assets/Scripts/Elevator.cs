using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Elevator : MonoBehaviour
{
    LevelGenerator generator;

    [SerializeField] Transform buttonsStartpoint;
    [SerializeField] GameObject elevatorButtonPrefab;
    float verticalButtonSpacing = 0.185f;
    float horizontalButtonSpacing = 0.24f;
    

    void Awake()
    {
        generator = LevelGenerator.Instance;
        SpawnButtons();
    }

    public void SpawnButtons()
    {
        int totalFloors = generator.totalFloors;

        for (int floor = 1; floor <= totalFloors; floor++)
        {
            int column = (floor - 1) / 10;
            int row = (floor - 1) % 10;
            Vector3 buttonPosition = new Vector3(column * horizontalButtonSpacing, row * verticalButtonSpacing, 0f);

            GameObject newButton = Instantiate(elevatorButtonPrefab, buttonsStartpoint);
            newButton.transform.localPosition = buttonPosition;

            ElevatorButton buttonComponent = newButton.GetComponent<ElevatorButton>();
            //Button buttonComponent = newButton.GetComponent<Button>();
            buttonComponent.elevator = this;
            buttonComponent.floorNumber = floor;
            buttonComponent.buttonText.text = floor.ToString();
        }
    }
}

