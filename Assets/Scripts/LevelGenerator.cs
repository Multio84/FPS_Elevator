using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField] Transform buildingRoot;
    [SerializeField] GameObject floorPrefab;
    [SerializeField] GameObject playerPrefab;
    [SerializeField] GameObject elevatorPrefab;
    [SerializeField] int floorsNumber;
    [SerializeField] int startFloor;
    static float floorHeight = 6f;

    Transform playerStartpoint;
    Transform elevatorStartpoint;
    Floor[] building;


    private void Awake()
    {
        building = new Floor[floorsNumber];
        GenerateBuilding();
        SpawnPlayer();
        SpawnElevator();
    }

    void GenerateBuilding()
    {
        for (int i = 0; i < floorsNumber; i++)
        {
            GameObject floorObj = Instantiate(floorPrefab, buildingRoot);
            floorObj.transform.position += new Vector3(0, i * floorHeight, 0);
            floorObj.name = "Floor_" + (i + 1);
            Floor floor = floorObj.GetComponent<Floor>();
            building[i] = floor;
            floor.number = i + 1;

            if (i == startFloor - 1) playerStartpoint = floor.playerStartpoint;
            if (i == 0) elevatorStartpoint = floor.elevatorStartpoint;
        }
    }

    void SpawnPlayer()
    {
        Instantiate(playerPrefab, playerStartpoint.position, Quaternion.identity);
    }

    void SpawnElevator()
    {
        Instantiate(elevatorPrefab, elevatorStartpoint.position, Quaternion.identity);
    }
}
