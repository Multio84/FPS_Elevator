using UnityEngine;
using UnityEngine.InputSystem;


[DefaultExecutionOrder(-10)]
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    [SerializeField] SettingsManager settingsManager;
    public static GameObject player;
    PlayerInput playerInput;


    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        StartGame();
    }

    void OnDisable()
    {
        UIController.Instance.OnMenuActive -= OnMenuActive;
        UIController.Instance.OnMenuInactive -= OnMenuInactive;
    }

    void StartGame()
    {
        settingsManager.Init();

        CreateGame();

        UIController.Instance.OnMenuActive += OnMenuActive;
        UIController.Instance.OnMenuInactive += OnMenuInactive;
        UIController.Instance.SetMenuActive(false);
    }

    public void CreateGame()
    {
        LevelGenerator.Instance.GenerateLevel();

        if (player == null)
            CreatePlayer();
        else
        {
            DetachPlayer();
            UIController.Instance.SetMenuActive(false);
        }    

        LevelGenerator.Instance.PlacePlayer(player);
    }

    void CreatePlayer()
    {
        player = LevelGenerator.Instance.SpawnPlayer();
        if (player is null)
            Debug.LogError("Player wasn't created.");

        player.GetComponent<PlayerInitalizer>().Init();
        playerInput = player.GetComponent<PlayerInput>();
    }

    void DetachPlayer()
    {
        player.transform.SetParent(null);
    }

    void OnMenuActive()
    {
        PauseGame(true);
    }

    void OnMenuInactive()
    {
        PauseGame(false);
    }

    void PauseGame(bool pause)
    {
        Time.timeScale = pause ? 0f : 1f;

        if (pause)
        { 
            playerInput.DeactivateInput();
            CursorController.Instance.SetMenuCursor();
        }
        else
        {
            playerInput.ActivateInput();
            CursorController.Instance.SetGameCursor();
        }
    }

}
