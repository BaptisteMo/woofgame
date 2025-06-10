using UnityEngine;

[CreateAssetMenu(menuName = "Boosts/IncreaseMaxSpeed")]
public class IncreaseMaxSpeedEffect : ScriptableObject, IEffect
{
    public float amount = 15f;

    public void Apply(PlayerMovement player)
    {
        GameSession.Instance.maxSpeed += amount;
    }
}