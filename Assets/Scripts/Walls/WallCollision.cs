using UnityEngine;

public class DeathWall : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerMovement player = other.GetComponent<PlayerMovement>();
            if (player != null)
            {
                GameSession.Instance.TriggerRunEnd();

                // Tu peux ici désactiver les contrôles et afficher un écran de fin
                Debug.Log("Niveau terminé !");
            }
        }
    }
}
