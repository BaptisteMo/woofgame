using UnityEngine;

[CreateAssetMenu(menuName = "Boosts/StableLaneSpeedBoost")]
public class StableLaneSpeedBoost : ScriptableObject, IEffect
{
    public float timeOnSameLaneRequired = 4f;
    [Range(1, 100)]
    public float speedBonus = 1f;

    public void Apply(PlayerMovement player)
    {
        var tracker = player.GetComponent<StableLaneSpeedTracker>();
        if (tracker == null)
        {
            tracker = player.gameObject.AddComponent<StableLaneSpeedTracker>();
        }

        tracker.Setup(timeOnSameLaneRequired, speedBonus, player);
        Debug.Log($"🛣️ Bonus appliqué : +{speedBonus} à la vitesse max toutes les {timeOnSameLaneRequired}s sur la même lane");
    }
}