using StarterAssets;
using UnityEngine;


[DefaultExecutionOrder(-10)]
public class GameManager : MonoBehaviour
{
    public static GameObject player;
    [SerializeField] GameObject uiWindowPrefab;
    [SerializeField] GameObject uiCanvas;
    bool isCursorActive;
    bool IsCursorActive
    {
        get => isCursorActive;
        set
        {
            isCursorActive = value;
            StarterAssetsInputs inputs = null;
            if (player != null)
                inputs = player.GetComponent<StarterAssetsInputs>();

            if (inputs != null)
            {
                inputs.cursorLocked = !value;
                inputs.cursorInputForLook = !value;
            }
        }
    }


    private void Awake()
    {
        Initialize();
        OpenWindow();
    }

    private void Initialize()
    {
        var generator = LevelGenerator.Instance;
        generator.GenerateLevel();
        player = generator.SpawnPlayer();
        IsCursorActive = true;
    }

    public void OpenWindow()
    {
        if (uiWindowPrefab != null && uiCanvas != null)
        {
            Instantiate(uiWindowPrefab, uiCanvas.transform, false);
        }
    }
}
