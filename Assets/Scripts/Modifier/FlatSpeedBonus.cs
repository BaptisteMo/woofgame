using System;
using UnityEngine;

public class FlatSpeedBonus : MonoBehaviour
{
    public float bonusAmount = 5f; // How much to boost the speed

    private void OnTriggerEnter(Collider other)
    {
        // Check if the object that enters is the player
        if (other.CompareTag("Player"))
        {
            // Try to get the PlayerMovement script and apply speed boost
            PlayerMovement player = other.GetComponent<PlayerMovement>();
            if (player != null)
            {
                var collectItem = other.GetComponent<CollectXItemsSpeedBonusTracker>();
                if (collectItem != null)
                {
                    collectItem.OnCollectablePicked();
                }
                var combo = other.GetComponent<CollectorComboSpeedTracker>();
                if (combo != null)
                {
                    combo.OnCollectablePicked();
                }

                float effectiveBonus = BoostHelper.ApplyMultiplier(bonusAmount);
                player.BoostSpeed(effectiveBonus);
                Debug.Log($"🚀 Boost pris : {effectiveBonus} vitesse");
            }

            // Destroy the bonus object after pickup
            Destroy(gameObject);
        }
    }
}