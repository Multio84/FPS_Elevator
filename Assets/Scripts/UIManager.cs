using System;
using UnityEngine;


[DefaultExecutionOrder(-20)]
public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [SerializeField] GameObject menu;
    bool isMenuActive = true;

    public Action OnMenuActive;
    public Action OnMenuInactive;


    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SetMenuActive(!isMenuActive);
        }
    }

    public void SetMenuActive(bool isActive)
    {
        if (menu == null)
        {
            Debug.Log(isActive ? "Trying to show menu: Menu is null" : "Trying to hide menu: Menu is null");
            return;
        }

        menu.SetActive(isActive);

        isMenuActive = isActive;
        if (isActive)
        {
            OnMenuActive.Invoke();
        }
        else
        {
            OnMenuInactive.Invoke();
        }
    }
}
