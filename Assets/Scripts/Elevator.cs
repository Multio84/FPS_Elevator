using System.Collections;
using TMPro;
using UnityEngine;


public class Elevator : MonoBehaviour
{
    LevelGenerator generator;
    [SerializeField] Transform buttonsStartpoint;
    [SerializeField] GameObject elevatorButtonPrefab;
    [SerializeField] TextMeshPro currentFloorText;
    float verticalButtonSpacing = 0.185f;
    float horizontalButtonSpacing = 0.24f;
    float acceleration = 5f;
    public Block blockOwner;    // the block to which the elevator belongs
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
        int totalFloors = generator.floorsNumber;

        for (int floor = 0; floor < totalFloors; floor++)
        {
            int column = floor / 10;
            int row = floor % 10;
            Vector3 buttonPosition = new Vector3(column * horizontalButtonSpacing, row * verticalButtonSpacing, 0f);

            GameObject buttonObj = Instantiate(elevatorButtonPrefab, buttonsStartpoint);
            buttonObj.name = "Button_" + floor;
            buttonObj.transform.localPosition = buttonPosition;

            ElevatorButton button = buttonObj.GetComponent<ElevatorButton>();
            button.elevator = this;
            button.floorNumber = floor;
            button.buttonText.text = floor.ToString();
        }
    }

    public void MoveTo(int floorNumber)
    {
        isMoving = true;
        Floor floor = blockOwner.Floors[floorNumber];
        Transform target = floor.elevatorStartpoint;

        StartCoroutine(AnimateElevator(target));
    }

    void UpdateCurrentFloor()
    {
        float elevatorY = transform.position.y;
        int newFloor = Mathf.RoundToInt(elevatorY / Block.FloorHeight);

        if (currentFloor != newFloor)
            currentFloor = newFloor;

        currentFloorText.text = currentFloor.ToString();
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

