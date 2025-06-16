using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ShopManager : MonoBehaviour
{
    [Header("Catalogues")]
    public List<ConsumableData> consumablePool;
    public List<BoostData> boostPool;
   [SerializeField] private int boostNumber;
   [SerializeField] private int consumableNumber;
    [SerializeField] private int cost;

    [Header("UI")]
    public ShopUI ui; 
    private List<ConsumableData> currentConsumables;
    private List<BoostData> currentBoosts;
  
    private int rerollCount = 0;



    void Start()
    {
        GenerateShop();
    }

    public void GenerateShop()
    {
        // Sécurité : vérifie que les listes ne sont pas vides
        if (consumablePool.Count < 1 || boostPool.Count < 1)
        {
            Debug.LogError("🛑 Le pool de consommables ou de boosts est vide !");
            return;
        }

        // 1. Consommables
        currentConsumables = new List<ConsumableData>();
        while (currentConsumables.Count < consumableNumber)
        {
            var random = consumablePool[Random.Range(0, consumablePool.Count)];
            if (!currentConsumables.Contains(random))
                currentConsumables.Add(random);
        }

        // 2. Boosts
        currentBoosts = new List<BoostData>();
        while (currentBoosts.Count < boostNumber)
        {
            var random = boostPool[Random.Range(0, boostPool.Count)];
            if (!currentBoosts.Contains(random))
                currentBoosts.Add(random);
        }

        ui.DisplayShop(currentBoosts, currentConsumables);
        ui.UpdateRerollPriceUI(GetRerollCost());
        GetRerollCost();

    }
    public void RemoveBoost(BoostData boost)
    {
        if (boostPool.Contains(boost))
        {
            boostPool.Remove(boost);
            Debug.Log($"🧹 Boost retiré du pool : {boost.boostName}");
        }

        if (currentBoosts.Contains(boost))
        {
            currentBoosts.Remove(boost);
            ui.DisplayShop(currentBoosts, currentConsumables); // refresh UI
        }
    }


    public void RerollBoosts()
    {
        cost = GetRerollCost();

        if (GameSession.Instance.playerMoney < cost)
        {
            Debug.Log("💸 Pas assez de monnaie pour un reroll !");
            return;
        }
        if (GameSession.Instance.playerMoney < cost) return;
        
        GameSession.Instance.playerMoney -= cost;
        rerollCount++;

        GameSession.Instance.playerMoney -= cost;

        currentBoosts = new List<BoostData>();
        while (currentBoosts.Count < boostNumber)
        {
            var random = boostPool[Random.Range(0, boostPool.Count)];
            if (!currentBoosts.Contains(random))
                currentBoosts.Add(random);
        }
        ui.DisplayShop(currentBoosts, currentConsumables);
        ui.UpdateMoneyUI();
        ui.UpdateRerollPriceUI(GetRerollCost()); // 🔁 met à jour le bouton !
    }
    public int GetRerollCost()
    {
        return cost + rerollCount;
    }
}