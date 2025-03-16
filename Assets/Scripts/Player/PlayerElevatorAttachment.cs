using UnityEngine;


public class PlayerElevatorAttachment : MonoBehaviour
{   
    public Transform player;


    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Elevator"))
        {
            Transform elevator = other.transform.parent;
            player.transform.SetParent(elevator);
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