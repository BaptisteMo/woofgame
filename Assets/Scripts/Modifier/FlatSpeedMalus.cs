using UnityEngine;

public class FlatSpeedMalus : MonoBehaviour
{
    public float malusAmount = 5f; // How much to boost the speed

    private void OnTriggerEnter(Collider other)
    {
        // Check if the object that enters is the player
        if (other.CompareTag("Player"))
        {
            // Try to get the PlayerMovement script and apply speed boost
            PlayerMovement player = other.GetComponent<PlayerMovement>();
            if (player != null)
            {
                player.DecreaseSpeed(malusAmount);
            }
            
         Destroy(gameObject);
        }
    }
    
}