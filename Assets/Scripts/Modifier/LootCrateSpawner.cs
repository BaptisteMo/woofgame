using UnityEngine;

public class LootCrateGenerator : MonoBehaviour
{
    public enum Lane { Left = -1, Center = 0, Right = 1 }
    public Lane targetLane;

    [Header("Chances de spawn (%)")]
    [Range(0, 100)] public float chanceSpawnNormal = 60f;
    [Range(0, 100)] public float chanceSpawnGolden = 20f;

    public GameObject normalCratePrefab;
    public GameObject goldenCratePrefab;

    private void Start()
    {
        TrySpawnCrate();
    }

    public void TrySpawnCrate()
    {
        float bonusNormal = 0f;
        float bonusGolden = GameSession.Instance.goldenCrateSpawnerChanceBonus;

        if (targetLane == Lane.Left)
        {
            bonusNormal += GameSession.Instance.leftLaneNormalCrateBonus;
            bonusGolden += GameSession.Instance.leftLaneGoldenCrateBonus;
        }

        float roll = Random.Range(0f, 100f);

        if (roll <= chanceSpawnGolden + bonusGolden)
        {
            Instantiate(goldenCratePrefab, transform.position, Quaternion.identity);
        }
        else if (roll <= chanceSpawnNormal + bonusNormal)
        {
            Instantiate(normalCratePrefab, transform.position, Quaternion.identity);
        }
        else
        {
            // Rien ne spawn
            Debug.Log("❌ Aucun loot spawné");
        }
    }
}