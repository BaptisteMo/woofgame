using UnityEngine;

[CreateAssetMenu(menuName = "Boosts/Global Boost Multiplier")]
public class bonusValueUp : ScriptableObject, IEffect
{
    [Range(0.1f, 3f)]
    public float multiplierBonus = 0.2f; // +20%

    public void Apply(PlayerMovement player)
    {
        GameSession.Instance.boostPowerMultiplier += multiplierBonus;
    }
}