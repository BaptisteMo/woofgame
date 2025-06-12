using UnityEngine;

[CreateAssetMenu(menuName = "Boosts/DoubleAccelerationIfFast")]
public class DoubleAccelerationIfFast : ScriptableObject, IEffect
{
    public float speedThreshold = 15f;

    public void Apply(PlayerMovement player)
    {
        GameSession.Instance.doubleAccelIfFastEnabled = true;
        GameSession.Instance.speedThresholdForDoubleAccel = speedThreshold;

        Debug.Log($"🚦 Boost activé : Si vitesse > {speedThreshold}, le multiplicateur d’accélération est doublé à la fin");
    }
}