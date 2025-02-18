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
    [Range(2, 99)] public int totalFloors;
    [SerializeField] int startFloor;
    static float floorHeight = 6f;

    Transform playerStartpoint;
    Transform elevatorStartpoint;
    public Floor[] building;
    public Elevator elevator;


    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        GenerateLevel();
    }

    void GenerateLevel()
    {
        GenerateBuilding();
        SpawnPlayer();
        InitializeFloorCallButtons();
        //SpawnElevator();
    }

    void GenerateBuilding()
    {
        building = new Floor[totalFloors];

        for (int i = 0; i < totalFloors; i++)
        {
            GameObject floorObj = Instantiate(floorPrefab, buildingRoot);
            floorObj.transform.position += new Vector3(0, i * floorHeight, 0);
            floorObj.name = "Floor_" + (i + 1);
            Floor floor = floorObj.GetComponent<Floor>();
            building[i] = floor;
            floor.number = i + 1;

            if (i == startFloor - 1) playerStartpoint = floor.playerStartpoint;
            if (i == 0)
            {
                elevatorStartpoint = floor.elevatorStartpoint;
                SpawnElevator();
            }
        }
    }

    void InitializeFloorCallButtons()
    {
        for (int i = 0; i < totalFloors; i++)
        {
            building[i].floorCallButton.floorNumber = i + 1;
            building[i].floorCallButton.elevator = elevator;
        }
    }

    void SpawnPlayer()
    {
        var player = Instantiate(playerPrefab, playerStartpoint.position, Quaternion.identity);
    }

    void SpawnElevator()
    {
        var elevatorOgj = Instantiate(elevatorPrefab, elevatorStartpoint.position, Quaternion.identity);
        elevator = elevatorOgj.GetComponent<Elevator>();
    }
}
