using UnityEngine;


public class PlayerElevatorAttachment : MonoBehaviour
{   
    public Transform player;
    public Transform elevatorRoot;


    void Awake()
    {
        Init();
    }

    public void Init()
    {
        elevatorRoot = GameObject.Find("ElevatorRoot").transform;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Elevator"))
        {
            player.transform.SetParent(elevatorRoot);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Elevator"))
        {
            player.transform.SetParent(null);
        }
    }
}