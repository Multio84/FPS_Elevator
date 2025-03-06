using UnityEngine;


public interface IInteractable
{
    void Interact();
}


public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] float interactDistance = 3f;
    [SerializeField] GameObject tooltip;
    bool isMenuMode = true;

    void Start()
    {
        UIManager.Instance.OnMenuActive += OnMenuActive;
        UIManager.Instance.OnMenuInactive += OnMenuInactive;
    }

    void OnDisable()
    {
        UIManager.Instance.OnMenuActive -= OnMenuActive;
        UIManager.Instance.OnMenuInactive -= OnMenuInactive;
    }

    void Update()
    {
        if (isMenuMode) return;

        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactDistance))
        {
            IInteractable interactable = hit.collider.GetComponentInParent<IInteractable>();
            if (interactable != null)
            {
                tooltip.SetActive(true);

                if (Input.GetKeyDown(KeyCode.E))
                {
                    interactable.Interact();
                }
            }
            else
            {
                tooltip.SetActive(false);
            }
        }
    }

    void OnMenuActive()
    {
        isMenuMode = true;
        tooltip.SetActive(false);
    }

    void OnMenuInactive()
    {
        isMenuMode = false;
    }

}
