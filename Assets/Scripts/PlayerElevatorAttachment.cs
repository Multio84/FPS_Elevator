using UnityEngine;


public class PlayerElevatorAttachment : MonoBehaviour
{   
    public Transform player;
    public Transform elevatorRoot;

    private void Awake()
    {
        elevatorRoot = GameObject.Find("ElevatorRoot").transform;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Elevator"))
        {
            player.transform.SetParent(elevatorRoot);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Elevator"))
        {
            player.transform.SetParent(null);
        }
    }
}