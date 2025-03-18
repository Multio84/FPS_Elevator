using UnityEngine;


public class DeathTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        var player = other.gameObject.GetComponent<PlayerDeath>();
        if (player != null && player.CompareTag("Player"))
        {
            player.Death();
        }
    }
}
