using UnityEngine;

public class CollectXItemsSpeedBonusTracker : MonoBehaviour
{
    private int itemsRequired;
    private float bonusPercent;

    private int currentCount = 0;
    private int bonusLevel = 0;

    private PlayerMovement player;
    private const string SourcePrefix = "CollectXItemsSpeedBonus_";

    public void Setup(int requiredItems, float percentPerLevel, PlayerMovement movement)
    {
        itemsRequired = requiredItems;
        bonusPercent = percentPerLevel;
        player = movement;
    }

    public void OnCollectablePicked()
    {
        currentCount++;

        if (currentCount >= itemsRequired)
        {
            currentCount = 0;
            bonusLevel++;

            string sourceId = SourcePrefix + bonusLevel;

            player.AddSpeedModifier(sourceId, bonusPercent);
            Debug.Log($"📦 Collecté {itemsRequired} objets ! Bonus +{bonusPercent * bonusLevel}% vitesse max (total : {bonusLevel} bonus)");
        }
    }
}