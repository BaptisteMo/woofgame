using UnityEngine;

[CreateAssetMenu(menuName = "Boosts/FasterAcceleration")]
public class FasterAccelerationEffect : ScriptableObject, IEffect
{
    public float accelerationFactor = 0.75f; // diminue la durée d'accélération de 25 %

    public void Apply(PlayerMovement player)
    {
        GameSession.Instance.accelerationDuration *= accelerationFactor;
    }
}

