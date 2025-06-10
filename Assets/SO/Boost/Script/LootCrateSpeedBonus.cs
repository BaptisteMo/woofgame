using UnityEngine;

[CreateAssetMenu(menuName = "Boosts/LootSpeedBoost")]
public class LootSpeedBoost : ScriptableObject, IEffect
{
    [Range(0f, 100f)]
    public float percentBonus = 10f;

    public void Apply(PlayerMovement player)
    {
        GameSession.Instance.lootGrantsSpeed = true;
        GameSession.Instance.lootSpeedBonusPercent = percentBonus;
        Debug.Log($"🚀 Boost : les caisses donnent +{percentBonus}% de vitesse");
    }
}