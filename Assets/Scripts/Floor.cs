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

    int elevatorCurrentFloor;
    [SerializeField] TextMeshPro elevatorCurrentFloorText;


    private void Update()
    {
        SetElevatorCurrentFloorNumber();
    }

    public void SetNumber(int number)
    {
        this.number = number;
        numberText.text = "Level " + number.ToString();
    }

    public void SetElevatorCurrentFloorNumber()
    {
        elevatorCurrentFloorText.text = elevator.currentFloor.ToString();
    }
}
