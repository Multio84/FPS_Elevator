using TMPro;
using UnityEngine;


public class Floor : MonoBehaviour
{
    public Transform playerStartpoint;
    public Transform elevatorStartpoint;
    public FloorCallButton floorCallButton;
    [HideInInspector] public Elevator elevator;
    [HideInInspector] public int number;
    [SerializeField] TextMeshPro numberText;
    [SerializeField] TextMeshPro elevatorCurrentFloorText;


    void Start()
    {
        SetElevatorCurrentFloorText();
    }

    void Update()
    {
        if (elevator.isMoving)
            SetElevatorCurrentFloorText();
    }

    public void SetNumber(int number)
    {
        this.number = number;
        numberText.text = "Level " + number.ToString();
    }

    public void SetElevatorCurrentFloorText()
    {
        elevatorCurrentFloorText.text = elevator.currentFloor.ToString();
    }
}
