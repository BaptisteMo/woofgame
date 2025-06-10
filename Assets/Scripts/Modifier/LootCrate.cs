using UnityEngine;

public class LootCrate : MonoBehaviour
{
    [Header("Loot Settings")]
    [Range(0f, 100f)] public float baseDropChance = 50f;
    public int coinReward = 5;

    [Header("Attraction Settings")]
    public float attractionRadius = 20f;
    public float magnetSpeedMultiplier = 2f;

    private Transform playerTransform;
    private bool isMagnetized = false;
    private float playerSpeed; // valeur par défaut, sera mise à jour dynamiquement

    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

        // Empêche les collisions avec les murs si la caisse est attirée
        GetComponent<Collider>().isTrigger = true;
    }

    void Update()
    {
        if (playerTransform == null) return;

        float distance = Vector3.Distance(transform.position, playerTransform.position);
        float dx = Mathf.Abs(transform.position.x - playerTransform.position.x);

        // 1️⃣ Si bonus actif et à portée, on enclenche le mouvement
        if (!isMagnetized && GameSession.Instance.adjacentLootCollectorEnabled && dx > 0.1f && distance < attractionRadius)
        {
            isMagnetized = true;
            playerSpeed = GameSession.Instance.lastPlayerSpeed; // supposons qu’on a sauvegardé la vitesse
        }

        // 2️⃣ Si attirée, on se dirige vers le joueur
        if (isMagnetized)
        {
            float speed = playerSpeed * magnetSpeedMultiplier;
            transform.position = Vector3.MoveTowards(transform.position, playerTransform.position, speed * Time.deltaTime);

            // 3️⃣ Arrivée au joueur
            if (Vector3.Distance(transform.position, playerTransform.position) < 0.5f)
            {
                GrantLoot();
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Cas normal, le joueur touche directement la caisse
        if (other.CompareTag("Player") && !isMagnetized)
        {
            GrantLoot();
            Destroy(gameObject);
        }
    }

    private void GrantLoot()
    {
        float bonus = GameSession.Instance.coinDropBonusPercent;
        float finalChance = baseDropChance + bonus;
        float roll = Random.Range(0f, 100f);

        if (roll <= finalChance)
        {
            GameSession.Instance.playerMoney += coinReward;
            Debug.Log($"💰 Caisse lootée ! ({finalChance}%)");
        }
        else
        {
            Debug.Log($"❌ Rien obtenu ({finalChance}%)");
        }
    }
}
