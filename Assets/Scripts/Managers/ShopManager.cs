using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    [Header("Catalogues")]
    public List<ConsumableData> consumablePool;
    public List<BoostData> boostPool;
    public List<StatUpgradeData> statUpgradePool;

    [Header("UI")]
    public ShopUI ui;

    private List<ConsumableData> currentConsumables;
    private List<BoostData> currentBoosts;
    private StatUpgradeData currentStat;

    public bool isAfterBoss = false;

    void Start()
    {
        GenerateShop();
    }

    public void GenerateShop()
    {
        // Sécurité : vérifie que les listes ne sont pas vides
        if (consumablePool.Count < 2 || boostPool.Count < 2)
        {
            Debug.LogError("🛑 Le pool de consommables ou de boosts est vide !");
            return;
        }

        // 1. Consommables
        currentConsumables = new List<ConsumableData>();
        while (currentConsumables.Count < 2)
        {
            var random = consumablePool[Random.Range(0, consumablePool.Count)];
            if (!currentConsumables.Contains(random))
                currentConsumables.Add(random);
        }

        // 2. Boosts
        currentBoosts = new List<BoostData>();
        while (currentBoosts.Count < 2)
        {
            var random = boostPool[Random.Range(0, boostPool.Count)];
            if (!currentBoosts.Contains(random))
                currentBoosts.Add(random);
        }

        // 3. StatUpgrade (optionnel)
        if (GameSession.Instance != null && GameSession.Instance.currentStatUpThisLevel != null)
        {
            currentStat = GameSession.Instance.currentStatUpThisLevel;
        }

        ui.DisplayShop(currentBoosts, currentConsumables, currentStat);
    }


    public void RerollBoosts()
    {
        if (GameSession.Instance.playerMoney < 20) return; // reroll coûte 20 par ex.
        GameSession.Instance.playerMoney -= 20;
        GenerateShop(); // regenère seulement les boosts ici dans la version propre
    }
    
    public void BuyCurrentStatUpgrade()
    {
        StatUpgradeData stat = GameSession.Instance.currentStatUpThisLevel;

        if (stat == null) return;
        if (GameSession.Instance.playerMoney < stat.price) return;

        GameSession.Instance.playerMoney -= stat.price;

        GameSession.Instance.unlockedStatUpgrades.Add(stat);
        GameSession.Instance.availableStatUpgrades.Remove(stat);

        GameSession.Instance.ApplyStatUpgrade(stat);
        GameSession.Instance.currentStatUpThisLevel = null; // On ne l'affiche plus
    }

}