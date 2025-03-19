using UnityEngine;


public interface IInteractable
{
    void Interact();
}


public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] float interactDistance = 3f;
    [SerializeField] GameObject hud;
    bool isMenuMode;

    void Awake()
    {
        UIController.Instance.OnMenuActive += OnMenuActive;
        UIController.Instance.OnMenuInactive += OnMenuInactive;
    }

    void OnDisable()
    {
        UIController.Instance.OnMenuActive -= OnMenuActive;
        UIController.Instance.OnMenuInactive -= OnMenuInactive;
    }

    void Update()
    {
        if (isMenuMode) return;

        if (Input.GetKeyDown(KeyCode.E))
        {
            Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, interactDistance))
            {
                IInteractable interactable = hit.collider.GetComponentInParent<IInteractable>();
                if (interactable != null)
                {
                    interactable.Interact();
                }
            }
        }
    }

    void OnMenuActive()
    {
        isMenuMode = true;
        hud.SetActive(false);
    }

    void OnMenuInactive()
    {
        isMenuMode = false;
        hud.SetActive(true);
    }

}
