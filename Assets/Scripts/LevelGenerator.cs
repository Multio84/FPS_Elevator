using UnityEngine;


[DefaultExecutionOrder(-20)]
public class LevelGenerator : MonoBehaviour
{
    public static LevelGenerator Instance;

    [Header("Generation Objects")]
    [SerializeField] Transform buildingRoot;
    [SerializeField] GameObject floorPrefab;
    [SerializeField] GameObject floorCombinedPrefab;
    [SerializeField] GameObject basementPrefab;
    [SerializeField] GameObject stairsPrefab;
    [SerializeField] GameObject playerPrefab;
    [SerializeField] GameObject elevatorPrefab;

    [Header("Generation Settings")]
    public const float BlockWidth = 16f;
    public const int MinBlocks = 1;
    public const int MaxBlocks = 10;
    public int blocksNumber = 1;
    public const int MinFloor = 0;
    public const int MaxFloor = 99;
    //[Range(MinFloor + 2, MaxFloor + 1)] public int floorsNumber = 2;
    public int floorsNumber = 2;
    //[Tooltip("The Floor you will start from.\nFloors are numbered starting from zero")]
    public int playerStartFloor = 0;
    //[Tooltip("The Floor elevator will be spawned.\nFloors are numbered starting from zero")]
    public int elevatorStartFloor = 0;

    [HideInInspector] Block[] building;

    //void OnValidate()
    //{
    //    playerStartFloor = Mathf.Clamp(playerStartFloor, MinFloor, floorsNumber - 1);
    //    elevatorStartFloor = Mathf.Clamp(elevatorStartFloor, MinFloor, floorsNumber - 1);
    //}


    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void GenerateLevel()
    {
        if (building != null) 
            DemolishBuilding();
        
        building = new Block[blocksNumber];

        for (int i = 0; i < blocksNumber; i++)
        {
            Vector3 blockPos = buildingRoot.position + new Vector3(i * BlockWidth, 0, 0);
            GameObject blockRoot = new GameObject($"Block_{i}");
            blockRoot.transform.position = blockPos;
            blockRoot.transform.SetParent(buildingRoot);

            Block block = new Block(blockRoot, basementPrefab, floorPrefab, stairsPrefab, elevatorPrefab, floorsNumber, elevatorStartFloor);
            building[i] = block;
        }
    }

    void DemolishBuilding()
    {
        foreach (Block block in building)
        {
            Destroy(block.blockRoot);
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
        player.transform.position = building[0].Floors[playerStartFloor].playerStartpoint.position;
    }

}
