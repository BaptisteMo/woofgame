using UnityEngine;

using UnityEngine;

[CreateAssetMenu(menuName = "Boosts/Coin Loot Chance Bonus")]
public class CoinLootChanceBoost : ScriptableObject, IEffect
{
    [Range(0f, 100f)]
    public float bonusPercent = 15f;

    public void Apply(PlayerMovement player)
    {
        GameSession.Instance.coinDropBonusPercent += bonusPercent;
    }
}


