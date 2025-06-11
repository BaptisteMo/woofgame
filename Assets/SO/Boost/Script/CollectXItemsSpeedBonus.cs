using UnityEngine;

[CreateAssetMenu(menuName = "Boosts/CollectXItemsSpeedBonus")]
public class CollectXItemsSpeedBonus : ScriptableObject, IEffect
{
    public int itemsRequired = 5;
    [Range(1,100)]
    public float bonusPercent = 10f;

    public void Apply(PlayerMovement player)
    {
        var tracker = player.gameObject.AddComponent<CollectXItemsSpeedBonusTracker>();
        tracker.Setup(itemsRequired, bonusPercent, player);
        Debug.Log($"🎁 Boost combo actif : +{bonusPercent}% pour 5 item collecté");
    }
}