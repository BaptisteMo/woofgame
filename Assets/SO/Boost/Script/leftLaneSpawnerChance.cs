using UnityEngine;

[CreateAssetMenu(menuName = "Boosts/LeftLaneLootChanceBonus")]
public class LeftLaneLootChanceBonus : ScriptableObject, IEffect
{
    [Header("Bonus d'apparition (en %)")]
    public float normalCrateBonusPercent = 10f;
    public float goldenCrateBonusPercent = 5f;

    public void Apply(PlayerMovement player)
    {
        GameSession.Instance.leftLaneNormalCrateBonus += normalCrateBonusPercent;
        GameSession.Instance.leftLaneGoldenCrateBonus += goldenCrateBonusPercent;

        Debug.Log($"🎁 Boost : +{normalCrateBonusPercent}% de caisses normales et +{goldenCrateBonusPercent}% de caisses dorées sur la ligne de gauche");
    }
}