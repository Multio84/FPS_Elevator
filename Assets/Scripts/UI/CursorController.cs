using System.Collections;
using UnityEngine;


[DefaultExecutionOrder(-30)]
public class CursorController : MonoBehaviour
{
    public static CursorController Instance { get; private set; }


    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void SetGameCursor()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void SetMenuCursor()
    {
        StartCoroutine(EnableCursor());
    }

    private IEnumerator EnableCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;   // just to center cursor on screen
        yield return null;
        Cursor.lockState = CursorLockMode.None;

        Cursor.visible = true;
    }
}
