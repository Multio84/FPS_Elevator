using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LevelGenerator : MonoBehaviour
{
    public static LevelGenerator Instance;
    [SerializeField] Transform buildingRoot;
    [SerializeField] GameObject floorPrefab;
    [SerializeField] GameObject playerPrefab;
    [SerializeField] GameObject elevatorPrefab;
    [Range(2, 100)] public int totalFloors;
    public int playerStartFloor;
    public int elevatorStartFloor;
    public const float FloorHeight = 6f;

    [HideInInspector] public Floor[] building;
    [HideInInspector] public Elevator elevator;


    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(Instance);

        GenerateLevel();
    }

    void GenerateLevel()
    {
        ConstructBuilding();
        SpawnElevator();
        InitializeFloors();
        SpawnPlayer();
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
        var elevatorOgj = Instantiate(elevatorPrefab, startpoint.position, Quaternion.identity);
        elevator = elevatorOgj.GetComponent<Elevator>();
    }

    void SpawnPlayer()
    {
        var playerStartpoint = building[playerStartFloor].playerStartpoint;
        var player = Instantiate(playerPrefab, playerStartpoint.position, Quaternion.identity);
    }


}
