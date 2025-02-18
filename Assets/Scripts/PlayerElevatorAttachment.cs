using UnityEngine;


public class PlayerElevatorAttachment : MonoBehaviour
{   
    public Transform player;
    

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Elevator"))
        {
            player.transform.SetParent(other.transform);
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