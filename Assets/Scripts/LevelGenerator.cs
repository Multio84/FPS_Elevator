using StarterAssets;
using UnityEngine;

[DefaultExecutionOrder(-20)]
public class LevelGenerator : MonoBehaviour
{
    public static LevelGenerator Instance;

    [Header("Generation Objects")]
    [SerializeField] Transform buildingRoot;
    [SerializeField] GameObject floorPrefab;
    [SerializeField] GameObject playerPrefab;
    [SerializeField] GameObject elevatorPrefab;

    [Header("Generation Settings")]
    public const float FloorHeight = 6f;
    public const int MinFloor = 0;
    public const int MaxFloor = 99;
    [Range(MinFloor + 2, MaxFloor + 1)] public int totalFloors;
    [Tooltip("The Floor you will start from.\nFloors are numbered starting from zero")]
    public int playerStartFloor;
    [Tooltip("The Floor elevator will be spawned.\nFloors are numbered starting from zero")]
    public int elevatorStartFloor;

    [HideInInspector] public Floor[] building;
    [HideInInspector] public Elevator elevator;


    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(Instance);
    }

    void OnValidate()
    {
        playerStartFloor = Mathf.Clamp(playerStartFloor, MinFloor, totalFloors - 1);
        elevatorStartFloor = Mathf.Clamp(elevatorStartFloor, MinFloor, totalFloors - 1);
    }

    public void GenerateLevel()
    {
        ConstructBuilding();
        SpawnElevator();
        InitializeFloors();
    }

    void ConstructBuilding()
    {
        building = new Floor[totalFloors];

        for (int i = 0; i < totalFloors; i++)
        {
            GameObject floorObj = Instantiate(floorPrefab, buildingRoot);
            floorObj.transform.position += new Vector3(0, i * FloorHeight, 0);
            floorObj.name = "Floor_" + i;

            building[i] = floorObj.GetComponent<Floor>();
        }
    }

    // call it after elevator spawning
    void InitializeFloors()
    {
        for (int i = 0; i < totalFloors; i++)
        {
            var floor = building[i];

            floor.SetNumber(i);
            floor.elevator = elevator;
            floor.floorCallButton.floorNumber = i;
            floor.floorCallButton.elevator = elevator;
        }
    }

    void SpawnElevator()
    {
        var startpoint = building[elevatorStartFloor].elevatorStartpoint;
        var elevatorObj = Instantiate(elevatorPrefab, startpoint.position, Quaternion.identity);
        elevator = elevatorObj.GetComponent<Elevator>();
    }

    public GameObject SpawnPlayer()
    {
        var playerStartpoint = building[playerStartFloor].playerStartpoint;
        return Instantiate(playerPrefab, playerStartpoint.position, Quaternion.identity);
    }
}
