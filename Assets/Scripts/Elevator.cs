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
    float acceleration = 4f;
    public int currentFloor = 0;
    public bool isMoving = false;

    void Awake()
    {
        generator = LevelGenerator.Instance;
        UpdateCurrentFloor();
        SpawnButtons();
    }

    public void SpawnButtons()
    {
        int totalFloors = generator.totalFloors;

        for (int floor = 0; floor <= totalFloors; floor++)
        {
            int column = floor / 10;
            int row = floor % 10;
            Vector3 buttonPosition = new Vector3(column * horizontalButtonSpacing, row * verticalButtonSpacing, 0f);

            GameObject newButton = Instantiate(elevatorButtonPrefab, buttonsStartpoint);
            newButton.transform.localPosition = buttonPosition;

            ElevatorButton buttonComponent = newButton.GetComponent<ElevatorButton>();
            buttonComponent.elevator = this;
            buttonComponent.floorNumber = floor;
            buttonComponent.buttonText.text = floor.ToString();
        }
    }

    public void MoveTo(int floorNumber)
    {
        isMoving = true;
        Floor floor = LevelGenerator.Instance.building[floorNumber];
        Transform target = floor.elevatorStartpoint;

        StartCoroutine(AnimateElevator(target));
    }

    void UpdateCurrentFloor()
    {
        float elevatorY = transform.position.y;
        int newFloor = Mathf.RoundToInt(elevatorY / LevelGenerator.FloorHeight);

        if (newFloor != currentFloor)
        {
            currentFloor = newFloor;
            Debug.Log("Current floor is: " + currentFloor);
        }
    }

    public IEnumerator AnimateElevator(Transform target)
    {
        Vector3 startPos = transform.position;
        Vector3 targetPos = new Vector3(startPos.x, target.position.y, startPos.z);
        float distance = Mathf.Abs(targetPos.y - startPos.y);

        if (distance < 0.001f)
            yield break;

        float duration = 2f * Mathf.Sqrt(distance / acceleration);
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            float easedT = (1f - Mathf.Cos(t * Mathf.PI)) / 2f;

            transform.position = Vector3.Lerp(startPos, targetPos, easedT);

            UpdateCurrentFloor();

            yield return null;
        }

        transform.position = targetPos;

        isMoving = false;
        ButtonManager.TurnOffPressedButton();
    }
}

