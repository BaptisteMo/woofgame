using UnityEngine;

[CreateAssetMenu(menuName = "Boosts/MaxSpeedIfNoCollision")]
public class MaxSpeedIncreaseIfNoCollision : ScriptableObject, IEffect
{
    public float secondsRequired = 5f;
    public float speedBonusPercent = 20f;

    public void Apply(PlayerMovement player)
    {
        var tracker = player.gameObject.AddComponent<MaxSpeedNoCollisionTracker>();
        tracker.Setup(secondsRequired, speedBonusPercent);
        Debug.Log($"⏱️ Boost activé : +{speedBonusPercent}% de vitesse max après {secondsRequired} sec sans collision");
    }
}