using UnityEngine;


public class Block : MonoBehaviour
{
    public Floor[] Floors { get; private set; }
    public Elevator Elevator { get; private set; }

    public GameObject blockRoot;
    GameObject floorPrefab;
    GameObject stairsPrefab;
    GameObject elevatorPrefab;
    public const float FloorHeight = 6f;
    int floorsNumber;
    int elevatorStartFloor;


    public void Init(
        GameObject floorPrefab,
        GameObject stairsPrefab,
        GameObject elevatorPrefab,
        int floorsNumber,
        int elevatorStartFloor
        )
    {
        this.floorPrefab = floorPrefab;
        this.stairsPrefab = stairsPrefab;
        this.elevatorPrefab = elevatorPrefab;
        this.floorsNumber = floorsNumber;
        this.elevatorStartFloor = elevatorStartFloor;
    }

    public void Create()
    {
        ConstructBlock();
        SpawnElevator();
        InitializeFloors();
    }

    //TODO: разобраться с созданием лестниц, удалением объектов
    //(пометить все, что для удаления как комбайн и добавлять в список все, что комбайн, даже без компонентов)


    void ConstructBlock()
    {
        Floors = new Floor[floorsNumber];
        GameObject floorInstance = Instantiate(floorPrefab, transform);

        for (int i = 0; i < floorsNumber; i++)
        {
            Vector3 floorPos = transform.position + new Vector3(0, i * FloorHeight, 0);
            var floorObj = Instantiate(floorInstance, floorPos, transform.rotation);
            floorObj.transform.SetParent(blockRoot.transform);
            floorObj.name = "Floor_" + i;

            if (i < floorsNumber - 1)
            {
                Instantiate(stairsPrefab, floorObj.transform);
            }

            Floors[i] = floorObj.GetComponent<Floor>();
        }

        Destroy(floorInstance);
        floorInstance = null;

        ObjectCombiner.Instance.CombineObjectsByTag(blockRoot, "Block");
    }

    void SpawnElevator()
    {
        var startpoint = Floors[elevatorStartFloor].elevatorStartpoint;
        var elevatorObj = Instantiate(elevatorPrefab, startpoint.position, Quaternion.identity, startpoint);
        Elevator = elevatorObj.GetComponent<Elevator>();
        Elevator.block = this;
    }

    // call it after elevator spawn
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
