using UnityEngine;


public class Block
{
    public Floor[] Floors { get; private set; }
    public Elevator Elevator { get; private set; }

    public Transform blockRoot;
    GameObject floorPrefab;
    GameObject elevatorPrefab;
    public const float FloorHeight = 6f;
    int floorsNumber;
    int elevatorStartFloor;


    public Block(Transform blockRoot, GameObject floorPrefab, GameObject elevatorPrefab, int floorsNumber, int elevatorStartFloor)
    {
        this.blockRoot = blockRoot;
        this.floorPrefab = floorPrefab;
        this.elevatorPrefab = elevatorPrefab;
        this.floorsNumber = floorsNumber;
        this.elevatorStartFloor = elevatorStartFloor;

        ConstructBlock();
        SpawnElevator();
        InitializeFloors();
    }

    void ConstructBlock()
    {
        Floors = new Floor[floorsNumber];

        for (int i = 0; i < floorsNumber; i++)
        {
            GameObject floorObj = Object.Instantiate(floorPrefab, blockRoot);
            floorObj.transform.position += new Vector3(0, i * FloorHeight, 0);
            floorObj.name = "Floor_" + i;

            Floors[i] = floorObj.GetComponent<Floor>();
        }
    }

    void SpawnElevator()
    {
        var startpoint = Floors[elevatorStartFloor].elevatorStartpoint;
        var elevatorObj = Object.Instantiate(elevatorPrefab, startpoint.position, Quaternion.identity, startpoint);
        Elevator = elevatorObj.GetComponent<Elevator>();
        Elevator.blockOwner = this;
    }

    // call it after elevator spawning
    void InitializeFloors()
    {
        for (int i = 0; i < floorsNumber; i++)
        {
            var floor = Floors[i];

            floor.SetNumber(i);
            floor.elevator = Elevator;
            floor.floorCallButton.floorNumber = i;
            floor.floorCallButton.elevator = Elevator;
        }
    }

}
