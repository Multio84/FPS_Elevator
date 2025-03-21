using UnityEngine;


[DefaultExecutionOrder(-20)]
public class LevelGenerator : MonoBehaviour
{
    public static LevelGenerator Instance { get; private set; }

    [Header("Generation Objects")]
    [SerializeField] Transform buildingRoot;
    [SerializeField] GameObject blockPrefab;
    [SerializeField] GameObject floorPrefab;
    [SerializeField] GameObject stairsPrefab;
    [SerializeField] GameObject playerPrefab;
    [SerializeField] GameObject elevatorPrefab;

    [Header("Generation Settings")]
    public const float BlockWidth = 16f;
    public const int MinBlocks = 1;
    public const int MaxBlocks = 10;
    public const int MinFloor = 0;
    public const int MaxFloor = 99;
    public static int blocksNumber = 2;
    public static int floorsNumber = 2;
    public static int playerStartFloor = 0;
    public static int elevatorStartFloor = 1;
    //[Range(MinFloor + 2, MaxFloor + 1)] public int floorsNumber = 2;
    //[Tooltip("The Floor you will start from.\nFloors are numbered starting from zero")]
    //[Tooltip("The Floor elevator will be spawned.\nFloors are numbered starting from zero")]

    GameObject[] building;


    //void OnValidate()
    //{
    //    playerStartFloor = Mathf.Clamp(playerStartFloor, MinFloor, floorsNumber - 1);
    //    elevatorStartFloor = Mathf.Clamp(elevatorStartFloor, MinFloor, floorsNumber - 1);
    //}

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void GenerateLevel()
    {
        if (building != null) 
            DemolishBuilding();
        
        ConstructBuilding();
    }

    void ConstructBuilding()
    {
        building = new GameObject[blocksNumber];

        for (int i = 0; i < blocksNumber; i++)
        {
            Vector3 blockPos = buildingRoot.position + new Vector3(i * BlockWidth, 0, 0);
            var blockObj = Instantiate(blockPrefab, blockPos, buildingRoot.rotation);
            blockObj.transform.SetParent(buildingRoot);
            blockObj.name = "Block_" + i;
            building[i] = blockObj;

            Block block = blockObj.GetComponent<Block>();
            block.Init(floorPrefab, stairsPrefab, elevatorPrefab, floorsNumber, elevatorStartFloor);
            block.Create();
        }
    }

    void DemolishBuilding()
    {
        foreach (var block in building)
        {
            Destroy(block);
        }

        building = null;
    }

    public GameObject SpawnPlayer()
    {
        var player = Instantiate(playerPrefab);
        PlacePlayer(player);
        return player;
    }

    public void PlacePlayer(GameObject player)
    {
        Block block = building[0].GetComponent<Block>();
        player.transform.position = block.Floors[playerStartFloor].playerStartpoint.position;
        player.transform.rotation = Quaternion.identity;
    }

}
