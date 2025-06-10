using UnityEngine;


[CreateAssetMenu(menuName = "Boosts/Coin Loot Chance Bonus")]
public class CoinLootChanceBoost : ScriptableObject, IEffect
{
    [Range(0f, 100f)]
    public float bonusPercent = 25f;

    public void Apply(PlayerMovement player)
    {
        GameSession.Instance.coinDropBonusPercent += bonusPercent;
    }
}


