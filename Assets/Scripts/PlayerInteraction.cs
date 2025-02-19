using UnityEngine;


public interface IInteractable
{
    void Interact();
}

public class PlayerInteraction : MonoBehaviour
{
    public float interactDistance = 3f;


    void Update()
    {
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
}
