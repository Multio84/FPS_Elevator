using UnityEngine;


[RequireComponent(typeof(MeshRenderer))]
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


    protected virtual void Awake()
    {
        meshRenderer = button.GetComponent<MeshRenderer>();
        if (meshRenderer is null)
            Debug.Log("Null MR");
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
        if (meshRenderer is null)
            Debug.Log("Null MR");
        meshRenderer.material = onMat;
        button.transform.localPosition = onPos;
    }

    void TurnOff()
    {
        lamp.enabled = false;
        if (meshRenderer is null)
            Debug.Log("Null MR");
        meshRenderer.material = offMat;
        button.transform.localPosition = offPos;
    }
}
