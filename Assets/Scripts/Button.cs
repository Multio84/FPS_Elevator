using UnityEngine;


public abstract class Button : MonoBehaviour, IInteractable
{
    [SerializeField] GameObject button;
    [SerializeField] Light lamp;
    [SerializeField] Material offMat;
    [SerializeField] Material onMat;
    MeshRenderer meshRenderer;
    Vector3 offPos;
    Vector3 onPos;
    bool isPressed;

    [HideInInspector] public int floorNumber;
    [HideInInspector] public Elevator elevator;


    private void Awake()
    {
        meshRenderer = button.GetComponent<MeshRenderer>();
        offPos = button.transform.localPosition;
        onPos = offPos + new Vector3(0, 0, 0.03f);
        TurnOff();
    }

    public virtual void Interact()
    {
        isPressed = !isPressed;
        if (isPressed)
            TurnOn();
        else
            TurnOff();
    }

    void TurnOn()
    {
        lamp.enabled = true;
        meshRenderer.material = onMat;
        button.transform.localPosition = onPos;
    }

    void TurnOff()
    {
        lamp.enabled = false;
        meshRenderer.material = offMat;
        button.transform.localPosition = offPos;
    }
}
