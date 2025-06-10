using UnityEngine;

[CreateAssetMenu(menuName = "Boosts/goldenCrateSpawnChance")]
public class goldenCrateSpawnChance : ScriptableObject, IEffect
{
    [Header("Bonus d'apparition (en %)")]
    public float goldenCrateBonusPercent = 5f;

    public void Apply(PlayerMovement player)
    {
        GameSession.Instance.goldenCrateSpawnerChanceBonus += goldenCrateBonusPercent;

        Debug.Log($"🎁 Boost : +{goldenCrateBonusPercent}% de caisses dorées");
    }
}