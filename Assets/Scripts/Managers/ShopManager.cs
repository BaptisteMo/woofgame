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
    

    [Header("UI")]
    public ShopUI ui; 
    private List<ConsumableData> currentConsumables;
    private List<BoostData> currentBoosts;
  

    public bool isAfterBoss = false;

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
    }


    public void RerollBoosts()
    {
        if (GameSession.Instance.playerMoney < 20) return; // reroll coûte 20 par ex.
        GameSession.Instance.playerMoney -= 20;
        GenerateShop(); // regenère seulement les boosts ici dans la version propre
    }

}