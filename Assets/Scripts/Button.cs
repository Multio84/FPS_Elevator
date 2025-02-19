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
    [HideInInspector] public Elevator elevator;
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
        if (!isPressed) TurnOn();
    }

    void TurnOn()
    {
        if (elevator.isMoving || IsElevatorOnTheFloor()) return;

        lamp.enabled = true;
        meshRenderer.material = onMat;
        button.transform.localPosition = onPos;
        isPressed = true;

        ButtonManager.pressedButton = this;
        StartElevator();
    }

    public void TurnOff()
    {
        lamp.enabled = false;
        meshRenderer.material = offMat;
        button.transform.localPosition = offPos;
        isPressed = false;
    }

    void StartElevator()
    {
        elevator.MoveTo(floorNumber);
    }

    bool IsElevatorOnTheFloor()
    {
        return elevator.currentFloor == floorNumber;
    }
}
