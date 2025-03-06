using UnityEngine;


public class Block
{
    public Floor[] Floors { get; private set; }
    public Elevator Elevator { get; private set; }

    public GameObject blockRoot;
    GameObject basementPrefab;
    GameObject floorPrefab;
    GameObject floorCombined;
    GameObject stairsPrefab;
    GameObject elevatorPrefab;
    public const float FloorHeight = 6f;
    int floorsNumber;
    int elevatorStartFloor;


    public Block(
        GameObject blockRoot, 
        GameObject basementPrefab, 
        GameObject floorPrefab, 
        GameObject stairsPrefab, 
        GameObject elevatorPrefab, 
        int floorsNumber, 
        int elevatorStartFloor
        )
    {
        this.blockRoot = blockRoot;
        this.basementPrefab = basementPrefab;
        this.floorPrefab = floorPrefab;
        this.stairsPrefab = stairsPrefab;
        this.elevatorPrefab = elevatorPrefab;
        this.floorsNumber = floorsNumber;
        this.elevatorStartFloor = elevatorStartFloor;

        ConstructFloor();
        ConstructBlock();
        SpawnElevator();
        InitializeFloors();
    }

    void ConstructBlock()
    {
        Object.Instantiate(basementPrefab, blockRoot.transform);

        Floors = new Floor[floorsNumber];

        for (int i = 0; i < floorsNumber; i++)
        {
            var floorObj = Object.Instantiate(floorCombined, blockRoot.transform);
            floorObj.transform.position += new Vector3(0, i * FloorHeight, 0);
            floorObj.name = "Floor_" + i;
            
            Floors[i] = floorObj.GetComponent<Floor>();

            //if (i < floorsNumber - 1)
            //{
            //    Object.Instantiate(stairsPrefab, floor.rootObj.transform);
            //}
        }

        Object.Destroy(floorCombined);
    }

    void ConstructFloor()
    {
        floorCombined = Object.Instantiate(floorPrefab, blockRoot.transform);
        Floor floor = floorCombined.GetComponent<Floor>();
        Object.Instantiate(stairsPrefab, floor.rootObj.transform);

        ObjectCombiner.CombineMeshes(floor.rootObj);
    }

    void SpawnElevator()
    {
        var startpoint = Floors[elevatorStartFloor].elevatorStartpoint;
        var elevatorObj = Object.Instantiate(elevatorPrefab, startpoint.position, Quaternion.identity, startpoint);
        Elevator = elevatorObj.GetComponent<Elevator>();
        Elevator.block = this;
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
