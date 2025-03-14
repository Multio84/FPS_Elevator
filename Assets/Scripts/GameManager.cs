using StarterAssets;
using UnityEngine;
using UnityEngine.InputSystem;


[DefaultExecutionOrder(-10)]
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public static GameObject player;
    bool isCursorActive;
    bool IsCursorActive
    {
        get => isCursorActive;
        set
        {
            isCursorActive = value;
            MyStarterAssetsInputs inputs = null;
            if (player != null)
                inputs = player.GetComponent<MyStarterAssetsInputs>();

            if (inputs != null)
            {
                inputs.cursorLocked = !value;
                inputs.cursorInputForLook = !value;
                Cursor.visible = value;
            }
        }
    }


    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        UIManager.Instance.OnMenuActive += OnMenuActive;
        UIManager.Instance.OnMenuInactive += OnMenuInactive;

        GenerateGame();
        PauseGame(true);
    }

    private void OnDisable()
    {
        UIManager.Instance.OnMenuActive -= OnMenuActive;
        UIManager.Instance.OnMenuInactive -= OnMenuInactive;
    }

    public void GenerateGame()
    {
        LevelGenerator.Instance.GenerateLevel();

        if (player == null)
            player = LevelGenerator.Instance.SpawnPlayer();
        else
            DetachPlayer();

        LevelGenerator.Instance.PlacePlayer(player);
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

    public void PauseGame(bool pause)
    {
        IsCursorActive = pause;
        Time.timeScale = pause ? 0f : 1f;

        var playerInput = player.GetComponent<PlayerInput>();
        if (playerInput != null)
        {
            if (pause)
                playerInput.DeactivateInput();
            else
                playerInput.ActivateInput();
        }
    }

}
