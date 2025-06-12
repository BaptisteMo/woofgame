using UnityEngine;

[CreateAssetMenu(menuName = "Boosts/FastTopSlowAccel")]
public class FastTopSlowAccel : ScriptableObject, IEffect
{
    public float speedBonus = 5f;             // +5 de vitesse max
    public float accelerationPenalty = 3f;    // +3s d’accélération (donc plus lent)

    public void Apply(PlayerMovement player)
    {
        GameSession.Instance.maxSpeed += speedBonus;
        GameSession.Instance.accelerationDuration += accelerationPenalty;

        Debug.Log($"🚀 Boost appliqué : +{speedBonus} de vitesse max, mais accélération plus lente (+{accelerationPenalty}s)");
    }
}