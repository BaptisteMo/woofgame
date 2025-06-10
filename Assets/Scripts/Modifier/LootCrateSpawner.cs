using UnityEngine;

public class LootCrateGenerator : MonoBehaviour
{
    [Header("Configuration")]
    [Tooltip("Position de la lane : -1 = gauche, 0 = centre, 1 = droite")]
    public int laneIndex = 0;

    [Range(0, 100)] public float normalCrateChance = 50f;
    [Range(0, 100)] public float goldenCrateChance = 30f;

    public GameObject normalCratePrefab;
    public GameObject goldenCratePrefab;

    public float laneWidth = 5f;

    void Start()
    {
        float roll = Random.Range(0f, 100f);
        Vector3 position = transform.position;
        position.y = position.y + 0.5f;

        if (roll <= normalCrateChance)
        {
            Instantiate(normalCratePrefab, position, Quaternion.identity);
        }
        else if (roll <= normalCrateChance + goldenCrateChance)
        {
            Instantiate(goldenCratePrefab, position, Quaternion.identity);
        }
        // sinon : rien ne se passe
    }
}