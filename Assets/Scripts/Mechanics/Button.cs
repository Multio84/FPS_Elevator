using System.Collections;
using UnityEngine;


public abstract class Button : MonoBehaviour, IInteractable
{
    [SerializeField] GameObject button;
    [SerializeField] Light lamp;
    [SerializeField] Material offMat;
    [SerializeField] Material onMat;
    // seconds, before button turns off, if it can't be turned on
    const float AutoTurnOffTime = 0.65f;
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
        Press(true);

        if (elevator.isMoving || IsElevatorOnTheFloor())
        {
            // button turns off soon automatically
            StartCoroutine(UnpressWithDelay());
            return;
        }

        isPressed = true;

        ButtonManager.pressedButton = this;
        StartElevator();
    }

    public void TurnOff()
    {
        Press(false);
        isPressed = false;
    }

    IEnumerator UnpressWithDelay()
    {
        yield return new WaitForSeconds(AutoTurnOffTime);
        Press(false);
    }

    void Press(bool isPressed)
    {
        lamp.enabled = isPressed;

        if (isPressed)
        {   
            meshRenderer.material = onMat;
            button.transform.localPosition = onPos;
        }
        else
        {
            meshRenderer.material = offMat;
            button.transform.localPosition = offPos;
        }
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
