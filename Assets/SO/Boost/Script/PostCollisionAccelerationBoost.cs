using UnityEngine;

[CreateAssetMenu(menuName = "Boosts/PostCollisionAccelerationBoost")]
public class PostCollisionAccelerationBoost : ScriptableObject, IEffect
{
    public float accelerationBonus = 5f;
    public float duration = 3f;

    public void Apply(PlayerMovement player)
    {
        var tracker = player.gameObject.AddComponent<PostCollisionAccelerationTracker>();
        tracker.Setup(accelerationBonus, duration, player);
        Debug.Log($"💥 Bonus activé : +{accelerationBonus} accélération pendant {duration} sec après collision");
    }
}