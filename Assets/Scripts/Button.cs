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
    public Elevator elevator;
    [HideInInspector] public int floorNumber;



    protected virtual void Awake()
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
        StartElevator();
    }

    void TurnOff()
    {
        lamp.enabled = false;
        meshRenderer.material = offMat;
        button.transform.localPosition = offPos;
    }

    void StartElevator()
    {
        // if the elevator is not on the floor
        if (elevator.transform != LevelGenerator.Instance.building[floorNumber - 1].transform)
        {
            elevator.MoveTo(floorNumber);
        }
    }
}
